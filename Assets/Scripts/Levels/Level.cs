using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Assets.Scripts.Camera;
using Assets.Scripts.Levels.Model;
using Assets.Scripts.Levels.Style;
using Assets.Scripts.Map;
using Assets.Scripts.Map.Decorations;
using Assets.Scripts.Map.Items;
using Assets.Scripts.Missions;
using Assets.Scripts.UI;
using UnityEngine;
using Debug = UnityEngine.Debug;
using GradientBackground = Assets.Scripts.Levels.Style.GradientBackground.GradientBackground;

namespace Assets.Scripts.Levels
{
    public class Level : MonoBehaviour
    {


        private void Start()
        {

#if UNITY_EDITOR

            var sw = new Stopwatch();
            sw.Start();
#endif

            FetchCoreComponents();
            LoadDatabase();

#if UNITY_EDITOR
            ValidateLevel();
#endif

            ResetScore();
            LevelStyleUtils.SetColor();
            UnhideLevel();
            InvalidateLevelElements();
            StartLevel();
            FireAction(LevelActionEvent.LevelActionType.LevelInitialized);

#if UNITY_EDITOR

            sw.Stop();
            //Debug.Log("Level.Start(): " + sw.Elapsed.TotalSeconds);

#endif
        }

        private void Update()
        {
        }

        private static Level current;
        public static Level Current
        {
            get { return current ?? (current = FindObjectOfType<Level>()); }
        }

        [SerializeField]
        public LevelIndex Index;

        #region Level core components 
        public LevelSolution Solution { get; private set; }
        public LevelMission Mission { get; private set; }

        public GameMap LevelMap { get; private set; }
        public OverlayUI OverlayUI { get; private set; }

        public void FetchCoreComponents()
        {
            Solution = FindObjectOfType<LevelSolution>();
            Mission = FindObjectOfType<LevelMission>();
            LevelMap = FindObjectOfType<GameMap>();
            OverlayUI = FindObjectOfType<OverlayUI>();
        }


        #endregion

        #region UI instances and prefabs
        public Canvas LevelResultsPrefab;
        private Canvas currentLevelResults;

        public Canvas MenuPrefab;
        private Canvas currentPauseMenu;
        #endregion

        #region game progress management
        private void RegisterLevelResults()
        {
            
        }

        #endregion

        #region Database management

        private void LoadDatabase()
        {
            levelsDatabase = Resources.Load<LevelsDatabase>("LevelsDatabase");

            if (levelsDatabase == null)
            {
                Debug.LogWarning("Level.levelsDatabase == null");
            }

            DontDestroyOnLoad(levelsDatabase);
        }


        private LevelsDatabase levelsDatabase;
        public LevelModel CreateLevelModel()
        {
            return new LevelModel(Index.GetScenePath(false), LevelMissionModel.EnterTarget, Index.WorldNumber,
                Index.LevelNumber);
        }
        #endregion

        #region Decoration management
        private enum DecorationsContext
        {
            Preplay,
            Afterplay
        }

        private DecorationsContext currentDecorationsContext;

        private Decorator decorator;
        public Decorator Decorator
        {
            get { return decorator ?? (decorator = FindObjectOfType<Decorator>()); }
        }

        private void StartDecorations(DecorationsContext context, DecorationPlaymode playmode)
        {
            currentDecorationsContext = context;
            Decorator.Play(playmode);
        }


        #endregion

        #region Fade management

        private enum FadeContext
        {
            PlayStarted,
            MenuOpen,
            Reset
        }

        private FadeVariant currentFadeVariant;


        private class FadeVariant
        {

            public static FadeVariant MenuOpen = new FadeVariant(FadeContext.MenuOpen, 0.3f, UnityEngine.Camera.main.backgroundColor, FadeMode.To, 1);

            public FadeVariant(FadeContext context, float duration, Color color, FadeMode mode, float amount)
            {
                Context = context;
                Duration = duration;
                Color = color;
                Mode = mode;
                Amount = amount;
            }

            public enum FadeMode
            {
                To,
                From
            }


            public FadeContext Context { get; set; }
            public float Duration { get; set; }
            public Color Color { get; set; }
            public FadeMode Mode { get; set; }

            public float Amount { get; set; }

            public Texture2D GenerateFadeTexture()
            {
                return iTween.CameraTexture(Color);
            }

            public Hashtable CreateITweenHashtable()
            {
                return iTween.Hash("time", Duration, "amount", Amount);
            }
        }

        private void OnITweenFadeDone()
        {
            iTween.CameraFadeDestroy();
            OnFadeEnd(currentFadeVariant);
        }

        private void StartFadeOperation(FadeVariant variant)
        {

            OnFadeEnd(variant); // temp
            return;

            var hash = variant.CreateITweenHashtable();
            var texture = variant.GenerateFadeTexture();

            iTween.CameraFadeAdd(texture);

            hash.Add("oncomplete", "OnITweenFadeDone");
            hash.Add("oncompletetarget", gameObject);

            iTween.CameraFadeTo(hash);

            currentFadeVariant = variant;
        }

        #endregion

        #region Level elements management

