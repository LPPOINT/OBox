
using Assets.Scripts.GameGUI.Controls;
using Assets.Scripts.GameGUI.Controls.SlidePanel;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Numeration;
using Assets.Scripts.Model.Statuses;
using Assets.Scripts.Model.Unlocks;
using SmartLocalization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameGUI
{
    public class GUIWorld : MonoBehaviour
    {


        [SerializeField] public WorldNumber TargetWorld;

        public GUIWorldSelectionPage Page { get; set; }

        public GUIWorldModel Model { get;  set; }

        public Text WorldNumber;
        public Text WorldName;

        public Image WorldIcon;
        public Image WorldLockedIcon;

        public Image StarsProgressIcon;
        public Text StarsProgress;
        public Text TapToUnlock;


        public void VisualizeByModel()
        {

            if (Model == null)
            {
                Debug.Log("VisualizeByModel(): Model == null");
                return;
            }

            WorldNumber.gameObject.SetActive(true);
            WorldName.gameObject.SetActive(true);

            const string localizedNumberKey = "Worlds.WorldNumber";
            var localizedNumber = LanguageManager.Instance.GetTextValue(localizedNumberKey);

            var localizedNameKey = "Worlds.Names.World" + (int) Model.Data.Number;
            var localizedName = LanguageManager.Instance.GetTextValue(localizedNameKey);

            WorldNumber.text = string.Format(localizedNumber,(int)Model.Data.Number);
            WorldName.text = localizedName;

            StarsProgress.text = Model.CurrentStars + "/" + GameModel.LevelsInWorld*3;

            if (Model.Status == WorldStatus.Locked)
            {
                WorldLockedIcon.gameObject.SetActive(true);
                WorldIcon.gameObject.SetActive(false);

                StarsProgressIcon.gameObject.SetActive(false);
                StarsProgress.gameObject.SetActive(false);

                TapToUnlock.gameObject.SetActive(true);
            }
            else
            {
                WorldLockedIcon.gameObject.SetActive(false);
                WorldIcon.gameObject.SetActive(true);

                StarsProgressIcon.gameObject.SetActive(true);
                StarsProgress.gameObject.SetActive(true);

                TapToUnlock.gameObject.SetActive(false);
            }

        }


        public void OnClick()
        {

            var node = GetComponent<SlidePanelNode>();
            if (!node.IsSelected)
            {
                return;
            }

            if (Model.Status == WorldStatus.Unlocked)
            {
                GUISupervisor.Instance.OpenPage(GUIPageType.LevelSelection,
                    page => (page as GUILevelSelectionPage).World = Model.Data.Number);
            }
            else if(Model.Status == WorldStatus.Locked)
            {
                Page.ShowWorldUnlockPopup(Model.Data, new WorldUnlockHandler());
            }
        }

    }
}
