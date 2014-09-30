﻿using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    public class SceneHelper : ScriptableObject
    {
        public static void OpenLevel(int levelNumber)
        {
            EditorApplication.OpenScene("Assets/Scenes/Levels/Level" + levelNumber + ".unity");
        }

        [MenuItem("Levels/Level 1")]
        public static void Level1()
        {
            OpenLevel(1);
        }

        [MenuItem("Levels/Level 2")]
        public static void Level2()
        {
            OpenLevel(2);
        }

        [MenuItem("Levels/Level 3")]
        public static void Level3()
        {
            OpenLevel(3);
        }

        [MenuItem("Levels/Level 4")]
        public static void Level4()
        {
            OpenLevel(4);
        }

    }
}
