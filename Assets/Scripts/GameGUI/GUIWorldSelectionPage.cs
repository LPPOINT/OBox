using System.Collections.Generic;
using System.Linq;
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
                world.VisualizeByModel();
            }

        }

    }
}
