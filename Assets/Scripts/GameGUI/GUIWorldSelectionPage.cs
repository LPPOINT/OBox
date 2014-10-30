﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Camera.Effects;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Numeration;
using UnityEngine;

namespace Assets.Scripts.GameGUI
{
    public class GUIWorldSelectionPage : GUIPage
    {
        public override GUIPageType Type
        {
            get { return GUIPageType.WorldSelection; }
        }

        public GUIWorldUnlockPopup WorldUnlockPopupPrefab;

        public List<GUIWorld> Worlds;

        private void Start()
        {
            Worlds = new List<GUIWorld>(FindObjectsOfType<GUIWorld>());

            foreach (var world in Worlds)
            {
                var number = world.TargetWorld;
                var data = GUIWorldDataManager.GetWorldDataByNumber(number);
                var worldModel = GUIWorldModel.FromGameModel(data, GameModel.Instance);
                world.Model = worldModel;
                world.Page = this;
                world.VisualizeByModel();
            }

        }

        public void ShowWorldUnlockPopup(GUIWorldData worldData, IGUIWorldUnlockHandler unlockHandler)
        {
            StartCoroutine(WaitAndShowPopup(0.3f, worldData, unlockHandler));
        }

        private IEnumerator WaitAndShowPopup(float waitTime, GUIWorldData worldData, IGUIWorldUnlockHandler unlockHandler)
        {
            CameraBlurEffect.BlurIn();
            yield return new WaitForSeconds(waitTime);
            var popup = (Instantiate(WorldUnlockPopupPrefab.gameObject) as GameObject).GetComponent<GUIWorldUnlockPopup>();
            popup.Data = worldData;
            popup.UnlockHandler = unlockHandler;

        }

        public void CloseWorldUnlockPopup()
        {
            CameraBlurEffect.BlurOut();
            GUIWorldUnlockPopup.Current.Close();
        }

    }
}
