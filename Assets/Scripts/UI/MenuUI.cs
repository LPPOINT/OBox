﻿using System;
using Assets.Scripts.Levels;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.UI
{

    [InitializeOnLoad]
    public class MenuUI : PopupUI
    {

        public RectTransform MenuBorder;

        public Level Level { get; set; }

        private void Start()
        {
            Level = Level.Current;
        }

        public void OnContinueButton()
        {
            Level.HidePauseMenu();
        }

        public void OnResetButton()
        {
            Level.ResetLevel();
        }

        public void OnMainMenuButton()
        {
            Level.Exit();
        }

    }
}
