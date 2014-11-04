using System;
using System.Globalization;
using Assets.Scripts.Common;
using Assets.Scripts.Levels;
using Assets.Scripts.Missions;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Numeration;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{

    public class OverlayUI : MonoBehaviour
    {
        public ProgressBar StepsProgress;
        public Image FirstStar;
        public Image SecondStar;
        public Image ThirdStar;
        public GameObject StarsContainer;
        public Text StepsCount;
        public Button TryAgainButton;
        public Button MenuButton;
        public Image MissionIcon;

        public GameObject TopContainer;
        public GameObject BottomContainer;

        private Animator topAnimator;
        private Animator bottomAnimator;

        private const string TopShowing = "TopIn";
        private const string TopHidding = "TopOut";

        private const string BottomShowing = "BottomIn";
        private const string BottomHidding = "BottomOut";

        #region ShowMode management

        public ShowMode Mode;
        private ShowMode currentMode;
        public ShowMode CurrentMode
        {
            get { return currentMode; }
             set
            {
                currentMode = value;
                UpdateCurrentMode();
            }
        }

        public enum ShowMode
        {
            ShowAll,
            OnlyMenu,
            Hide
        }
        public void UpdateMode(ShowMode mode)
        {
            try
            {
                if (mode == ShowMode.Hide)
                {
                    gameObject.SetActive(false);
                }
                else if (mode == ShowMode.OnlyMenu)
                {
                    gameObject.SetActive(true);
                    TryAgainButton.gameObject.SetActive(false);
                    StepsCount.gameObject.SetActive(false);
                    ThirdStar.gameObject.SetActive(false);
                    SecondStar.gameObject.SetActive(false);
                    FirstStar.gameObject.SetActive(false);
                    //StepsProgress.gameObject.SetActive(false);
                    MenuButton.gameObject.SetActive(true);
                }
                else if (mode == ShowMode.ShowAll)
                {
                    gameObject.SetActive(true);
                    TryAgainButton.gameObject.SetActive(true);
                    StepsCount.gameObject.SetActive(true);
                    ThirdStar.gameObject.SetActive(true);
                    SecondStar.gameObject.SetActive(true);
                    FirstStar.gameObject.SetActive(true);
                    StepsProgress.gameObject.SetActive(true);
                    MenuButton.gameObject.SetActive(true);
                }
            }
            catch 
            {
                Debug.LogError("Error while updating show mode of overlay ui. Make sure that all variables initialized in inspector.");
            }
        }

        public void UpdateCurrentMode()
        {
            UpdateMode(CurrentMode);
        }

#if UNITY_EDITOR
        public void ApplyStartMode()
        {
            UpdateMode(Mode);
        }
#endif
        public void ResetShowMode()
        {

            CurrentMode = Mode;
        }

        #endregion

        private void Start()
        {
            ResetShowMode();
            currentStars = StarsCount.ThreeStar;

            if (TopContainer != null && BottomContainer != null)
            {
                topAnimator = TopContainer.GetComponent<Animator>();
                bottomAnimator = BottomContainer.GetComponent<Animator>();
            }
        }


        public void Invalidate()
        {
            var level = Level.Current;

            var remainingSteps = level.RemainingSteps;
            var maxSteps = level.CurrentMaxSteps;
            var stars = level.CurrentStars;

            if (MissionIcon != null)
            {
                MissionIcon.sprite = level.Mission.Icon;
            }

            if (StepsProgress != null)
            {
                if (stars != StarsCount.OneStar)
                {
                    StepsProgress.gameObject.SetActive(true);
                    StepsProgress.SetValues(remainingSteps, maxSteps);
                }
                else
                {
                    StepsProgress.gameObject.SetActive(false);
                    StepsCount.gameObject.SetActive(false);
                }
            }

            if (StepsCount != null && stars != StarsCount.OneStar && remainingSteps != -1)
            {
                StepsCount.gameObject.SetActive(true);
                StepsCount.text = remainingSteps.ToString(CultureInfo.InvariantCulture);
            }

            SetStarsCount(stars);

        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void PlayShowAnimation()
        {
            topAnimator.Play(TopShowing);
            bottomAnimator.Play(BottomShowing);
        }

        public void PlayHideAnimation()
        {
            topAnimator.Play(TopHidding);
            bottomAnimator.Play(BottomHidding);
        }

        #region stars management


        private StarsCount currentStars;

        private void SetStarsCount(StarsCount count)
        {
            if (count == StarsCount.TwoStar && currentStars == StarsCount.ThreeStar)
            {
                OffsetStarsForward();
                DisposeStar(StarsCount.ThreeStar);
            }
            else if (count == StarsCount.OneStar && currentStars == StarsCount.ThreeStar)
            {
                OffsetStarsForward(2);
                DisposeStar(StarsCount.ThreeStar);
                DisposeStar(StarsCount.TwoStar);
            }
            else if (count == StarsCount.OneStar && currentStars == StarsCount.TwoStar)
            {
                OffsetStarsForward();
                DisposeStar(StarsCount.TwoStar);
            }
            else if (count == StarsCount.ThreeStar && currentStars == StarsCount.OneStar)
            {
                OffsetStarsBackward(2);
                ShowStar(StarsCount.TwoStar);
                ShowStar(StarsCount.ThreeStar);
            }
            else if (count == StarsCount.ThreeStar && currentStars == StarsCount.TwoStar)
            {
                OffsetStarsBackward();
                ShowStar(StarsCount.ThreeStar);
            }
            else if (count == StarsCount.ThreeStar && currentStars == StarsCount.None)
            {
                OffsetStarsBackward(3);
                ShowStar(StarsCount.OneStar);
                ShowStar(StarsCount.TwoStar);
                ShowStar(StarsCount.ThreeStar);
            }

            currentStars = count;
        }


        private void PlayStarAnimation(GameObject starObject, string animationStateName)
        {
            var animator = starObject.GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogWarning("PlayStarAnimation(): passed wrong star object");
                return;
            }

            animator.Play(animationStateName);
        }
        private GameObject GetStarObject(StarsCount star)
        {
            switch (star)
            {
                case StarsCount.OneStar:
                    return FirstStar.gameObject;
                case StarsCount.TwoStar:
                    return SecondStar.gameObject;
                case StarsCount.ThreeStar:
                    return ThirdStar.gameObject;
            }
            return null;
        }

        private void DisposeStar(GameObject starObject)
        {
            PlayStarAnimation(starObject, "StarDisposing");

        }
        private void DisposeStar(StarsCount star)
        {

            DisposeStar(GetStarObject(star));
        }

        private void OffsetStars(float offset)
        {
            iTween.MoveTo(StarsContainer, new Vector3(StarsContainer.transform.position.x + offset, StarsContainer.transform.position.y, StarsContainer.transform.position.z), 0.2f);
        }

        private float GetStarsOffsetValue()
        {
            return 0.24f; // TODO: calculate this shit
            //var starWidth = GetStarObject(StarsCount.OneStar).GetComponent<RectTransform>().rect.width;
            //return starWidth;
        }

        private void OffsetStarsForward(int power = 1)
        {
            OffsetStars(Math.Abs(GetStarsOffsetValue() * power));
        }

        private void OffsetStarsBackward(int power = 1)
        {
            OffsetStars(-Math.Abs(GetStarsOffsetValue() * power));
        }

        private void ShowStar(GameObject starObject)
        {
            PlayStarAnimation(starObject, "StarIdle");
        }
        private void ShowStar(StarsCount star)
        {
            ShowStar(GetStarObject(star));
        }

        #endregion

        #region mission management
        private void SetMissionUI(LevelMission mission)
        {
            
        }

        #endregion

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.H)) PlayHideAnimation();
            else if(Input.GetKeyDown(KeyCode.G)) PlayShowAnimation(); 

        }

        #region event handlers
        public void OnResetClick()
        {
            Level.Current.ResetLevel();
        }
        public void OnMenuClick()
        {
            Level.Current.ShowPauseMenu();
        }

        public void OnMissionIconClick()
        {
            Level.Current.ShowHelpPopup();
        }

        #endregion
    }
}
