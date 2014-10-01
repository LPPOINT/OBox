using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Camera;
using Assets.Scripts.Map;
using Assets.Scripts.Map.Items;
using Assets.Scripts.Missions;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Levels
{
    public class Level : MonoBehaviour
    {

        public static Level Current
        {
            get
            {
                return FindObjectOfType<Level>();
            }
        }

        public LevelSolution Solution;
        public LevelMission Mission;

        public GameMap LevelMap;
        public LevelTopUI TopUI;

        public Canvas LevelResultsPrefab;
        private Canvas currentLevelResults;

        public Canvas MenuPrefab;
        private Canvas currentMenu;

        public int StepsForThreeStars;
        public int StepsForTwoStars;
        public int StepsForOneStar;


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

        #region State controlling
        private LevelState state;
        public LevelState State
        {
            get { return state; }
            private set
            {
                var oldState = value;
                state = value;
                foreach (var element in GetLevelElements())
                {
                    element.OnLevelStateChanged(oldState, state);
                }
            }
        }

        public void Play()
        {
            State = LevelState.Playing;
        }

        public void LockInput()
        {
            State = LevelState.NoInput;
        }

        public void Pause()
        {
            State = LevelState.Paused;
        }

        #endregion

        public int GetStepsTargetForStar(StarsCount star)
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

        public int CurrentSteps { get; private set; }
        public int CurrentStepsTarget { get; private set; }

        public int RemainingSteps
        {
            get { return CurrentStepsTarget - CurrentSteps + 1; }
        }

        public StarsCount CurrentStar { get; private set; }

        public int LevelNumber
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Application.loadedLevelName.Substring(5));
                }
                catch
                {
                    Debug.LogWarning("Cant detect level number!");
                    return 0;
                }
            }
        }


        private void ResetScore()
        {

            CurrentStar = StarsCount.ThreeStar;
            CurrentSteps = 0;
            CurrentStepsTarget = GetStepsTargetForStar(CurrentStar);

            TopUI.StepsSlider.minValue = 0;
            TopUI.StepsSlider.maxValue = CurrentStepsTarget;
            TopUI.StepsSlider.value = RemainingSteps;
            TopUI.StepsCount.text = RemainingSteps.ToString();
            TopUI.StepsCount.fontStyle = FontStyle.Normal;

            SetUIStar(CurrentStar);
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

        protected void Start()
        {


            if (LevelMap == null)
            {
                Debug.LogWarning("Level.Map == null");
            }

            if (Solution == null) Solution = FindObjectOfType<LevelSolution>();

            if (Mission == null)
            {
                var mission = FindObjectOfType<LevelMission>();
                if (mission == null)
                {
                    var missionGO = new GameObject("Mission");
                    Mission = missionGO.AddComponent<EnterTargetMission>();
                }
                else Mission = mission;
            }

            Play();

            CurrentStar = StarsCount.ThreeStar;
            CurrentStepsTarget = GetStepsTargetForStar(StarsCount.ThreeStar);
            ResetScore();
            InvalidateLevelElements();

            foreach (var levelElement in GetLevelElements())
            {
                levelElement.OnLevelStarted();
            }
        }

        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (State != LevelState.Paused) Pause();
                else Play();
            }
        }


        private void DecrementUIStar()
        {

            if (CurrentStar == StarsCount.TwoStar)
            {
                TopUI.ThirdStar.GetComponent<Animator>().Play("StarDisposing");
            }
            else if (CurrentStar == StarsCount.OneStar)
            {
                TopUI.SecondStar.GetComponent<Animator>().Play("StarDisposing");
            }
            else if (CurrentStar == StarsCount.None)
            {
                TopUI.FirstStar.GetComponent<Animator>().Play("StarDisposing");
            }
        }
        private void IncrementUIStar()
        {
            if (CurrentStar == StarsCount.ThreeStar)
            {
                TopUI.ThirdStar.GetComponent<Animator>().Play("StarIdle");
            }
            else if (CurrentStar == StarsCount.TwoStar)
            {
                TopUI.SecondStar.GetComponent<Animator>().Play("StarIdle");
            }
            else if (CurrentStar == StarsCount.OneStar)
            {
                TopUI.FirstStar.GetComponent<Animator>().Play("StarIdle");
            }
        }

        private void SetUIStar(StarsCount count)
        {
            switch (count)
            {
                case StarsCount.None:
                    TopUI.SecondStar.GetComponent<Animator>().Play("StarDisposing");
                    TopUI.FirstStar.GetComponent<Animator>().Play("StarDisposing");
                    TopUI.ThirdStar.GetComponent<Animator>().Play("StarDisposing");
                    break;
                case StarsCount.OneStar:
                    TopUI.SecondStar.GetComponent<Animator>().Play("StarDisposing");
                    TopUI.ThirdStar.GetComponent<Animator>().Play("StarDisposing");
                    TopUI.FirstStar.GetComponent<Animator>().Play("StarIdle");
                    break;
                case StarsCount.TwoStar:
                    TopUI.SecondStar.GetComponent<Animator>().Play("StarIdle");
                    TopUI.ThirdStar.GetComponent<Animator>().Play("StarDisposing");
                    TopUI.FirstStar.GetComponent<Animator>().Play("StarIdle");
                    break;
                case StarsCount.ThreeStar:
                    TopUI.SecondStar.GetComponent<Animator>().Play("StarIdle");
                    TopUI.ThirdStar.GetComponent<Animator>().Play("StarIdle");
                    TopUI.FirstStar.GetComponent<Animator>().Play("StarIdle");
                    break;

            }
        }

        private void RegisterPlayerStep()
        {

            if (CurrentStar == StarsCount.None)
                return;


            CurrentSteps++;

            if (CurrentSteps > CurrentStepsTarget)
            {
                CurrentStar--;
                CurrentSteps = 0;

                if (CurrentStar == StarsCount.None)
                {
                    TopUI.StepsSlider.value = 0;
                    TopUI.StepsCount.text = "∞";
                    TopUI.StepsCount.fontStyle = FontStyle.Bold;
                    DecrementUIStar();
                    return;
                }

                CurrentStepsTarget = GetStepsTargetForStar(CurrentStar);

                TopUI.StepsSlider.minValue = 0;
                TopUI.StepsSlider.maxValue = CurrentStepsTarget;
                TopUI.StepsSlider.value = RemainingSteps;
                TopUI.StepsCount.text = RemainingSteps.ToString();
                TopUI.StepsCount.fontStyle = FontStyle.Normal;
                DecrementUIStar();

            }
            else
            {
                TopUI.StepsSlider.value = RemainingSteps;
                TopUI.StepsCount.text = RemainingSteps.ToString();
                TopUI.StepsCount.fontStyle = FontStyle.Normal;
            }

        }
        private void RegisterPlayerOutside()
        {
            Reset();
        }

        private void RegesterPlayerMoveBegin(MapItemMove move)
        {
            foreach (var element in GetLevelElements())
            {
                element.OnPlayerMoveBegin(LevelMap.Player, move);
            }
        }

        private void RegisterPlayerMoveEnd(MapItemMove move)
        {
            foreach (var element in GetLevelElements())
            {
                element.OnPlayerMoveEnd(LevelMap.Player, move);
            }
        }

        public void ProcessEvent(LevelEvent e)
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
                var moveEvent = e as MapItem.MapItemMoveEvent;

                if(moveEvent.State == MapItem.MapItemMoveEvent.MoveState.Started) RegesterPlayerMoveBegin(moveEvent.Move);
                else RegisterPlayerMoveEnd(moveEvent.Move);

            }
            else if (e is LevelMission.MissionDoneEvent)
            {
                EndLevel();
            }

            foreach (var element in GetLevelElements())
            {
                if (e.Element != element)
                {
                    element.ProcessEvent(e);
                }
            }
        }

        public void OpenMenu()
        {
            Pause();
            TopUI.CurrentMode = LevelTopUI.ShowMode.Hide;
            CameraFade.FadeOut(UnityEngine.Camera.main.backgroundColor, 0.3f, () =>
                                                                              {


                                                                                  foreach (var element in GetLevelElements())
                                                                                  {
                                                                                      element.OnMenuOpen();
                                                                                  }

                                                                                  currentMenu = Instantiate(MenuPrefab);
                                                                                  currentMenu.GetComponent<MenuUI>()
                                                                                      .Level = this;
                                                                              });
        }

        public void CloseMenu()
        {
            CloseMenu(null);
        }

        public void CloseMenu(Action afterClosed)
        {
            Play();
            TopUI.RevertMode();

            foreach (var element in GetLevelElements())
            {
                element.OnMenuClosed();
            }

            if (currentMenu != null)
            {
                currentMenu.GetComponent<MenuUI>().Close();
            }
        }


        public void Reset()
        {

            if (LevelMap == null)
            {
                Debug.LogWarning("Reset(): LevelMap == null");
                return;
            }

            Play();
            TopUI.RevertMode();


            if (currentLevelResults != null)
            {
                Destroy(currentLevelResults.gameObject);
                currentLevelResults = null;
            }


            CurrentSteps = 0;

            foreach (var element in GetLevelElements())
            {
                element.OnLevelReset();
            }

            CameraFade.FadeOut(0.3f, () =>
                                     {
                                         ResetScore();
                                         LevelMap.Reset();

                                         foreach (var e in GetLevelElements())
                                         {
                                             e.OnLevelStarted();
                                         }

                                     });


        }

        public void EndLevel()
        {
            Pause();
            TopUI.CurrentMode = LevelTopUI.ShowMode.Hide;

            CameraFade.FadeOut(UnityEngine.Camera.main.backgroundColor, 0.3f, () =>
                                                                              {

                                                                                  foreach (var element in GetLevelElements())
                                                                                  {
                                                                                      element.OnLevelEnded();
                                                                                  }

                                                                                  currentLevelResults =
                                                                                      Instantiate(LevelResultsPrefab);
                                                                                  currentLevelResults
                                                                                      .GetComponent<LevelResultsUI>()
                                                                                      .Level = this;
                                                                              });



        }


        public void LoadNextLevel()
        {
            Application.LoadLevel("Level" + (LevelNumber + 1));
        }

        public void LoadLevel(int l)
        {
            Application.LoadLevel("Level" + l);
        }

    }
}
