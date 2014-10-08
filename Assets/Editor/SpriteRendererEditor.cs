using Assets.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomEditor(typeof(SpriteRenderer))]
    public class SpriteRendererEditor : UnityEditor.Editor
    {




        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var renderer = (SpriteRenderer) target;

            var oldColor = ColorUtils.CalculateRendererColor(renderer);
            var newColor = (RendererColor)EditorGUILayout.EnumPopup("Color preset:", oldColor);

            if (newColor != oldColor)
            {
                ColorUtils.SetRendererColor(renderer, newColor);
                EditorUtility.SetDirty(renderer);
            }
            else
            {
                
            }

        }
    }
}
