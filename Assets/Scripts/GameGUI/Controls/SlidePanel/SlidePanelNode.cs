using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameGUI.Controls.SlidePanel
{

    [ExecuteInEditMode]
    public class SlidePanelNode : MonoBehaviour
    {
        public Graphic Graphic;
        public RectTransform ActivePosition;

        public bool IsSelected { get; set; }

        private void Start()
        {
            if (Graphic == null) Graphic = GetComponent<Graphic>();
        }


        public void OnTranslationToBegin()
        {
            
        }

        public void OnTranslationToEnd()
        {
            
        }

        public void OnTranslationFromBegin()
        {
            
        }

        public void OnTranslationFromEnd()
        {
            
        }

    }
}
