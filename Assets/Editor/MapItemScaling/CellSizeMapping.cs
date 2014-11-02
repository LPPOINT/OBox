using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Editor.MapItemScaling
{
    public static class CellSizeMapping
    {
        public static Dictionary<MapResolution, Vector2> Sizes = new Dictionary<MapResolution, Vector2>()
                                                                 {
                                                                     {MapResolution.Resolution14x19, new Vector2(0.5f, 0.5f)},
                                                                     {MapResolution.Resolution7x19, new Vector2(1, 1)}
                                                                 };

        public static Vector2 GetCellSizeForResolution(MapResolution r)
        {
            try
            {
                return Sizes[r];
            }
            catch 
            {
                return Vector2.one;
            }
        }
    }
}
