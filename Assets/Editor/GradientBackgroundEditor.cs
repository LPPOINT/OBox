using System.Linq;
using Assets.Scripts.Levels.GradientBackground;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomEditor(typeof(GradientBackground))]
    public class GradientBackgroundEditor : UnityEditor.Editor
    {

        private GradientPreset lastPreset;
        private string newPresetName;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var gbg = (GradientBackground) target;

            EditorGUILayout.Space();

            GUILayout.BeginHorizontal(); 
            if (string.IsNullOrEmpty(newPresetName))
            {
                var assetsCount =
                    AssetDatabase.GetAllAssetPaths().Count(s => s.StartsWith("Assets/Resources/GradientPresets/Preset"));
                newPresetName = "Preset" + (assetsCount+1);
            }

            newPresetName = EditorGUILayout.TextField(string.Empty, newPresetName);

            if (GUILayout.Button("Save Preset"))
            {

                var preset = gbg.CreatePreset();

                AssetDatabase.CreateAsset(preset, "Assets/Resources/GradientPresets/" + newPresetName + ".asset");
            }

            GUILayout.EndHorizontal();

            var sourcePreset = EditorGUILayout.ObjectField("Preset:", lastPreset, typeof (GradientPreset), true) as GradientPreset;

            if (sourcePreset != null && lastPreset != sourcePreset)
            {
                lastPreset = sourcePreset;
                gbg.Color1 = sourcePreset.Color1;
                gbg.Color2 = sourcePreset.Color2;

                EditorUtility.SetDirty(gbg);
            }


        }
    }
}
