using System.Diagnostics;
using Assets.Scripts.Levels;
using Assets.Scripts.UI;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace Assets.Editor
{
    [CustomEditor(typeof(OverlayUI))]
    public class LevelTopUIEditor : UnityEditor.Editor
    {

        private OverlayUI.ShowMode lastShowMode;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var ltui = (OverlayUI) target;

            if (lastShowMode != ltui.Mode)
            {
                lastShowMode = ltui.Mode;
                ltui.ApplyStartMode();
            }

        }
    }
}
