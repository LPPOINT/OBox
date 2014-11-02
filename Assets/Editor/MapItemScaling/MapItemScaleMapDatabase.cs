using System;
using System.Collections.Generic;
using Assets.Scripts.Map.Items;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.MapItemScaling
{
    public class MapItemScaleMapDatabase : ScriptableObject
    {
        public List<MapItemScaleMap> Nodes;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void Save()
        {
            EditorUtility.SetDirty(this);
        }

        public static MapItemScaleMapDatabase Fetch()
        {
            var old = AssetDatabase.LoadAssetAtPath("Assets/Editor/MapItemScaling/Database.asset",
                typeof (MapItemScaleMapDatabase)) as MapItemScaleMapDatabase;

            if (old != null)
            {
                return old;
            }

            Debug.Log("Creating new databse");

            var db = CreateInstance<MapItemScaleMapDatabase>();

            db.Nodes = new List<MapItemScaleMap>();

            db.Nodes.Add(MapItemScaleMap.CreateDefaultFor<Wall>());
            db.Nodes.Add(MapItemScaleMap.CreateDefaultFor<Target>());
            db.Nodes.Add(MapItemScaleMap.CreateDefaultFor<Teleporter>());
            db.Nodes.Add(MapItemScaleMap.CreateDefaultFor<Turn>());

            AssetDatabase.CreateAsset(db, "Assets/Editor/MapItemScaling/Database.asset");
          
            return db;

        }

    }
}
