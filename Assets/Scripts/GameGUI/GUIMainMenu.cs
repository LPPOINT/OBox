﻿using UnityEngine;

namespace Assets.Scripts.GameGUI
{
    public class GUIMainMenu : GUIPage
    {
        public override GUIPageType Type
        {
            get { return GUIPageType.MainMenu; }
        }

        public void OnPlayClick()
        {
            Supervisor.OpenPage(GUIPageType.Test);
        }

        public void OnShopClick()
        {
            
        }

        public void OnRemoveAdsClick()
        {
            
        }

    }
}
