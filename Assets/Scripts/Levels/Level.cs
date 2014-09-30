using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Camera;
using Assets.Scripts.Map;
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

        public GameMap LevelMap;
        public LevelTopUI TopUI;

        public Canvas LevelResultsPrefab;
        private Canvas currentLevelResults;

        public Canvas MenuPrefab;
        private Canvas currentMenu;

        public int StepsForThreeStars;
        public int StepsForTwoStars;
        public int StepsForOneStar;


        public IEnumerable<LevelElement> FindLevelElements()
        {
            return FindObjectsOfType<LevelElement>();
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
                foreach (var element in FindLevelElements())
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

            Play();

            CurrentStar = StarsCount.ThreeStar;
            CurrentStepsTarget = GetStepsTargetForStar(StarsCount.ThreeStar);
            ResetScore();
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

        public void RegisterPlayerStep()
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
        public void RegisterPlayerOutside()
        {
            Reset();
        }

        public void RegesterPlayerMoveBegin(MapItemMove move)
        {
            foreach (var element in FindLevelElements())
            {
                element.OnPlayerMoveBegin(LevelMap.Player, move);
            }
        }

        public void RegisterPlayerMoveEnd(MapItemMove move)
        {
            foreach (var element in FindLevelElements())
            {
                element.OnPlayerMoveEnd(LevelMap.Player, move);
            }
        }

        public void OpenMenu()
        {
            Pause();
            TopUI.CurrentMode = LevelTopUI.ShowMode.Hide;
            CameraFade.FadeOut(UnityEngine.Camera.main.backgroundColor, 0.3f, () =>
                                                                              {


                                                                                  foreach (var element in FindLevelElements())
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

            foreach (var element in FindLevelElements())
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

            foreach (var element in FindLevelElements())
            {
                element.OnLevelReset();
            }

            CameraFade.FadeOut(0.3f, () =>
                                     {
                                         ResetScore();
                                         LevelMap.Reset();

                                         foreach (var e in FindLevelElements())
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

                                                                                  foreach (var element in FindLevelElements())
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
