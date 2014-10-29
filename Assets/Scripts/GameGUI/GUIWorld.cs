
using System.Net.Mime;
using Assets.Scripts.Model.Statuses;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameGUI
{
    public class GUIWorld : MonoBehaviour
    {
        public GUIWorldModel Model;

        public Text WorldNumber;
        public Text WorldName;
        public Image WorldIcon;
        public Image WorldLockedIcon;
        public Image StarsProgressIcon;
        public Text StarsProgress;

        public void VisualizeByModel()
        {
            
        }


        public void OnClick()
        {
            if (Model.Status == WorldStatus.Unlocked)
            {
                GUISupervisor.Instance.OpenPage(GUIPageType.LevelSelection,
                    page => (page as GUILevelSelectionPage).World = Model.Number);
            }
            else if(Model.Status == WorldStatus.Locked)
            {
                GUIWorldUnlockPopup.Show(Model);
            }
        }
    }
}