        private IEnumerable<LevelElement> elements;
        public void InvalidateLevelElements()
        {
            elements = FindObjectsOfType<LevelElement>();
        }
        public IEnumerable<LevelElement> GetLevelElements()
        {
            if (elements == null)
            {
                InvalidateLevelElements();
            }
            return elements;
        }

        #endregion

        #region State management

        public class LevelStateChangedEvent : LevelEvent
        {
            public LevelStateChangedEvent(LevelState oldState, LevelState newState)
            {
                OldState = oldState;
                NewState = newState;
            }

            public LevelState OldState { get; private set; }
            public LevelState NewState { get; private set; }
        }

        public LevelState State { get; private set; }

        private void ChangeState(LevelState newState)
        {
            var oldState = State;
            State = newState;

            ProcessEvent(new LevelStateChangedEvent(oldState, State));
                

        }

        public void Play()
        {
            State = LevelState.Playing;
            ChangeState(LevelState.Playing);
        }

        public void LockInput()
        {
            ChangeState(LevelState.NoInput);
        }

        public void Pause()
        {
            ChangeState(LevelState.Paused);
        }

        #endregion

        #region Steps & stars management

        public int StepsForThreeStars;
        public int StepsForTwoStars;
        public int StepsForOneStar;

        public void UpdateOverlayUI()
        {
            if(OverlayUI != null)
                OverlayUI.Invalidate();
        }

        public int CurrentSteps { get; private set; }
        public int CurrentMaxSteps { get; private set; }

        public int RemainingSteps
        {
            get { return CurrentMaxSteps - CurrentSteps ; }
        }

        public StarsCount CurrentStars { get; private set; }

        public int GetMaxStepsForStar(StarsCount star)
        {
            switch (star)
            {
                case StarsCount.None:
                    return -1;
                case StarsCount.OneStar:
                    return StepsForOneStar;
                case StarsCount.TwoStar:
                    return StepsForTwoStars;
                case StarsCount.ThreeStar:
                    return StepsForThreeStars;
            }
            return StepsForOneStar;
        }

        private void ResetScore()
        {

            CurrentStars = StarsCount.ThreeStar;
            CurrentSteps = 0;
            CurrentMaxSteps = GetMaxStepsForStar(CurrentStars);

            UpdateOverlayUI();
        }

        #endregion

        #region Validators

        private void ValidateLevel()
        {
#if UNITY_EDITOR


            if (LevelMap == null)
            {
                Debug.LogWarning("Level.Map == null");
            }

            ValidateSolution();
            ValidateMission();
            ValidateSteps();
            ValidateLevelIndex();
#endif
        }
        private void ValidateSteps()
        {
            if (StepsForOneStar == 0
                || StepsForTwoStars == 0
                || StepsForThreeStars == 0)
            {
                Debug.LogWarning("Invalid level step limits. EDIT steps limits in level inspector. Ащк this run steps limits will be sets by default.");

                StepsForOneStar = 15;
                StepsForTwoStars = 10;
                StepsForThreeStars = 5;
            }
        }

        private void ValidateLevelIndex()
        {
            if (Index.LevelNumber == 0)
            {
                Debug.LogWarning("Levent number or level world not found");
            }
        }
        private void ValidateSolution()
        {
            if (Solution == null) Solution = FindObjectOfType<LevelSolution>();


        }
        private void ValidateMission()
        {

            if (Mission == null)
            {
                var mission = FindObjectOfType<LevelMission>();
                if (mission == null)
                {
                    var missionGO = new GameObject("Mission");

                    if (LevelMap.FindItemsOfType<Map.Items.Target>().Any())
                    {
                        Mission = missionGO.AddComponent<EnterTargetMission>();
                    }
                    else
                    {
                        Mission =  missionGO.AddComponent<DestroyAllWallsMission>();
                    }


                    Debug.LogWarning("ValidateMission(): Mission for level not found. Initializing mission by temp value (" + Mission.GetType().Name + ")");

                }
                else Mission = mission;

            }
            
                if (Mission is DestroyAllWallsMission && LevelMap.FindItemsOfType<Map.Items.Target>().Any())
                {
                    Debug.LogWarning("ValidateMission(): Unexpected target found.");
                }
                else if (Mission is EnterTargetMission && !LevelMap.FindItemsOfType<Map.Items.Target>().Any())
                {
                    Debug.LogWarning("ValidateMission(): For EnterTargetMission target not found");
                }
        }
        #endregion

        #region Events management

        private void RegisterPlayerStep()
        {

            if (CurrentStars == StarsCount.None)
                return;


            CurrentSteps++;

            if (CurrentSteps > CurrentMaxSteps-1)
            {
                CurrentStars--;
                CurrentSteps = 0;
                CurrentMaxSteps = GetMaxStepsForStar(CurrentStars);
            }

            UpdateOverlayUI();
        }
        private void RegisterPlayerOutside()
        {
            ResetLevel();
        }

        public class LevelActionEvent : LevelEvent
        {
            public LevelActionEvent(LevelActionType type)
            {
                Type = type;
            }

            public enum LevelActionType
            {
                LevelStarted,
                LevelReset,
                LevelEnd,
                PauseMenuOpen,
                PauseMenuClosed,
                LevelInitialized
            }

