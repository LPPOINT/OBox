using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Map;
using Rotorz.Tile;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{

    [CustomEditor(typeof(GameMap))]
    public class GameMapEditor : UnityEditor.Editor
    {

        public bool AutoscaleNewTiles;

        private List<MapItem> lastItems;

        private Vector2 cells;
        private Vector2 cellsDepency;

        public override bool RequiresConstantRepaint()
        {
            return true;
        }

        public override void OnInspectorGUI()
        {

            

            base.OnInspectorGUI();
            var map = (GameMap) target;


            AutoscaleNewTiles = EditorGUILayout.Toggle("Autosclale New Tiles", AutoscaleNewTiles);

            EditorGUILayout.Separator();

            



            var ts = map.GetComponent<TileSystem>();
            var cellScale = ts.CellSize;

            cells.x = ts.ColumnCount;
            cells.y = ts.RowCount;

            var newColumns = EditorGUILayout.IntField("Columns", (int)cells.x);
            var newRows = UnityEditor.EditorGUILayout.IntField("Rows", (int)cells.y);

            EditorGUILayout.Separator();




            cellsDepency = new Vector2(ts.ColumnCount / cellScale.x, ts.RowCount / cellScale.y);


            if (AutoscaleNewTiles)
            {

                if (lastItems == null)
                {
                    lastItems = new List<MapItem>();
                }

                var items = map.GetComponentsInChildren<MapItem>();

                for (int i = 0; i < items.Length; i++)
                {
                    var mapItem = items[i];
                    if (!lastItems.Contains(mapItem))
                    {
                        lastItems.Add(mapItem);
                        mapItem.SetSize(cellScale.x, cellScale.y);
                    }
                }

                for (int i = 0; i < lastItems.Count; i++)
                {
                    var lastItem = lastItems[i];
                    if (!items.Contains(lastItem))
                    {
                        lastItems.Remove(lastItem);
                    }
                }
            }


            if(newColumns != cells.x || newRows != cells.y)
            {
                cellScale.x = newColumns*cellsDepency.x;
                cellScale.y = newRows * cellsDepency.y;
                Debug.Log("CreateSystem");
                ts.CreateSystem(0.5f, 0.5f, 0, 22, 14);
            }

            if (GUILayout.Button("Update items scale"))
            {

                    var items = map.GetComponentsInChildren<MapItem>();

                    foreach (var item in items)
                    {
                        item.SetSize(cellScale.x, cellScale.y);
                    }

            }


        }
    }
}
