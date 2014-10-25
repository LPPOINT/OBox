using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.GameGUI
{
    public class GUITestPage : GUIPage
    {

        private void Start()
        {
            
        }

        public override GUIPageType Type
        {
            get { return GUIPageType.Debug; }
        }

        public void OnMenuClick()
        {
            Supervisor.OpenPage(GUIPageType.MainMenu);
        }
    }
}
