using Assets.Scripts.Model;
using Assets.Scripts.Model.Numeration;

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
            for (var i = 0; i < transform.GetComponentsInChildren<GUILevelIcon>().Length; i++)
            {
                var icon = transform.GetComponentsInChildren<GUILevelIcon>()[i];
                icon.Page = this;
                icon.Model = GUILevelIconModel.CreateFromGameModel(i+1, World, GameModel.Instance);
                icon.VisualizeByModel();
            }
        }
    }
}
