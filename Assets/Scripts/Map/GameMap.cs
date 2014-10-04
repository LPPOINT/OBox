using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Map.Collision;
using Assets.Scripts.Map.Items;
using Rotorz.Tile;
using UnityEngine;

namespace Assets.Scripts.Map
{

    //[RequireComponent(typeof(TileSystem))]
    public class GameMap : MonoBehaviour
    {




        private TileSystem tileSystem;
        public TileSystem TileSystem
        {
            get { return tileSystem ?? (tileSystem = gameObject.GetComponent<TileSystem>()); }
        }

        public List<MapItem> Items { get; private set; }
 

        public Player Player { get; private set; }

        private void InitializeItems()
        {
            if (Items == null)
            {
                Items = new List<MapItem>();
            }
            else
            {
                Items.Clear();
            }

            Items.AddRange(gameObject.GetComponentsInChildren<MapItem>());

            Player = Items.FirstOrDefault(item => item is Player) as Player;

            foreach (var mapItem in Items)
            {
               mapItem.SetSize(TileSystem.CellSize.x, TileSystem.CellSize.y);
            }

        }

        public void Reset()
        {

        }


        private void Update()
        {

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(2))
            {
                var wp = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var tile = TileSystem.ClosestTileIndexFromWorld(wp);

                var ms = new List<DebugClickModifers>();

                if(Input.GetKey(KeyCode.Space)) ms.Add(DebugClickModifers.Space);
                if (Input.GetKey(KeyCode.LeftControl)) ms.Add(DebugClickModifers.Ctrl);
                if (Input.GetKey(KeyCode.LeftAlt)) ms.Add(DebugClickModifers.Alt);
                if (Input.GetKey(KeyCode.Tab)) ms.Add(DebugClickModifers.Tab);


                var itemInTile = GetItemByIndex(tile.column, tile.row);

                if (itemInTile == null)
                {
                    var player = Items.FirstOrDefault(item => item is Player);
                    if (player != null && !player.IsMoving)
                    {
                        player.SetIndex(tile);
                    }
                }
                else
                {
                    itemInTile.OnDebugClick(ms);
                }
            }
#endif

        }

        private void Start()
        {
            InitializeItems();
        }

        public MapItem GetFirstWallOfDirection(MapItem origin, Direction direction)
        {


            switch (direction)
            {
                case Direction.Left:
                    for (var x = origin.Index.column; x >= 0; x--)
                    {
                        var item = GetItemByIndex(x, origin.Index.row);
                        if (IsItemIsWall(item, origin))
                        {
                            return item;
                        }
                    }
                    return null;
                case Direction.Right:
                    for (var x = origin.Index.column; x <= TileSystem.ColumnCount; x++)
                    {
                        var item = GetItemByIndex(x, origin.Index.row);
                        if (IsItemIsWall(item, origin))
                        {
                            return item;
                        }
                    }
                    return null;
                case Direction.Top:
                    for (var y = origin.Index.row; y >= 0; y--)
                    {
                        var item = GetItemByIndex(origin.Index.column, y);
                        if (IsItemIsWall(item, origin))
                        {
                            return item;
                        }
                    }
                    return null;
                case Direction.Bottom:
                    for (var y = origin.Index.row; y <= TileSystem.RowCount; y++)
                    {
                        var item = GetItemByIndex(origin.Index.column, y);
                        if (IsItemIsWall(item, origin))
                        {
                            return item;
                        }
                    }
                    return null;
                default:
                    return null;
            }
        }

        private bool IsItemIsWall(MapItem item, MapItem origin)
        {
            return item != null && item != origin && item.GetCollider(origin) != MapItemColliderType.MoveThrow && !item.IsPositionsEquals(origin);
        }

        public TileIndex GetItemStop(MapItem origin, MapItem wall)
        {
            var xOffset = origin.Index.column - wall.Index.column;
            var yOffset = origin.Index.row - wall.Index.row;
            var direction = GetDirectionByOffset(new Vector2(xOffset, yOffset));
            var rotatedDirection = SwapDirection(direction);
            var wallOffset = GetOffsetByDirection(rotatedDirection);
            return new TileIndex(wall.Index.row + (int)wallOffset.y, wall.Index.column + (int)wallOffset.x);
        }

        public MapItem GetItemByIndex(int column, int row)
        {
            return Items.FirstOrDefault(item => item.Index.column == column && item.Index.row == row);
        }

        public Vector3 GetOutsidePosition(MapItem item, Direction direction, float speed, TimeSpan outsideTime)
        {


            switch (direction)
            {
                case Direction.Left:
                    return new Vector3(transform.position.x - TileSystem.CellSize.x, item.transform.position.y, transform.position.z);
                case Direction.Top:
                    return new Vector3(item.transform.position.x, transform.position.y - TileSystem.CellSize.y, transform.position.z);
                case Direction.Right:
                    return new Vector3(transform.position.x + TileSystem.CellSize.x * TileSystem.ColumnCount, item.transform.position.y, transform.position.z);
                case Direction.Bottom:
                    return new Vector3(item.transform.position.x, transform.position.y - TileSystem.CellSize.y * TileSystem.RowCount, transform.position.z);
            }
            return new Vector3();
        }

        public IEnumerable<MapItem> GetItemsByIndex(int column, int row)
        {
            return Items.Where(item => item.Index.column == column && item.Index.row == row);
        }

