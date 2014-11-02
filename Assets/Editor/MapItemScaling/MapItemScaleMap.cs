using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Editor.MapItemScaling
{

    [Serializable]
    public class MapItemScaleMap
    {
        public string TargetItemType;
        public List<MapItemScaleMapNode> Nodes;
        public static MapItemScaleMap CreateDefaultFor<T>() where T : MapItem
        {
            return CreateDefaultFor(typeof (T).AssemblyQualifiedName);
        }
        public static MapItemScaleMap CreateDefaultFor(string mapItemType)
        {
            var map = new MapItemScaleMap();
            map.TargetItemType = mapItemType;
            map.Nodes = new List<MapItemScaleMapNode>();


            map.Nodes.Add(new MapItemScaleMapNode(MapResolution.Resolution22x11, 0.1f));

            return map;
        }


    }
}
