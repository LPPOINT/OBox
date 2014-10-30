using System;
using System.Collections.Generic;
using Assets.Scripts.Model.Numeration;

namespace Assets.Scripts.GameGUI
{
    [Serializable]
    public class GUIWorldData
    {
        public WorldNumber Number;
        public int StarsForUnlock;
        public List<GUIWorldFeature> Features;
    }
}