        /// <summary>
        /// Do not use! 
        /// </summary>
        /// <returns></returns>
        private Direction GetDirectionByOffset(Vector2 offset)
        {
            if (offset.x >= 1)
            {
                return Direction.Left;
            }
            else if (offset.x <= -1)
            {
                return Direction.Right;
            }
            else if (offset.y >= 1)
            {
                return Direction.Top;
            }
            else if (offset.y <= -1)
            {
                return Direction.Bottom;
            }
            Debug.LogWarning("Map.GetDirectionByOffset(): Cant calculate direction");
            return Direction.Left;
        }

        /// <summary>
        /// Do not use!
        /// </summary>
        /// <returns></returns>
        private Vector2 GetOffsetByDirection(Direction direction)
        {
            var xOffset = 0;
            var yOffset = 0;

            switch (direction)
            {
                case Direction.Left:
                    xOffset = -1;
                    break;
                case Direction.Right:
                    xOffset = +1;
                    break;
                case Direction.Top:
                    yOffset = -1;
                    break;
                case Direction.Bottom:
                    yOffset = 1;
                    break;
            }

            return new Vector2(xOffset, yOffset);
        }

        public static Direction SwapDirection(Direction sourceDirection)
        {
            switch (sourceDirection)
            {
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                case Direction.Top:
                    return Direction.Bottom;
                case Direction.Bottom:
                    return Direction.Top;
            }
            return Direction.Top;
        }


        public MapItem GetNextItem(MapItem origin, Direction direction)
        {
            var offset = GetOffsetByDirection(direction);
            return GetItemByIndex(origin.Index.column + (int) offset.x, origin.Index.row + (int) offset.y);
        }

        public IEnumerable<T> FindItemsOfType<T>() where T: MapItem
        {
            return transform.GetComponentsInChildren<T>();
        }

        private MapItemCollisionType CalculateTouchCollision(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return MapItemCollisionType.TouchRight;
                case Direction.Right:
                    return MapItemCollisionType.TouchLeft;
                case Direction.Top:
                    return MapItemCollisionType.TouchBottom;
                case Direction.Bottom:
                    return MapItemCollisionType.TouchTop;
                default:
                    return MapItemCollisionType.TouchRight;
            }
        }


        public void ProcessCollisionEnter(MapItem item)
        {

            var count = 0;

            foreach (var i in GetItemsByIndex(item.Index.column, item.Index.row))
            {
                if (i != item)
                {
                    count++;
                    i.OnItemCollisionEnter(MapItemCollisionType.Inside, item);
                }
            }

            var left = GetNextItem(item, Direction.Left);
            var right = GetNextItem(item, Direction.Right);
            var top = GetNextItem(item, Direction.Top);
            var bottom = GetNextItem(item, Direction.Bottom);

            if (left != null) left.OnItemCollisionEnter(CalculateTouchCollision(Direction.Left), item);
            if (right != null) right.OnItemCollisionEnter(CalculateTouchCollision(Direction.Right), item);
            if (top != null) top.OnItemCollisionEnter(CalculateTouchCollision(Direction.Top), item);
            if (bottom != null) bottom.OnItemCollisionEnter(CalculateTouchCollision(Direction.Bottom), item);

        }
        public void ProcessCollisionLeave(MapItem item)
        {

            var count = 0;
            foreach (var i in GetItemsByIndex(item.Index.column, item.Index.row))
            {
                if (i != item)
                {
                    count++;
                    i.OnItemCollisionLeave(MapItemCollisionType.Inside, item);
                }
            }



            var left = GetNextItem(item, Direction.Left);
            var right = GetNextItem(item, Direction.Right);
            var top = GetNextItem(item, Direction.Top);
            var bottom = GetNextItem(item, Direction.Bottom);


            if (left != null) left.OnItemCollisionLeave(CalculateTouchCollision(Direction.Left), item);
            if (right != null) right.OnItemCollisionLeave(CalculateTouchCollision(Direction.Right), item);
            if (top != null) top.OnItemCollisionLeave(CalculateTouchCollision(Direction.Top), item);
            if (bottom != null) bottom.OnItemCollisionLeave(CalculateTouchCollision(Direction.Bottom), item);

        }

        public void ProcessCollisionExtends(MapItem item, TileIndex index, TileIndex oldIndex)
        {
            var items =
                Items.Where(
                    mapItem => mapItem.CollisionDetectionMode == MapItemCollisionDetectionMode.AllTime);

            foreach (var i in GetItemsByIndex(index.column, index.row))
            {
                if (i != item)
                {
                    i.OnItemCollisionEnter(MapItemCollisionType.ExtendsThrough, item);
                }
            }


            foreach (var i in GetItemsByIndex(oldIndex.column, oldIndex.row))
            {
                if (i != item)
                {
                    i.OnItemCollisionLeave(MapItemCollisionType.ExtendsThrough, item);
                }
            }



        }

        public Direction CalculateDirection(TileIndex from, TileIndex to)
        {
            if (from.column > to.column)
            {
                return Direction.Left;
            }
            if (from.column < to.column)
            {
                return Direction.Right;
            }
            if (from.row > to.row)
            {
                return Direction.Top;
            }
            if (from.row < to.row)
            {
                return Direction.Bottom;
            }
            return Direction.Top;
        }

    }
}
