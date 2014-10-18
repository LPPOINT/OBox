
using Assets.Scripts.Levels;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Levels
{
    [CustomEditor(typeof(Level))]
    public class LevelEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Update Model"))
            {
                LevelsDatabaseManager.UpdateCurrentLevelModel();
            }


        }
    }
}
