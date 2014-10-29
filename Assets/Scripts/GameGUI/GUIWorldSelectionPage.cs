using System.Collections.Generic;

namespace Assets.Scripts.GameGUI
{
    public class GUIWorldSelectionPage : GUIPage
    {
        public override GUIPageType Type
        {
            get { return GUIPageType.WorldSelection; }
        }

        public List<GUIWorld> Worlds;

    }
}
