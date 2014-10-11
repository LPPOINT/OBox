using Assets.Scripts.Levels.Style.GradientBackground;
using UnityEditor;

namespace Assets.Editor.Levels.Style
{
    [CustomEditor(typeof(GradientBackground))]
    public class GradientBackgroundEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var gb = (GradientBackground) target;

            if (gb.LevelStyle == null)
            {
                EditorGUILayout.HelpBox("Level style missing", MessageType.Error);
            }

        }
    }
}
