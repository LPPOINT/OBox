using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Map;
using Rotorz.Tile;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.MapItemScaling
{
    public class MapItemScaleMapWidow : EditorWindow
    {

        [MenuItem("MapItem/ScaleMap")]
        static void Init()
        {
            var w = CreateInstance<MapItemScaleMapWidow>();
            w.title = "Scale Map";
            w.Show();
        }

        private MapItemScaleMapDatabase database;
        private Dictionary<string, bool> foldouts = new Dictionary<string, bool>();

        private void Awake()
        {
            database = MapItemScaleMapDatabase.Fetch();
            if (database == null)
            {
                EditorUtility.DisplayDialog("Error", "MapItemScaleMapDatabase not found.", "OK");
                return;
            }

            ResetFoldout();

        }

        private void ResetFoldout()
        {
            foldouts.Clear();
            foreach (var n in database.Nodes)
            {
                foldouts.Add(n.TargetItemType, false);
            }
        }

        private void OnGUI()
        {
            for (var i = 0; i < database.Nodes.Count; i++)
            {
                var dbItem = database.Nodes[i];
                bool foldoutStatus = false;

                try
                {
                    foldoutStatus = foldouts[dbItem.TargetItemType];
                }
                catch (Exception e)
                {
                    ResetFoldout();
                    foldoutStatus = false;
                }

                if (EditorGUILayout.Foldout(foldoutStatus, Type.GetType(dbItem.TargetItemType).Name))
                {
                    foldouts[dbItem.TargetItemType] = true;

                    foreach (var node in dbItem.Nodes)
                    {
                        EditorGUILayout.BeginHorizontal();

                        var presetIndex =
                            EditorGUILayout.Popup(MapResolution.GetPresetStringIndexFromResolution(node.Resolution),
                                MapResolution.GetPresetsStrings().ToArray());

                        var oldR = node.Resolution;
                        var oldS = node.Scale;

                        node.Resolution = MapResolution.CreateResolutionFromPresetString(presetIndex);

                        node.Scale = EditorGUILayout.FloatField(node.Scale);



                        var sceneObj = EditorGUILayout.ObjectField(null, typeof (GameObject), true) as GameObject;

                        if (sceneObj != null)
                        {
                            node.Scale = sceneObj.gameObject.transform.localScale.x;

                        }

                        if (oldS != node.Scale || oldR != node.Resolution)
                        {
                            database.Save();
                        }

                        if (GUILayout.Button("Remove"))
                        {
                            database.Nodes.Remove(dbItem);
                            database.Save();
                        }

                        EditorGUILayout.EndHorizontal();
                    }
                }
                else
                {
                    foldouts[dbItem.TargetItemType] = false;
                }

                

            }

            if (GUILayout.Button("Pick values from map"))
            {
                var map = FindObjectOfType<GameMap>();
                


                foreach (Transform child in map.transform.FindChild("chunk_0_0").transform)
                {
                    if (child.GetComponent<MapItem>() != null || child.GetComponentInChildren<MapItem>() != null)
                    {

                        var mapItem = child.GetComponent<MapItem>() ?? child.GetComponentInChildren<MapItem>();

                        var fullType = mapItem.GetType().AssemblyQualifiedName;
                        if (database.Nodes.FirstOrDefault(scaleMap => scaleMap.TargetItemType == fullType) == null)
                        {
                            var scaleMap = new MapItemScaleMap();
                            scaleMap.TargetItemType = fullType;
                            scaleMap.Nodes = new List<MapItemScaleMapNode>();
                            scaleMap.Nodes.Add(
                                new MapItemScaleMapNode(MapResolution.FromTileSystem(map.GetComponent<TileSystem>()),
                                    child.gameObject.transform.localScale.x));
                            database.Nodes.Add(scaleMap);
                        }
                        else
                        {
                            var currentResolution = MapResolution.FromTileSystem(map.GetComponent<TileSystem>());
                            var scaleMap = database.Nodes.FirstOrDefault(sm => sm.TargetItemType == fullType);
                            var forCurrentResolution =
                                scaleMap.Nodes.FirstOrDefault(
                                    node =>
                                        node.Resolution.Columns == currentResolution.Columns &&
                                        node.Resolution.Rows == currentResolution.Rows);
                            if (forCurrentResolution != null)
                            {
                                forCurrentResolution.Scale = child.transform.localScale.x;
                            }
                            else
                            {
                                scaleMap.Nodes.Add(new MapItemScaleMapNode(currentResolution,
                                    child.transform.localScale.x));
                            }
                        }
                    }
                }


                ResetFoldout();
            }

            if (GUILayout.Button("Save"))
            {
                database.Save();
            }

        }
    }
}
