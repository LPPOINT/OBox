using Assets.Scripts.Levels.Style;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Levels.Style
{
    [CustomEditor(typeof(LevelCustomStyle))]
    public class CustomLevelStyleEditor : UnityEditor.Editor
    {

        [SerializeField] private string newStylePresetName = string.Empty;
        [SerializeField] private Color lastFrontColor;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var style = (LevelCustomStyle) target;

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            newStylePresetName = EditorGUILayout.TextField(string.Empty, newStylePresetName);
            if (GUILayout.Button("Save preset"))
            {
                var p = style.CreatePreset();
                AssetDatabase.CreateAsset(p, "Assets/Resources/LevelStyles/" + newStylePresetName + ".asset");
            }

            EditorGUILayout.EndHorizontal();

            if (lastFrontColor != style.FrontColor)
            {
                lastFrontColor = style.FrontColor;
                LevelStyleUtils.SetColor(lastFrontColor);
            }

        }
    }
}
