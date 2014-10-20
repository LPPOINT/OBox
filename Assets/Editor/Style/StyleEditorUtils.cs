using Assets.Scripts.Levels.Style.ElementsColorization;
using Assets.Scripts.Styles;
using Assets.Scripts.Styles.Gradient;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;

namespace Assets.Editor.Style
{
    public static class StyleEditorUtils
    {

        public static void ShowApplyStyleButtonFor(StyleProvider provider)
        {
            if (GUILayout.Button("Apply Style"))
            {
                ApplyStyleFromProvider(provider);
            }
        }

        public static void ApplyStyleFromProvider(StyleProvider newProvider)
        {
            GradientBackground.MainGradient.ColorProvider = newProvider;
            GradientBackground.MainGradient.Visualize();

            SetFrontColor(newProvider.GetStyle().GetFrontColor());

        }

        public static void SetFrontColor(Color color)
        {
            var allColored = Object.FindObjectsOfType<ColoredObject>();

            foreach (var o in allColored)
            {
                o.Color = color;

                foreach (var dirty in o.GetDirtyObjects())
                {
                    EditorUtility.SetDirty(dirty);
                }

            }

        }

    }
}
