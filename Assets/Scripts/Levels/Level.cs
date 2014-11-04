using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Assets.Scripts.Camera;
using Assets.Scripts.Camera.Effects;
using Assets.Scripts.GameGUI.Shop;
using Assets.Scripts.Levels.Style;
using Assets.Scripts.Levels.Style.ElementsColorization;
using Assets.Scripts.Map;
using Assets.Scripts.Map.Decorations;
using Assets.Scripts.Map.Items;
using Assets.Scripts.Missions;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Numeration;
using Assets.Scripts.Styles;
using Assets.Scripts.UI;
using UnityEngine;
using Debug = UnityEngine.Debug;
using GradientBackground = Assets.Scripts.Styles.Gradient.GradientBackground;

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

#if UNITY_EDITOR
            LevelValidator.Validate(this);
#endif

            InitializeShopInteraction();
            ResetScore();
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
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetLevel();
            }
        }


        private void OnDisable()
        {
            current = null;
            DropShopInteraction();
        }

        private static Level current;
        public static Level Current
        {
            get { return current ?? (current = FindObjectOfType<Level>()); }
        }

        [SerializeField]
        public LevelIndex Index;

        #region Level core components 
        public LevelMission Mission { get; private set; }
        public GameMap LevelMap { get; private set; }
        public OverlayUI OverlayUI { get; private set; }

        public void FetchCoreComponents()
        {
            Mission = FindObjectOfType<LevelMission>();
            LevelMap = FindObjectOfType<GameMap>();
            OverlayUI = FindObjectOfType<OverlayUI>();
        }



        #endregion

        #region UI management

        public Canvas LevelResultsPrefab;
        private Canvas currentLevelResults;

        public Canvas MenuPrefab;
        public Canvas HelpPopupPrefab;
        private PopupUI currentPopup;

        public void UpdateOverlayUI()
        {
            if (OverlayUI != null)
                OverlayUI.Invalidate();
        }

        private void EstablishUIPopup(GameObject canvasGameObject)
        {
            EstablishUIPopup(canvasGameObject.GetComponent<Canvas>());
        }
        private void EstablishUIPopup(Canvas canvas)
        {
            canvas.worldCamera = UnityEngine.Camera.main;
            canvas.planeDistance = 1;
        }

        private void ShowPopup(Canvas popupUICanvasPrefab)
        {
            CameraBlurEffect.BlurIn();
            OverlayUI.PlayHideAnimation();
            StartCoroutine(WaitAndShowPopup(0.3f, popupUICanvasPrefab));



        }

        private IEnumerator WaitAndShowPopup(float t, Canvas popupUIPRefab)
        {

            yield return new WaitForSeconds(t);
            var popup = (Instantiate(popupUIPRefab.gameObject) as GameObject);
            currentPopup = popup.GetComponent<PopupUI>();
        }

        private void HideCurrentPopup()
        {
            if (currentPopup != null)
            {
                OverlayUI.PlayShowAnimation();
                CameraBlurEffect.BlurOut();
                Destroy(currentPopup.gameObject);
                currentPopup = null;
            }
            else
            {
               
            }
        }

        #endregion

        #region game progress management
        private void RegisterLevelResults()
        {
            
        }

        #endregion

        #region Shop integration

        private void InitializeShopInteraction()
        {
            ShopPopup.Opened += (sender, args) => LockInput();
            ShopPopup.Closed += (sender, args) => Play();
        }

        private void DropShopInteraction()
        {
            
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

        private DecorationPlaymode GetDecorationPlaymodeByContext(DecorationsContext context)
        {
            if(context == DecorationsContext.Afterplay) return DecorationPlaymode.Out;
            else if(context == DecorationsContext.Preplay) return DecorationPlaymode.In;
            return DecorationPlaymode.In;
        }

        private void StartDecorationsAndHandleContext(DecorationsContext context)
        {
            var playmode = GetDecorationPlaymodeByContext(context);
            currentDecorationsContext = context;
            Decorator.Play(playmode);
            
            if(context == DecorationsContext.Afterplay)
                FireAction(LevelActionEvent.LevelActionType.AfterplayBegin);
            else if (context == DecorationsContext.Preplay)
                FireAction(LevelActionEvent.LevelActionType.PreplayBegin);

        }


        #endregion

        #region Fade management

        private enum FadeContextType
        {
            PlayStarted,
            MenuOpen,
            Reset
        }

        private FadeContext currentFadeContext;


        private class FadeContext
        {

            public static FadeContext MenuOpen = new FadeContext(FadeContextType.MenuOpen, 0.3f, UnityEngine.Camera.main.backgroundColor, FadeMode.To, 1);

            public FadeContext(FadeContextType contextType, float duration, Color color, FadeMode mode, float amount)
            {
                ContextType = contextType;
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


            public FadeContextType ContextType { get; set; }
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
            OnFadeEnd(currentFadeContext);
        }

        private void StartFadeOperationAndHandleContext(FadeContext context)
        {

            OnFadeEnd(context); // temp
            return;

            var hash = context.CreateITweenHashtable();
            var texture = context.GenerateFadeTexture();

            iTween.CameraFadeAdd(texture);

            hash.Add("oncomplete", "OnITweenFadeDone");
            hash.Add("oncompletetarget", gameObject);

            iTween.CameraFadeTo(hash);

            currentFadeContext = context;
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


        public int CurrentSteps { get; private set; }
        public int CurrentMaxSteps { get; private set; }

        public int RemainingSteps
        {
            get
            {
                if (CurrentMaxSteps == 0) return -1; 
                return CurrentMaxSteps - CurrentSteps ; }
        }

        public StarsCount CurrentStars { get; private set; }

        public int GetMaxStepsForStar(StarsCount star)
        {
            switch (star)
            {
                case StarsCount.None:   
                case StarsCount.OneStar:
                    return -1;
                case StarsCount.TwoStar:
                    return StepsForTwoStars;
                case StarsCount.ThreeStar:
                    return StepsForThreeStars;
            }
            return -1;
        }

        private void ResetScore()
        {

            CurrentStars = StarsCount.ThreeStar;
            CurrentSteps = 0;
            CurrentMaxSteps = GetMaxStepsForStar(CurrentStars);

            UpdateOverlayUI();
        }

        #endregion

        #region Events management

        private void RegisterPlayerStep()
        {

            if (CurrentStars == StarsCount.OneStar)
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
                LevelInitialized,
                PreplayBegin,
                AfterplayBegin
            }

            public LevelActionType Type { get; private set; }

        }

        private void FireAction(LevelActionEvent.LevelActionType actionType)
        {
            ProcessEvent(new LevelActionEvent(actionType));
        }

        public void ProcessEvent(LevelEvent e, params Type[] targets)
        {



            foreach (var element in GetLevelElements())
            {

                if ((e.Element != element || e.Element == null))
                {
                    element.ProcessEvent(e);
                }
            }

            if (e is Player.PlayerOutsideEvent)
            {
                RegisterPlayerOutside();
            }
            else if (e is Player.PlayerStepEvent)
            {
                RegisterPlayerStep();
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
                    OnDecorationsEnd(currentDecorationsContext);
                }


            }


        }
        #endregion

        #region Level actions (start, end, reset, open/close menu, results menu)

        public void HideLevel()
        {
            if(OverlayUI != null) OverlayUI.Disable();
            GradientBackground.MainGradient.AlignToFrontAnchor();

        }

        public void UnhideLevel()
        {
            if (OverlayUI != null) OverlayUI.Enable();
            GradientBackground.MainGradient.AlignToBackAnchor();
        }


        public void EndLevel()
        {
            LockInput();
            RegisterLevelResults();
            StartDecorationsAndHandleContext(DecorationsContext.Afterplay);
        }

        public void StartLevel()
        {
            ResetScore();
            LockInput();
            StartDecorationsAndHandleContext(DecorationsContext.Preplay);
        }

        public void ResetLevel()
        {
            LockInput();
            HideCurrentPopup();
            UnhideLevel();

            if (currentLevelResults != null)
                CloseResultsMenu();

            FireAction(LevelActionEvent.LevelActionType.LevelReset);

            StartLevel();
            
        }

        public void Exit()
        {
            Application.LoadLevel("GUI");
        }

        public void ShowPauseMenu()
        {
            Pause();
            ShowPopup(MenuPrefab);
            FireAction(LevelActionEvent.LevelActionType.PauseMenuOpen);
        }

        public void HidePauseMenu()
        {
            Play();
            HideCurrentPopup();
            FireAction(LevelActionEvent.LevelActionType.PauseMenuClosed);
        }


        public void OpenResultsMenu(LevelResultsUIModel model)
        {
            if (currentLevelResults != null)
            {
                Debug.LogWarning("Results menu already opened");
                return;
            }
            HideLevel();

            currentLevelResults = (Instantiate(LevelResultsPrefab.gameObject) as GameObject).GetComponent<Canvas>();
            var currentLevelResultsUI = currentLevelResults.GetComponent<LevelResultsUI>();


            if (currentLevelResultsUI.GetComponent<UIColored>() != null)
            {
                var styleProvider = StyleProvider.Main;
                if(styleProvider != null)
                {
                    var style = styleProvider.GetStyle();
                    if (style != null)
                    {
                        currentLevelResultsUI.GetComponent<UIColored>().Color = style.GetFrontColor();
                    }
                }

            }

            currentLevelResultsUI.Level = this;
            currentLevelResultsUI.Model = model;
            EstablishUIPopup(currentLevelResultsUI.gameObject);


        }

        public void CloseResultsMenu()
        {
            if (currentLevelResults == null)
            {
                Debug.LogWarning("results menu not found");
                return;
            }

            //UnhideLevel();
            Destroy(currentLevelResults.gameObject);

        }


        public void ShowHelpPopup()
        {
            Pause();
            ShowPopup(HelpPopupPrefab);
        }

        public void HideHelpPopup()
        {

            Play();
            HideCurrentPopup();
        }

        #endregion

        #region fade and decorations events management

        private void OnDecorationsEnd(DecorationsContext context)
        {


            if (context == DecorationsContext.Preplay)
            {
                OnPreplayEnd();
            }
            else if (context == DecorationsContext.Afterplay)
            {
                OnAfterplayEnd();
            }
        }

        private void OnFadeEnd( FadeContext fadeContext)
        {
            if (fadeContext.ContextType == FadeContextType.MenuOpen)
            {
                
            }
        }

        private void OnPreplayEnd()
        {
            UpdateOverlayUI();
            FireAction(LevelActionEvent.LevelActionType.LevelStarted);
            Play();
        }

        private void OnAfterplayEnd()
        {
            HideLevel();
            FireAction(LevelActionEvent.LevelActionType.LevelEnd);
            OpenResultsMenu(new LevelResultsUIModel());
        }

        #endregion


    }
}
