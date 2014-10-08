﻿using System;
using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class MenuUI : MonoBehaviour
    {

        public RectTransform MenuBorder;

        public Level Level { get; set; }

        private bool isClosing;
        private Action afterClosingAction;

        public void Close(Action afterClosing)
        {
            afterClosingAction = afterClosing;
            isClosing = true;
            MenuBorder.gameObject.GetComponent<Animator>().Play("MenuDisposing");
        }

        private void Update()
        {
            if (isClosing)
            {
                afterClosingAction();
                Destroy(gameObject);
            }
        }


        public void OnContinueButton()
        {
            Level.ClosePauseMenuAndPlay();
        }

        public void OnResetButton()
        {
            Level.ResetLevel();
        }

        public void OnMainMenuButton()
        {
            
        }

    }
}
