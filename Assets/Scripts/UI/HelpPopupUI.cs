using System;
using Assets.Scripts.Levels;
using Assets.Scripts.Localization;
using Assets.Scripts.Missions;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class HelpPopupUI : PopupUI
    {
        public void Resume()
        {
            Level.Current.HideHelpPopup();
        }

        public Sprite Icon { get;  set; }

        public Image IconImage;
        public Text DescriptionText;

        private void Start()
        {


            var level = Level.Current;



            Icon = level.Mission.Icon;
            var missionType = level.Mission.GetType();
            var key = string.Empty;

            if (missionType == typeof (EnterTargetMission)) key = "MissionHelp.Target";
            if (missionType == typeof(DestroyAllWallsMission)) key = "MissionHelp.DestroyAllWalls";

            if (IconImage != null) IconImage.sprite = Icon;
            if (DescriptionText != null) DescriptionText.InjectLocalization(key);
        }

    }
}
