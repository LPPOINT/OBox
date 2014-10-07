using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Levels;
using Assets.Scripts.Map;
using Rotorz.Tile;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Utils
{
    public class MapItemEditorUtils : ScriptableObject
    {

        private const string SliderPrefabPath = "Assets/Prefabs/InteractiveUI/Slider.prefab";

        [MenuItem("MapItem/Group Selected to Slider")]
        public static void GroupSelectedToSlider()
        {
            var selected = Selection.gameObjects;
            var items = new List<MapItem>();

            foreach (var go in selected)
            {
                var item = go.GetComponent<MapItem>();
                if(item != null) items.Add(item);
            }

            var probeParent = items.FirstOrDefault().transform.parent;

            foreach (var mapItem in items)
            {
                if (mapItem.transform.parent != probeParent)
                {
                    Debug.LogError("GroupSelectedToSlider(): selected map items should be on one level");
                    return;
                }
            }

            var sliderPrefab = AssetDatabase.LoadAssetAtPath(SliderPrefabPath, typeof (GameObject));
            var slider = Instantiate(sliderPrefab) as GameObject;
            slider.name = "Slider";

            var map = FindObjectOfType<GameMap>();

            foreach (var mapItem in items)
            {
                mapItem.transform.parent = slider.transform;
            }

            slider.transform.parent = probeParent;


        }

        [MenuItem("MapItem/Apply items reposition")]
        public static void ApplyItemsReposition()
        {
            var ts = FindObjectOfType<GameMap>().gameObject.GetComponent<TileSystem>();
            var items = FindObjectsOfType<MapItem>();

            foreach (var mapItem in items)
            {
                var index = ts.ClosestTileIndexFromWorld(mapItem.transform.position);
                
            }


        }
    }
}
