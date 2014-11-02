using Assets.Scripts.Model.Unlocks;
using UnityEngine;

namespace Assets.Scripts.GameGUI
{
    public class GUIUnlockAllLevelsButton : MonoBehaviour
    {


        public const string In = "UnlockAllLevelsIn";
        public const string Out = "UnlockAllOut";
        public const string Idle = "UnlockAllLevelsIdle";

        public bool IsVisible { get; private set; }

        public void Show()
        {
            gameObject.GetComponent<Animator>().Play(In);
            IsVisible = true;
        }

        public void Hide()
        {
            gameObject.GetComponent<Animator>().Play(Out);
            IsVisible = false;
        }

        public void OnClick()
        {
            WorldUnlockHandler.UnlockAllWorlds();
            GUIPage.Find<GUIWorldSelectionPage>().UpdateAllWorldsNodes();
        }
    }
}
