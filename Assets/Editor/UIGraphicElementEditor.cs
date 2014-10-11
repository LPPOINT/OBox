using Assets.Editor.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Editor
{
    [CustomEditor(typeof(Graphic))]
    public class UIGraphicElementEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //var uiElement = (Graphic)target;

            //var oldColor = ColorUtils.CalculateUIElementColor(uiElement);
            //var newColor = (RendererColor)EditorGUILayout.EnumPopup("Color preset:", oldColor);

            //if (newColor != oldColor)
            //{
            //    ColorUtils.SetUIElementColor(uiElement, newColor);
            //    EditorUtility.SetDirty(uiElement);
            //}
            //else
            //{

            //}
        }
    }
}
