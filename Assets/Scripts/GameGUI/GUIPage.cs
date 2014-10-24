using UnityEngine;

namespace Assets.Scripts.GameGUI
{
    public class GUIPage : MonoBehaviour
    {


        public static GUISupervisor Supervisor
        {
            get { return GUISupervisor.Instance; }
        }

        public virtual GUIPageType Type { get {return GUIPageType.MainMenu;} }

        public virtual GUITranslation GetCustomTranslationTo(GUIPage newPage, GUIPageType newPageType)
        {
            return null;
        }

    }
}
