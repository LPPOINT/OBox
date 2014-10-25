using Assets.Scripts.Model;
using UnityEngine;

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
            Supervisor.OpenLevel(1, WorldNumber.World1);
        }

        public void OnShopClick()
        {
            
        }

        public void OnRemoveAdsClick()
        {
            
        }

    }
}
