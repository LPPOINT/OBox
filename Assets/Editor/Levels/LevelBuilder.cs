using Assets.Scripts.Levels;
using Assets.Scripts.Map;
using Assets.Scripts.Meta.Model;
using Assets.Scripts.Missions;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Levels
{
    public static class LevelBuilder
    {



        private static GameObject CreateLevelObject(string name)
        {
            var prefab =  AssetDatabase.LoadAssetAtPath(@"Assets/Prefabs/LevelTemplate/" + name + ".prefab", typeof(GameObject));
            return prefab as GameObject;
        }

        private static GameObject AddLevelObject(string name, string newName = "")
        {
            var o = CreateLevelObject(name);
            if (o != null)
            {
                var newObj = Object.Instantiate(o);

                newObj.name = !string.IsNullOrEmpty(newName) ? newName : name;
                EditorUtility.SetDirty(o);
                return newObj;
            }
            else
            {
                Debug.LogWarning("LevelBuilder(): level object not found");
            }
            return null;
        }

        public static void CreateNewLevel(int number, WorldNumber worldNumber)
        {
            var index = new LevelIndex(number, worldNumber);

            //EditorApplication.SaveScene();

            EditorApplication.NewScene();

            Object.DestroyImmediate(GameObject.Find("Main Camera"));
            Object.DestroyImmediate(GameObject.Find("Directional Light"));

            RenderSettings.skybox = null;

            AddLevelObject("Camera", "MainCamera");
            AddLevelObject("EventSystem");
            var levelGO = AddLevelObject("Level");
            var map = AddLevelObject("Map");
            var mission = AddLevelObject("Mission");
            var solution = AddLevelObject("Solution");
            var topUI = AddLevelObject("TopUI");

            var level = levelGO.GetComponent<Level>();

            level.Index = index;
            level.LevelMap = map.GetComponent<GameMap>();
            level.Mission = mission.GetComponent<LevelMission>();
            level.Solution = solution.GetComponent<LevelSolution>();
            level.TopUI = topUI.GetComponent<LevelTopUI>();

            var model = level.CreateLevelModel();

            LevelsDatabaseManager.AddOrUpdateLevelModel(model);

            EditorApplication.SaveScene(index.GetScenePath(true));

        }


    }
}
