using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{

    public class ProgressBar : MonoBehaviour
    {


        public enum ProgressBarTranslation
        {
            None,
            FromCurrent,
            FromCenter,
            FromBorders
        }


        public Image Background;
        private float initialWidth;

        public float MaxValue = 10;
        public float CurrentValue;

        private float lastMaxValue;
        private float lastCurrentValue;

        public void SetLimit(float current, float max)
        {
            CurrentValue = current;
            MaxValue = max;
        }

        private void UpdateValues()
        {
            SetValue(CurrentValue);
        }

        private float GetWidthForValue(float value)
        {
            return ((initialWidth) - ((initialWidth)*(MaxValue/value))) / (MaxValue/value); // хуйня
        }
        private void SetWidth(float value)
        {
            Background.rectTransform.sizeDelta = new Vector2(value, Background.rectTransform.sizeDelta.y);
        }

        private void OnITweenProgressBarDeltaUpdate(float value)
        {
            SetWidth(value);
        }



        public void SetValue(float newValue, ProgressBarTranslation translation = ProgressBarTranslation.FromCurrent)
        {
            if (MaxValue != 0)
            {
                if (newValue > MaxValue) SetValue(MaxValue);
                else if (newValue < 0) SetValue(0);

            }

            var targetWidth = GetWidthForValue(newValue);
            var currentWidth = Background.rectTransform.sizeDelta.x;

            const iTween.EaseType easeType = iTween.EaseType.easeInOutCirc;
            const float time = 0.2f;

            if (translation == ProgressBarTranslation.FromCurrent)
            {
                iTween.ValueTo(gameObject,
                    iTween.Hash("from", currentWidth, "to", targetWidth, "onupdate", "OnITweenProgressBarDeltaUpdate",
                        "time", time, "easetype", easeType));
            }
            else if (translation == ProgressBarTranslation.None)
            {
                SetWidth(targetWidth);
            }
            else if (translation == ProgressBarTranslation.FromCenter)
            {
                SetValue(0, ProgressBarTranslation.None);
                SetValue(newValue);
            }
            else if (translation == ProgressBarTranslation.FromBorders)
            {
                SetValue(MaxValue, ProgressBarTranslation.None);
                SetValue(newValue);
            }

        }


        private void Start ()
        {

            if (Background == null)
            {
                Background = GetComponent<Image>();
            }

            initialWidth = Background.rectTransform.rect.width;
            UpdateValues();
            lastMaxValue = MaxValue;
            lastCurrentValue = CurrentValue;
        }

        private void Update()
        {
            if (lastMaxValue != MaxValue || lastCurrentValue != CurrentValue)
            {
                UpdateValues();
                lastCurrentValue = CurrentValue;
                lastMaxValue = MaxValue;
            }

            if (Input.GetKey(KeyCode.RightArrow)) CurrentValue += 1;
            if (Input.GetKey(KeyCode.LeftArrow)) CurrentValue -= 1;

        }
    }
}
