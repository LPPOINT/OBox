using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Camera.Effects;
using Assets.Scripts.GameGUI.Controls.SlidePanel;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Numeration;
using Assets.Scripts.Model.Statuses;
using Assets.Scripts.Model.Unlocks;
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
        public SlidePanel WorldsSlider;
        public GUIUnlockAllLevelsButton UnlockAllLevelsButton;

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

            WorldsSlider.NodeChanged += (sender, args) =>
                                        {
                                            var w = args.CurrentNode.GetComponent<GUIWorld>();
                                            if (w != null)
                                            {
                                                if (UnlockAllLevelsButton != null)
                                                {
                                                    if (w.Model.Status == WorldStatus.Locked &&
                                                        !UnlockAllLevelsButton.IsVisible)
                                                    {
                                                        UnlockAllLevelsButton.Show();
                                                    }
                                                    else if (w.Model.Status == WorldStatus.Unlocked &&
                                                             UnlockAllLevelsButton.IsVisible)
                                                    {
                                                        UnlockAllLevelsButton.Hide();
                                                    }
                                                }
                                            }
                                        };

        }

        public void ShowWorldUnlockPopup(GUIWorldData worldData, IWorldUnlockHandler unlockHandler)
        {
            StartCoroutine(WaitAndShowPopup(0.3f, worldData, unlockHandler));
        }

        private IEnumerator WaitAndShowPopup(float waitTime, GUIWorldData worldData, IWorldUnlockHandler unlockHandler)
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

        public void UpdateWorldNode(WorldNumber number)
        {
            var w = Worlds.FirstOrDefault(world => world.Model.Data.Number == number);
            if (w != null)
            {
                w.Model = GUIWorldModel.FromGameModel(w.Model.Data, GameModel.Instance);
                w.VisualizeByModel();
            }

        }

        public void UpdateAllWorldsNodes()
        {
            foreach (var w in GameModel.EnumerateWorlds())
            {
                UpdateWorldNode(w);
            }
        }
    }
}
