using Assets.Scripts.Debugging;

namespace Assets.Scripts.GameGUI
{
    public class GUIDebugPage : GUIPage
    {
        public override GUIPageType Type
        {
            get { return GUIPageType.Debug;}
        }

        public void OnUnlockAllLevelsClick()
        {
            
        }

        public void OnApplyClick()
        {
            Supervisor.OpenPage(GUIPageType.MainMenu);
        }

        public void OnEnableDisableClick()
        {
            DebugInspector.Enabled = !DebugInspector.Enabled;
        }
    }
}
