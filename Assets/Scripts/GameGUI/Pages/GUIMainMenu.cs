namespace Assets.Scripts.GameGUI.Pages
{
    public class GUIMainMenu : GUIPage
    {
        public override GUIPageType Type
        {
            get { return GUIPageType.MainMenu; }
        }

        public void OnPlayClick()
        {
            Supervisor.OpenPage(GUIPageType.LevelSelection);
        }

        public void OnShopClick()
        {
            
        }

        public void OnRemoveAdsClick()
        {
            
        }

    }
}
