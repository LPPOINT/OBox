using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Assets.Scripts.Levels;
using Assets.Scripts.Levels.Model;
using Assets.Scripts.Meta.Model;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using LevelModel = Assets.Scripts.Levels.Model.LevelModel;

namespace Assets.Editor.Levels
{
    public  class LevelsDatabaseManager : ScriptableObject
    {


        public static void UpdateModel(LevelModel originalModel, LevelModel newModel)
        {
            originalModel.MissionModel = newModel.MissionModel;
            originalModel.LevelNumber = newModel.LevelNumber;
            originalModel.WorldNumber = newModel.WorldNumber;
        }

        public static void AddOrUpdateLevelModel(LevelModel levelModel)
        {
            var db = GetDatabase();
            if (db == null)
            {
                return;
            }

            if (GetLevelModel(levelModel.LevelNumber, levelModel.WorldNumber) != null)
            {
                var model = db.Levels.FirstOrDefault(v => v.LevelNumber == levelModel.LevelNumber && v.WorldNumber == levelModel.WorldNumber);
                UpdateModel(model, levelModel);
            }

            else db.Levels.Add(levelModel);
            SaveDatabase();
            EditorUtility.SetDirty(db);
        }

        public static LevelModel GetLevelModel(int levelNumber, WorldNumber worldNumber)
        {
            var db = GetDatabase();
            if (db == null)
            {
                return null;
            }
            return
                db.Levels.FirstOrDefault(model => model.LevelNumber == levelNumber && model.WorldNumber == worldNumber);
        }

        public static string DatabasePath
        {
            get { return @"Assets/Resources/LevelsDatabase.asset"; }
        }

        public static void SaveDatabase()
        {
            AssetDatabase.SaveAssets();
        }

        public static LevelsDatabase GetDatabase()
        {

            if(!HasDatabase())
                CreateDatabase();

            var db = AssetDatabase.LoadAssetAtPath(DatabasePath, typeof(LevelsDatabase));
            if (db == null)
            {
                Debug.LogWarning("LevelsDatabaseManager(): GetDatabase(): db == null");
            }
            if ((db as LevelsDatabase).Levels == null)
            {
                Debug.Log("New database list");
                (db as LevelsDatabase).Levels = new List<LevelModel>();
            }
            return db as LevelsDatabase;
        }

        public static bool HasDatabase()
        {
   
            return AssetDatabase.LoadAssetAtPath(DatabasePath, typeof(LevelsDatabase)) != null;
        }

        public static void CreateDatabase()
        {
            if (!HasDatabase())
            {
                Debug.Log("Creating new database");
                var newDb = ScriptableObject.CreateInstance<LevelsDatabase>();
                AssetDatabase.CreateAsset(newDb, DatabasePath);
            }
            else
            {
                Debug.LogWarning("CreateDatabase(): Database already created");
            }
        }

        public static void UpdateCurrentLevelModel()
        {
            LevelPreviewManager.UpdateCurrentLevelPreview();
            var model = FindObjectOfType<Level>().CreateLevelModel();
            AddOrUpdateLevelModel(model);
        }

        public static LevelIndex GetNewLevelIndex()
        {
            var db = GetDatabase();

            var world = WorldNumber.World1;
            if(db.Levels.Any(model => model.WorldNumber == WorldNumber.World2)) world = WorldNumber.World2;
            if (db.Levels.Any(model => model.WorldNumber == WorldNumber.World3)) world = WorldNumber.World3;
            if (db.Levels.Any(model => model.WorldNumber == WorldNumber.World4)) world = WorldNumber.World4;

            var levels = db.Levels.Where(model => model.WorldNumber == world);

            var max = levels.Max(model => model.LevelNumber);

            return new LevelIndex(max + 1, world);

        }

    }
}
