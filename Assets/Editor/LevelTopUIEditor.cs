using System.Diagnostics;
using Assets.Scripts.Levels;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace Assets.Editor
{
    [CustomEditor(typeof(LevelTopUI))]
    public class LevelTopUIEditor : UnityEditor.Editor
    {

        private LevelTopUI.ShowMode lastShowMode;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var ltui = (LevelTopUI) target;

            if (lastShowMode != ltui.Mode)
            {
                lastShowMode = ltui.Mode;
                ltui.ApplyStartMode();
            }

        }
    }
}
