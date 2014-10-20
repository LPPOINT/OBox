using Assets.Scripts.Styles.Gradient;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Style
{
    [CustomEditor(typeof(GradientBackground))]
    public class GradientBackgroundEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var gbg = (GradientBackground) target;

            if (GUILayout.Button("Visualize"))
            {
                gbg.Visualize();
            }

        }
    }
}
