using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{

    public class ProgressBar : MonoBehaviour
    {


        public Image Background;
        private float initialWidth;

        public float MaxValue = 10;
        public float CurrentValue;

        private float lastMaxValue;
        private float lastCurrentValue;

        public void SetValues(float current, float max)
        {
            CurrentValue = current;
            MaxValue = max;
        }

        private void UpdateValues()
        {
            SetCurrentValue(CurrentValue, false);
        }

        public float GetWidthForValue(float value)
        {
            return ((initialWidth ) - ((initialWidth) * (MaxValue / value))) / (MaxValue/value); // хуйня
        }
        private void SetWidth(float value)
        {
            Background.rectTransform.sizeDelta = new Vector2(value, Background.rectTransform.sizeDelta.y);
        }

        public void SetCurrentValue(float newValue, bool withTranslation)
        {
            if (MaxValue != 0)
            {
                if (newValue > MaxValue) SetCurrentValue(MaxValue, withTranslation);
                else if (newValue < 0) SetCurrentValue(0, withTranslation);
            }

            var targetWidth = GetWidthForValue(newValue);
            var currentWidth = Background.rectTransform.sizeDelta.x;

            if (!withTranslation)
            {
                SetWidth(targetWidth);
            }
            else
            {
                StartCoroutine(DoTranslation(0, 0)); //todo
            }

        }


        private IEnumerator DoTranslation(int ticks, float offset)
        {
            for (var i = 0; i < ticks; i++)
            {
                var currentWidth = Background.rectTransform.sizeDelta.x;
                var newWidth = currentWidth + offset;
                SetWidth(newWidth);
                yield return new WaitForEndOfFrame();
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
