using System.Linq;
using Assets.Editor.MapItemScaling;
using Assets.Scripts.Map;
using Rotorz.Tile;
using Rotorz.Tile.Editor;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [InitializeOnLoad]
    public class RTSMapSizeTool : ToolBase
    {
        static RTSMapSizeTool()
        {
            ToolManager.Instance.RegisterTool<RTSMapSizeTool>();
            icon = AssetDatabase.LoadAssetAtPath("Assets/Editor/Icons/RTSResizeIcon.png", typeof(Texture2D)) as Texture2D;
            database = MapItemScaleMapDatabase.Fetch();
        }

        public override void OnTool(ToolEvent e, IToolContext context)
        {
        }


        private int tmpX = -1;
        private int tmpY = -1;

        private TileSystem targetTileSystem;
        private GameMap targetMap;

        private static MapItemScaleMapDatabase database;

        public override void OnToolOptionsGUI()
        {
            if (targetTileSystem == null || targetMap == null)
            {
                targetMap = Object.FindObjectOfType<GameMap>();
                targetTileSystem = targetMap.GetComponent<TileSystem>();
            }

            if (GUILayout.Button("Apply grid scale to items"))
            {
                ApplyGridScale();
            }

            if (tmpX == -1 || tmpY == -1)
            {
                tmpX = targetTileSystem.ColumnCount;
                tmpY = targetTileSystem.RowCount;
            }

            var newX = EditorGUILayout.IntField("Columns", tmpX);
            var newY = EditorGUILayout.IntField("Rows", tmpY);

            tmpX = newX;
            tmpY = newY;

            if ( GUILayout.Button("Resize"))
            {
                var cellWidth = CellSizeMapping.GetCellSizeForResolution(new MapResolution(newX, newY));
                targetTileSystem.CreateSystem(cellWidth.x, cellWidth.y, 1, tmpX, tmpY);
                targetTileSystem.RefreshAllTiles();
                
            } 


        }

        private void ApplyGridScale()
        {
            targetMap = Object.FindObjectOfType<GameMap>();
            targetTileSystem = targetMap.GetComponent<TileSystem>();


            var r = new MapResolution(targetTileSystem.ColumnCount, targetTileSystem.RowCount);

            foreach (Transform child in targetMap.transform.FindChild("chunk_0_0").transform)
            {
                var item = child.GetComponent<MapItem>() ?? child.GetComponentInChildren<MapItem>();

                if (item == null)
                {
                    Debug.Log("Unexpected child of map detected");
                    return;
                }


                var itemType = item.GetType().AssemblyQualifiedName;
                var itemScaleMap = database.Nodes.FirstOrDefault(map => map.TargetItemType == itemType);
                if (itemScaleMap != null)
                {
                    var itemScaleForResolution =
                        itemScaleMap.Nodes.FirstOrDefault(
                            node => node.Resolution.Columns == r.Columns && node.Resolution.Rows == r.Rows);

                    if (itemScaleForResolution != null)
                    {
                        child.transform.localScale = new Vector3(itemScaleForResolution.Scale, itemScaleForResolution.Scale,
                            item.transform.localScale.z);
                    }
                }
            }
        }

        #region ToolBase stuff

        private static readonly Texture2D icon;

        public override string Label
        {
            get { return "Map Resizer"; }
        }

        public override Texture2D IconActive
        {
            get { return icon; }
        }

        public override Texture2D IconNormal
        {
            get { return icon; }
        }

        #endregion
    }
}
