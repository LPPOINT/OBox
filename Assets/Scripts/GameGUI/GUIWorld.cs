
using Assets.Scripts.Model;
using Assets.Scripts.Model.Numeration;
using Assets.Scripts.Model.Statuses;
using SmartLocalization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameGUI
{
    public class GUIWorld : MonoBehaviour
    {


        [SerializeField] public WorldNumber TargetWorld;

        public GUIWorldModel Model { get;  set; }

        public Text WorldNumber;
        public Text WorldName;
        public Image WorldIcon;
        public Image WorldLockedIcon;
        public Image StarsProgressIcon;
        public Text StarsProgress;

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

        }


        public void OnClick()
        {
            if (Model.Status == WorldStatus.Unlocked)
            {
                GUISupervisor.Instance.OpenPage(GUIPageType.LevelSelection,
                    page => (page as GUILevelSelectionPage).World = Model.Data.Number);
            }
            else if(Model.Status == WorldStatus.Locked)
            {
                GUIWorldUnlockPopup.Show(Model);
            }
        }

    }
}
