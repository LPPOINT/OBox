using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Styles;
using UnityEditor;
using UnityEngine;


namespace Assets.Editor.Style
{
    [CustomEditor(typeof(CustomStyleProvider))]
    public class CustomStyleProviderEditor : UnityEditor.Editor
    {

        private string presetName;

        private void EditColor(CustomStyleProvider p, string colorName)
        {
            if (p.Colors == null)
            {
                p.Colors = new List<StyleColorEntry>();
            }
            if (p.Colors.All(entry => entry.Name != colorName))
            {
                p.Colors.Add(new StyleColorEntry(Color.white, colorName));
            }

            var color = p.Colors.FirstOrDefault(entry => entry.Name == colorName);

            color.Color = EditorGUILayout.ColorField(colorName, color.Color);

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var p = (CustomStyleProvider) target;

            

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditColor(p, StyleNames.BackgroundGradientColor1);
            EditColor(p, StyleNames.BackgroundGradientColor2);
            EditColor(p, StyleNames.LevelFrontColor);

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            presetName = EditorGUILayout.TextField("Preset name", presetName);
            if (GUILayout.Button("Create preset"))
            {

                if (string.IsNullOrEmpty(presetName))
                {
                    EditorUtility.DisplayDialog("Preset name empty", "Please enter name of preset", "OK");
                    return;
                }

                var preset = StylePreset.Create(p.GetStyle());
                preset.Save(presetName);
            }

            if (GUI.changed)
            {
                UnityEditor.EditorUtility.SetDirty(p);
            }

        }
    }
}
