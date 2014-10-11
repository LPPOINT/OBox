using Assets.Editor.Utils.LevelColorUtils;
using Assets.Scripts.Levels;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Levels
{
    [CustomEditor(typeof(Level))]
    public class LevelEditor : UnityEditor.Editor
    {


        private Color levelColor;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Update Model"))
            {
                LevelsDatabaseManager.UpdateCurrentLevelModel();
            }

            var newColor = EditorGUILayout.ColorField("Elements Color", levelColor);

            if (newColor != levelColor)
            {
               
                levelColor = newColor;
                LevelColor.SetColor(levelColor);
            }

        }
    }
}
