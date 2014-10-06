using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts.Map.InteractiveUI
{
    public class MapItemsGroup : LevelElement
    {

        private List<MapItem> items;
        private Vector3 startPos;


        private void Start()
        {
            items = transform.GetComponentsInChildren<MapItem>().ToList();
            startPos = transform.position;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NormalizePositions();
            }
        }




        private void OnITweenNormalizationDone()
        {
            foreach (var mapItem in items)
            {
                mapItem.RefreshIndex();
            }
        }

        public void NormalizePositions()
        {
            var probeItem = items.FirstOrDefault();
            var closestIndex = Level.LevelMap.TileSystem.ClosestTileIndexFromWorld(probeItem.transform.position);
            var newPosition = Level.LevelMap.TileSystem.WorldPositionFromTileIndex(closestIndex);
            var positionsOffset = new Vector2(Mathf.Abs(probeItem.transform.position.x - newPosition.x),
                Mathf.Abs(probeItem.transform.position.y - newPosition.y));

            iTween.MoveTo(gameObject, iTween.Hash(
                    "position", new Vector3(transform.position.x + positionsOffset.x, transform.position.y + positionsOffset.y, transform.position.z),
                    "time", 0.3f,
                    "oncomplete", "OnITweenNormalizationDone",
                    "easetype", iTween.EaseType.easeInOutCirc
                ));


        }



        public IEnumerable<MapItem> GetEdgeItems(Direction edge)
        {


            var minX = items.Min(item => item.Index.column);
            var minY = items.Min(item => item.Index.row);

            var maxX = items.Max(item => item.Index.column);
            var maxY = items.Max(item => item.Index.row);

            switch (edge)
            {
                case Direction.Top:
                    return items.Where(item => item.Index.row == minY);
                case Direction.Bottom:
                    return items.Where(item => item.Index.row == maxY);
                case Direction.Left:
                    return items.Where(item => item.Index.column == minX);
                case Direction.Right:
                    return items.Where(item => item.Index.column == maxX);
            }
            return null;
        }

        public void Offset(Vector2 offset)
        {
            transform.position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z);

            foreach (var mapItem in items)
            {
                mapItem.RefreshIndex();
            }
        }

        public override void OnLevelReset()
        {
            base.OnLevelReset();
            transform.position = startPos;
        }
    }
}
