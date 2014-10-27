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
        }

        public override void OnTool(ToolEvent e, IToolContext context)
        {
            Debug.Log("OnTool");
        }


        private TileSystem targetTileSystem;
        private GameMap targetMap;



        public override void OnToolOptionsGUI()
        {
            if (targetTileSystem == null || targetMap == null)
            {
                targetMap = Object.FindObjectOfType<GameMap>();
                targetTileSystem = targetMap.GetComponent<TileSystem>();
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
