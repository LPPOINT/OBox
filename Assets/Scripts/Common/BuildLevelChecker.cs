using UnityEngine;

namespace Assets.Scripts.Common
{
    public class BuildLevelChecker : MonoBehaviour
    {
        private void OnGUI()
        {
#if UNITY_EDITOR
            if (Application.loadedLevelName.Contains("build"))
            {
                GUI.Label(new Rect(20,20, 400, 20), "IN BUILD LEVEL!!!");
            }
#endif
        }
    }
}
