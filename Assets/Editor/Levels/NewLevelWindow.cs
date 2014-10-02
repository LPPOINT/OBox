using System;
using Assets.Scripts.Levels;
using Assets.Scripts.Levels.Model;
using Assets.Scripts.Meta.Model;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Levels
{
    public class NewLevelWindow : EditorWindow
    {
        [MenuItem("Levels/New Level")]
        public static void Init()
        {
            var w = CreateInstance<NewLevelWindow>();
            w.maxSize = new Vector2(350, 350);
            w.minSize = new Vector2(300, 300);

            var newIndex = LevelsDatabaseManager.GetNewLevelIndex();
            w.levelNumber = newIndex.LevelNumber;
            w.worldNumber = newIndex.WorldNumber;

            w.Show();

        }


        private LevelMissionModel currentMissionType = LevelMissionModel.EnterTarget;
        private LevelTopUI.ShowMode currentTopUIShowMode = LevelTopUI.ShowMode.ShowAll;

        private int levelNumber = 1;
        private WorldNumber worldNumber = WorldNumber.World1;

        private int ThreeStarsSteps = 10;
        private int TwoStarsSteps = 15;
        private int OneStarsSteps = 20;

        private string GetLevelName(bool withExtension = true)
        {
            var l =  "Level" + levelNumber + "-" + (int) worldNumber;
            if (withExtension)
            {
                l += ".unity";
            }
            return l;
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            GUILayout.Label("New level settings", EditorStyles.boldLabel);
            GUILayout.Space(10);

            currentMissionType = (LevelMissionModel)EditorGUILayout.EnumPopup("Mission type", (Enum)currentMissionType);
            currentTopUIShowMode = (LevelTopUI.ShowMode)EditorGUILayout.EnumPopup("UI Show mode", (Enum)currentTopUIShowMode);

            GUILayout.Space(20);

            levelNumber = EditorGUILayout.IntField("Level number", levelNumber);
            worldNumber = (WorldNumber)EditorGUILayout.EnumPopup("World number", worldNumber);

            GUILayout.Space(20);

            ThreeStarsSteps = EditorGUILayout.IntField("Steps for 3 stars", ThreeStarsSteps);
            TwoStarsSteps = EditorGUILayout.IntField("Steps for 2 stars", TwoStarsSteps);
            OneStarsSteps = EditorGUILayout.IntField("Steps for 1 stars", OneStarsSteps);


            GUILayout.Space(20);

            if(GUILayout.Button("Create level"))
            {
                EditorApplication.SaveScene();
                LevelBuilder.CreateNewLevel(levelNumber, worldNumber);
            }

        }
    }
}
