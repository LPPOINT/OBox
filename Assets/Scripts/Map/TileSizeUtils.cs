using UnityEngine;

namespace Assets.Scripts.Map
{
    public static class TileSizeUtils
    {
        public static void SetScaleBySize(GameObject tileObject, Bounds tileBounds, float sizeWidth,
            float sizeHeight)
        {
            var targetXSize = sizeWidth;
            var targetYSize = sizeHeight;


            var currentXSize = tileBounds.size.x;
            var currentYSize = tileBounds.size.y;


            var scale = tileObject.transform.localScale;

            scale.x = targetXSize * scale.x / currentXSize;
            scale.y = targetYSize * scale.y / currentYSize;


            tileObject.transform.localScale = scale;
        }

        public static void SetMapItemScaleBySize(this MapItem mapItem, float sizeWidth, float sizeHeight)
        {
            if (mapItem.transform.parent == null)
            {
                SetScaleBySize(mapItem.gameObject, mapItem.GetComponent<Renderer>().bounds,
                    sizeWidth, sizeHeight);
            }
            else
            {
                var scaleTarget = mapItem.transform.parent.gameObject;
                SetScaleBySize(scaleTarget, mapItem.GetComponent<Renderer>().bounds,
                    sizeWidth, sizeHeight);
            }
        }

        public static void AlignMapItemToCellSize(this MapItem mapItem)
        {
            var size = mapItem.TileSystem.CellSize;
            SetMapItemScaleBySize(mapItem, size.x, size.y);
        }

    }
}
