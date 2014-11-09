using Assets.Scripts.GameGUI.Translations;
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

        private void Start()
        {
        }

        public virtual GUITranslation GetCustomTranslationTo(GUIPage newPage, GUIPageType newPageType)
        {
            return null;
        }


        public virtual void OnShow()
        {
            var c = GetComponent<Canvas>();
            if (c != null)
                c.planeDistance = 1;
        }

        public virtual void OnClose()
        {
            
        }

        public static T Find<T>() where T : GUIPage
        {
            return FindObjectOfType<T>();
        }
    }
}
