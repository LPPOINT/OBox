using System;
using System.Collections.Generic;
using Assets.Scripts.Model.Numeration;
using Assets.Scripts.Model.Statuses;

namespace Assets.Scripts.GameGUI
{
    [Serializable]
    public class GUIWorldModel
    {
        public WorldStatus Status;
        public string Name;
        public WorldNumber Number;
        public int CurrentStars;
        public int TotalStars;
        public List<GUIWorldFeature> Features;
        

    }
}
