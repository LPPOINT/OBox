using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Levels
{
    public class LevelTopUI : MonoBehaviour
    {
        public Slider StepsSlider;
        public Image FirstStar;
        public Image SecondStar;
        public Image ThirdStar;
        public Text StepsCount;
        public Button TryAgainButton;
        public Button MenuButton;

        public CanvasGroup LeftGroup;
        public CanvasGroup RightGroup;

        public ShowMode Mode;

        private ShowMode currentMode;
        public ShowMode CurrentMode
        {
            get { return currentMode; }
             set
            {
                currentMode = value;
                UpdateMode();
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
                StepsSlider.gameObject.SetActive(false);
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
                StepsSlider.gameObject.SetActive(true);
                MenuButton.gameObject.SetActive(true);
            }
        }

        public void UpdateMode()
        {
            UpdateMode(CurrentMode);
        }

        public void ApplyStartMode()
        {
            UpdateMode(Mode);
        }


        private void Start()
        {
            RevertMode();
        }

        public void RevertMode()
        {

            CurrentMode = Mode;
        }


        public void OnResetClick()
        {
            Level.Current.Reset();
        }
        public void OnMenuClick()
        {
            Level.Current.OpenMenu();
        }

    }
}
