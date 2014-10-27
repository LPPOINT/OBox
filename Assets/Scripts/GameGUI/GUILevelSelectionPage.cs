using Assets.Scripts.Model;

namespace Assets.Scripts.GameGUI
{
    public class GUILevelSelectionPage : GUIPage
    {
        public override GUIPageType Type
        {
            get { return GUIPageType.LevelSelection; }
        }

        public override GUIPageMode Mode
        {
            get { return GUIPageMode.Visual; }
        }

        public WorldNumber World;

        private void Start()
        {
            foreach (var i in transform.GetComponentsInChildren<GUILevelIcon>())
            {
                i.Page = this;
            }
        }

    }
}
