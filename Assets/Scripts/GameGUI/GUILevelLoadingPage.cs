using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameGUI
{
    public class GUILevelLoadingPage : GUIPage
    {
        public string LevelPath;

        public override GUIPageType Type
        {
            get { return GUIPageType.LevelLoading; }
        }

        public override GUIPageMode Mode
        {
            get { return GUIPageMode.Logical; }
        }

        public AsyncOperation LevelLoadingOperation { get; private set; }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.3f);

            Debug.Log("Has pro: " + Application.HasProLicense());
            Application.LoadLevel(LevelPath);

            //

        }

        public override void OnShow()
        {

        }
    }
}
