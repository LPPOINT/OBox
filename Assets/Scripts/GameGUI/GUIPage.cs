using UnityEngine;

namespace Assets.Scripts.GameGUI
{
    public class GUIPage : MonoBehaviour
    {


        public static GUISupervisor Supervisor
        {
            get { return GUISupervisor.Instance; }
        }

        public virtual GUIPageType Type
        {
            get
            {
                return GUIPageType.MainMenu;
            }
        }
        public virtual GUIPageMode Mode { get
        {
            return GetComponent<Canvas>() != null ? GUIPageMode.Visual : GUIPageMode.Logical;
        } 
        }

        public virtual GUITranslation GetCustomTranslationTo(GUIPage newPage, GUIPageType newPageType)
        {
            return null;
        }


        public virtual void OnShow()
        {
            
        }

        public virtual void OnClose()
        {
            
        }

    }
}
