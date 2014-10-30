using System;
using System.Diagnostics;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Numeration;
using Assets.Scripts.Model.Statuses;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.GameGUI
{
    [Serializable]
    public class GUIWorldModel
    {
        public WorldStatus Status;
        public GUIWorldData Data;
        public int CurrentStars;

        public static GUIWorldModel FromGameModel(GUIWorldData data, GameModel model)
        {
            var wm = new GUIWorldModel();

            if (data == null)
            {
                Debug.Log("Data == null");
                return null;
            }

            wm.Data = data;
            wm.CurrentStars = 0;
            wm.Status = model.GetWorldStatus(data.Number);

            return wm;

        }

    }
}
