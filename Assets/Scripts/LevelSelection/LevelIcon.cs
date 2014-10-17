using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.LevelSelection
{

    [ExecuteInEditMode]
    public class LevelIcon : MonoBehaviour
    {
        public enum LevelIconState
        {
            Disabled,
            Current,
            OneStar,
            TwoStar,
            ThreeStar,
            NoneStars
        }

        public LevelIconState State;
        public int LevelNumber;

        public Text LevelNumberText;
        public Image OneStarImage;
        public Image TwoStarImage;
        public Image ThreeStarImage;

        public void OnClick()
        {

        }

        public void Invalidate()
        {
            
        }

        private bool IsValidState
        {
            get
            {
                return false;
            }
        }

        public bool IsValid
        {
            get { return LevelNumberText != null && LevelNumberText.text == LevelNumber.ToString(CultureInfo.InvariantCulture) && IsValidState; }
        }

        private void Start()
        {
            Invalidate();
        }

        private void Update()
        {
            if (!IsValid)
            {
                Invalidate();
            }
        }
    }
}
