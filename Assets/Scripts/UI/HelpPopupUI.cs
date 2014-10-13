using System;
using Assets.Scripts.Levels;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class HelpPopupUI : MonoBehaviour
    {
        public void Resume()
        {
            Level.Current.HideHelpPopup();
        }

        public Sprite Icon { get;  set; }
        public string Description { get; set; }

        public Image IconImage;
        public Text DescriptionText;

        private void Start()
        {
            if (IconImage != null) IconImage.sprite = Icon;
            if (DescriptionText != null) DescriptionText.text = Description;
        }

    }
}