            public LevelActionType Type { get; private set; }

        }

        private void FireAction(LevelActionEvent.LevelActionType actionType)
        {
            ProcessEvent(new LevelActionEvent(actionType));
        }

        public void ProcessEvent(LevelEvent e, params Type[] targets)
        {


            if (e is Player.PlayerOutsideEvent)
            {
                RegisterPlayerOutside();
            }
            else if (e is Player.PlayerStepEvent)
            {
                RegisterPlayerStep();
            }
            else if (e is MapItem.MapItemMoveEvent && e.IsPlayer)
            {

            }
            else if (e is LevelMission.MissionDoneEvent)
            {
                EndLevel();
            }
            else if (e is Decorator.DecoratorEvent)
            {
                var se = e as Decorator.DecoratorEvent;
                if (se.Status == Decorator.DecoratorEvent.DecoratorStatus.Done)
                {
                    OnDecorationsEnd(se.PlayMode, currentDecorationsContext);
                }


            }

            foreach (var element in GetLevelElements())
            {

                if ((e.Element != element || e.Element == null))
                {
                    element.ProcessEvent(e);
                }
            }
        }
        #endregion

        #region Level actions (start, end, reset, open/close menu, results menu)

        public void HideLevel()
        {
            if(OverlayUI != null) OverlayUI.gameObject.SetActive(false);
            GradientBackground.MainGradient.AlignToFrontAnchor();

        }

        public void UnhideLevel()
        {
            if (OverlayUI != null) OverlayUI.gameObject.SetActive(true);
            GradientBackground.MainGradient.AlignToBackAnchor();
        }

        public void EndLevel()
        {
            LockInput();
            RegisterLevelResults();
            StartDecorations(DecorationsContext.Afterplay, DecorationPlaymode.Out);
        }

        public void StartLevel()
        {
            ResetScore();
            LockInput();
            StartDecorations(DecorationsContext.Preplay, DecorationPlaymode.In);
        }

        public void ResetLevel()
        {
            LockInput();

            if(currentPauseMenu != null)
                ClosePauseMenu();
            if (currentLevelResults != null)
                CloseResultsMenu();

            FireAction(LevelActionEvent.LevelActionType.LevelReset);

            StartLevel();
            
        }

        public void OpenPauseMenuAndPause()
        {
            OpenPauseMenu();
            Pause();
        }

        public void OpenPauseMenu()
        {
            StartFadeOperation(FadeVariant.MenuOpen);
        }

        public void ClosePauseMenu()
        {
            if (currentPauseMenu == null)
            {
                Debug.LogWarning("menu to close not found");
                return;
            }
            UnhideLevel();
            Destroy(currentPauseMenu.gameObject);
            FireAction(LevelActionEvent.LevelActionType.PauseMenuClosed);
        }

        public void ClosePauseMenuAndPlay()
        {
            ClosePauseMenu();
            Play();
        }

        public void OpenResultsMenu(LevelResultsUIModel model)
        {
            if (currentLevelResults != null)
            {
                Debug.LogWarning("Results menu already opened");
                return;
            }

            if (currentPauseMenu != null)
            {
                ClosePauseMenu();
            }

            HideLevel();

            currentLevelResults = Instantiate(LevelResultsPrefab);
            var currentLevelResultsUI = currentLevelResults.GetComponent<LevelResultsUI>();

            currentLevelResultsUI.Level = this;
            currentLevelResultsUI.Model = model;

            LevelStyleUtils.SetColor(currentLevelResults.gameObject, LevelStyleUtils.MainStyle.GetFrontColor());

        }

        public void CloseResultsMenu()
        {
            if (currentLevelResults == null)
            {
                Debug.LogWarning("results menu not found");
                return;
            }

            UnhideLevel();
            Destroy(currentLevelResults.gameObject);

        }

        #endregion

        #region fade and decorations events management

        private void OnDecorationsEnd(DecorationPlaymode playmode, DecorationsContext context)
        {
            if (context == DecorationsContext.Preplay)
            {
                UpdateOverlayUI();
                FireAction(LevelActionEvent.LevelActionType.LevelStarted);
                Play();
            }
            else if (context == DecorationsContext.Afterplay)
            {
                FireAction(LevelActionEvent.LevelActionType.LevelEnd);
                OpenResultsMenu(new LevelResultsUIModel());
            }
        }

        private void OnFadeEnd( FadeVariant fadeVariant)
        {
            if (fadeVariant.Context == FadeContext.MenuOpen)
            {
                HideLevel();
                if (currentPauseMenu != null)
                {
                    Debug.LogWarning("Menu already opened");
                    return;
                }
                currentPauseMenu = Instantiate(MenuPrefab);
                var menuUI = currentPauseMenu.GetComponent<MenuUI>();
                menuUI.Level = this;

                LevelStyleUtils.SetColor(currentPauseMenu.gameObject, LevelStyleUtils.MainStyle.GetFrontColor());

                FireAction(LevelActionEvent.LevelActionType.PauseMenuOpen);
            }
        }

        #endregion

    }
}
