using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.Scripts.Map;
using Assets.Scripts.Map.Items;

namespace Assets.Scripts.Levels
{
    public class LevelEvent
    {
        public LevelEvent( string name, Dictionary<string, object> data)
        {

            Name = name;
            Data = data;
        }

        public LevelEvent()
        {
            
        }

        public LevelElement Element { get; internal set; }


        public bool IsMapItem
        {
            get { return Element is MapItem; }
        }

        public bool IsPlayer
        {
            get { return Element is Player; }
        }

        public string Name { get; private set; }

        public Dictionary<string, object> Data { get; private set; } 

    }
}
