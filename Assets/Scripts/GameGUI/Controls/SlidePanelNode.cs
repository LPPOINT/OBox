﻿using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameGUI.Controls
{

    [ExecuteInEditMode]
    public class SlidePanelNode : MonoBehaviour
    {
        public Graphic Graphic;
        public string Name;
        public RectTransform ActivePosition;

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
