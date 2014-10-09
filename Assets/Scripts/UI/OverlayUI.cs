﻿using System;
using System.Globalization;
using Assets.Scripts.Levels;
using Assets.Scripts.Missions;
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
        public Text StepsCount;
        public Button TryAgainButton;
        public Button MenuButton;
        public Image MissionIcon;

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
        }


        public void Invalidate()
        {
            var level = Level.Current;

            var remainingSteps = level.RemainingSteps;
            var maxSteps = level.CurrentMaxSteps;
            var stars = level.CurrentStars;

            if (StepsProgress != null)
            {
                if (stars != StarsCount.None)
                {
                    StepsProgress.gameObject.SetActive(true);
                    StepsProgress.SetValues(remainingSteps, maxSteps);
                }
                else StepsProgress.gameObject.SetActive(false);
            }

            if (StepsCount != null)
            {
                if (remainingSteps == -1)
                {
                    
                }
                else
                {
                    StepsCount.text = remainingSteps.ToString(CultureInfo.InvariantCulture);
                }
            }

            SetStarsUICount(stars);

        }


        #region stars management
        private void SetStarsUICount(StarsCount count)
        {

            if(FirstStar == null || SecondStar == null || ThirdStar == null) return;

            switch (count)
            {
                case StarsCount.None:
                    SecondStar.GetComponent<Animator>().Play("StarDisposing");
                    FirstStar.GetComponent<Animator>().Play("StarDisposing");
                    ThirdStar.GetComponent<Animator>().Play("StarDisposing");
                    break;
                case StarsCount.OneStar:
                    SecondStar.GetComponent<Animator>().Play("StarDisposing");
                    ThirdStar.GetComponent<Animator>().Play("StarDisposing");
                    FirstStar.GetComponent<Animator>().Play("StarIdle");
                    break;
                case StarsCount.TwoStar:
                    SecondStar.GetComponent<Animator>().Play("StarIdle");
                    ThirdStar.GetComponent<Animator>().Play("StarDisposing");
                    FirstStar.GetComponent<Animator>().Play("StarIdle");
                    break;
                case StarsCount.ThreeStar:
                    SecondStar.GetComponent<Animator>().Play("StarIdle");
                    ThirdStar.GetComponent<Animator>().Play("StarIdle");
                    FirstStar.GetComponent<Animator>().Play("StarIdle");
                    break;

            }
        }

        private void IncrementStars()
        {
            
        }

        private void DecrementStars()
        {
            
        }
        #endregion

        #region mission management
        private void SetMissionUI(LevelMission mission)
        {
            
        }

        #endregion



        #region event handlers
        public void OnResetClick()
        {
            Level.Current.ResetLevel();
        }
        public void OnMenuClick()
        {
            Level.Current.OpenPauseMenuAndPause();
        }

        public void OnMissionIconClick()
        {
            
        }

        #endregion
    }
}
