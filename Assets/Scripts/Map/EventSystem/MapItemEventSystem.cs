using System.Collections.Generic;

namespace Assets.Scripts.Map.EventSystem
{
    public class MapItemEventSystem
    {
        public MapItem MapItem { get; set; }

        public List<Event> Events { get; set; }

        public void EmitEvent(string e, EventParam args)
        {
            
        }

    }
}
