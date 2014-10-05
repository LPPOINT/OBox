using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Levels;
using Assets.Scripts.Map.Collision;
using Assets.Scripts.Map.Decorations;
using Rotorz.Tile;
using UnityEngine;


namespace Assets.Scripts.Map
{
    public class MapItem : LevelElement
    {

        private GameMap gameMap;
        public GameMap GameMap
        {
            get { return gameMap ?? (gameMap = gameObject.GetComponentInParent<GameMap>()); }

        }

        public TileSystem TileSystem
        {
            get { return GameMap.TileSystem; }
        }
        public TileData TileData { get; set; }
        public TileIndex Index { get; set; }

        public bool IsOutside { get; set; }

        public bool IsNear(TileIndex otherIndex)
        {
            return Math.Abs(Index.row - otherIndex.row) == 1
                   || Math.Abs(Index.column - otherIndex.column) == 1;
        }
        public bool IsNear(MapItem other)
        {
            return IsNear(other.Index);
        }

        public class MapItemMoveEvent : LevelEvent
        {


            public enum MoveState
            {
                Started,
                Done
            }

            public MapItemMoveEvent(MapItemMove move, MoveState state)
            {
                State = state;
                Move = move;
            }

            public MapItemMove Move { get; private set; }
            public MoveState State { get; private set; }
        }



        private Decoration GetDecorationWithPlaymode(DecorationPlaymode playmode)
        {
            var parent =
                GetComponentsInParent<Decoration>().FirstOrDefault(decoration => decoration.Playmode == playmode);
            if (parent != null)
            {
                var native = GetComponents<Decoration>().FirstOrDefault(decoration => decoration.Playmode == playmode);
                if (native == null && parent.CanBeRoot)
                {
                    return parent;
                }
                if (native != null)
                    return native;
                return null;
            }
            return GetComponents<Decoration>().FirstOrDefault(decoration => decoration.Playmode == playmode);
        }


        protected virtual void SetupDefaultDecoration(DecorationPlaymode playmode)
        {
            if (playmode == DecorationPlaymode.In)
            {
                var d = gameObject.AddComponent<ScaleFromDecoration>();
                d.Playmode = DecorationPlaymode.In;
            }
            else if (playmode == DecorationPlaymode.Out)
            {
                var d = gameObject.AddComponent<ScaleToDecoration>();
                d.Playmode = DecorationPlaymode.Out;
            }
        }

        public Decoration InDecoration
        {
            get { return GetDecorationWithPlaymode(DecorationPlaymode.In); }
        }
        public Decoration OutDecoration
        {
            get
            {
                return GetDecorationWithPlaymode(DecorationPlaymode.Out); 
            }
        }

        public void AddDecorationsIfNecessary()
        {
            if (InDecoration == null)
            {
                SetupDefaultDecoration(DecorationPlaymode.In);
            }
            if (OutDecoration == null)
            {
                SetupDefaultDecoration(DecorationPlaymode.Out);
            }
        }

        private MapItemMove currentMove;
        private MapItemMove lastMove;
        public bool IsMoving { get; private set; }

        public MapItemMove GetActiveMove()
        {
            return GetCurrentMove() ?? GetLastMove();
        }
        public MapItemMove GetCurrentMove()
        {
            return currentMove;
        }
        public MapItemMove GetLastMove()
        {
            return lastMove;
        }

        public virtual void OnDebugClick(IEnumerable<DebugClickModifers> modifers)
        {
            
        }

        public void SetSize(float width, float height)
        {

            var targetXSize = width;
            var targetYSize = height;


            var currentXSize = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
            var currentYSize = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;



            var scale = gameObject.transform.localScale;

            scale.x = targetXSize * scale.x / currentXSize;
            scale.y = targetYSize * scale.y / currentYSize;


            gameObject.transform.localScale = scale;
        }



        public void RefreshIndex()
        {
            Index = TileSystem.ClosestTileIndexFromWorld(transform.position);
            TileData = TileSystem.GetTile(Index);
        }

        protected virtual void Start()
        {
            RefreshIndex();
            AddDecorationsIfNecessary();
        }

        protected virtual void Update()
        {
            
        }

        public override void OnLevelStateChanged(LevelState oldState, LevelState newState)
        {
            base.OnLevelStateChanged(oldState, newState);
            if (newState == LevelState.Playing)
            {
                if (IsMoveSuspended)
                {
                    ResumeMove();
                }
            }
            else if (newState == LevelState.Paused)
            {
                if (!IsMoveSuspended)
                {
                    SuspendMove();
                }
            }
        }

        public virtual void OnIndexChanged(TileIndex oldIndex, TileIndex newIndex)
        {
            
        }

        public virtual void OnMoveStart(MapItemMove move)
        {
            FireEvent(new MapItemMoveEvent(move, MapItemMoveEvent.MoveState.Started));
        }

        public virtual void OnMoveDone(MapItemMove move)
        {
            FireEvent(new MapItemMoveEvent(move, MapItemMoveEvent.MoveState.Done));
        }

        public virtual void OnItemCollisionEnter(MapItemCollisionType collisionType, MapItem other)
        {
        }

        public virtual void OnItemCollisionLeave(MapItemCollisionType collisionType, MapItem other)
        {
        }

        public virtual MapItemCollisionDetectionMode CollisionDetectionMode
        {
            get
            {
                return MapItemCollisionDetectionMode.OnlyWhenOriginStops;
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
        }

        protected virtual void OnCollisionExit2D(Collision2D collision)
        {
            
        }

        public virtual MapItemColliderType GetCollider(MapItem other)
        {
            return MapItemColliderType.StopNear;
        }


        public void SetIndex(TileIndex index)
        {



            if (IsMoving)
            {
                Debug.LogWarning("Trying to set index of map item while item.IsMoving == true.");
                IsMoving = false;
                lastMove = currentMove;
                currentMove = null;
            }

            GameMap.ProcessCollisionLeave(this);

            if (index == Index)
            {
                return;
            }

            if (TileData == null)
            {
                Debug.LogWarning("SetIndex(): TileData == null");
            }

            IsOutside = false;
            var newPos = TileSystem.WorldPositionFromTileIndex(index);
            transform.position = newPos;

            var oldIndex = Index;
            Index = index;

            GameMap.ProcessCollisionEnter(this);


            OnIndexChanged(oldIndex, Index);


        }

        private void OnITweenMoveDone()
        {
            IsMoving = false;
            lastMove = currentMove;
            currentMove = null;
            var lastIndex = Index;

            if (lastMove is ToCellItemMove)
            {
                Index = (lastMove as ToCellItemMove).To;
                IsOutside = false;
            }
            else if(lastMove is OutsideItemMove)
            {
                Index = new TileIndex(-1, -1);
                IsOutside = true;
            }

            OnIndexChanged(lastIndex, Index);
            OnMoveDone(lastMove);
 

            GameMap.ProcessCollisionEnter(this);

        }

        private void OnITweenMoveStart()
        {
            IsMoving = true;
            OnMoveStart(currentMove);
            GameMap.ProcessCollisionLeave(this);
        }


        private TileIndex lastMoveIndex;
        private void OnITweenMoveUpdate()
        {
            var index = TileSystem.ClosestTileIndexFromWorld(transform.position);

            if (index != lastMoveIndex)
            {

                GameMap.ProcessCollisionExtends(this, index, lastMoveIndex);
                lastMoveIndex = index;
            }

        }

        public void Move(TileIndex to)
        {
            Move(to, 30);
        }
        public MapItemMove Move(TileIndex to, float speed, MoveSource moveSource = MoveSource.User)
        {
            var move = MapItemMove.ToCell(this, to);
            move.Source = moveSource;
            move.Speed = speed;
            return Move(move);
        }
        public MapItemMove Move(MapItemMove move)
        {

            if (move is ToCellItemMove)
            {
                var toCellMove = move as ToCellItemMove;
                if (IsMoving || Index == toCellMove.To)
                {
                    lastMove = new EmptyItemMove(move as ToCellItemMove);
                    return move;
                }

                var newPos = TileSystem.WorldPositionFromTileIndex(toCellMove.To);
                var hash = iTween.Hash("position", newPos, "speed", move.Speed, "easetype", move.EaseType, "oncomplete",
                    "OnITweenMoveDone", "onstart", "OnITweenMoveStart", "onupdate", "OnITweenMoveUpdate");
                iTween.MoveTo(gameObject, hash);
                lastMoveIndex = Index;
                currentMove = move;
            }
            else if (move is OutsideItemMove)
            {
                var outsideMove = move as OutsideItemMove;
                var outsidePos = GameMap.GetOutsidePosition(this, outsideMove.Direction, outsideMove.Speed, outsideMove.TimeInOutside);

                var hash = iTween.Hash("position", outsidePos, "speed", move.Speed, "easetype", move.EaseType, "oncomplete",
                    "OnITweenMoveDone", "onstart", "OnITweenMoveStart", "onupdate", "OnITweenMoveUpdate");
                iTween.MoveTo(gameObject, hash);
                lastMoveIndex = Index;
                currentMove = move;

            }
            return move;
        }

        public bool IsMoveSuspended { get; private set; }

        public void SuspendMove()
        {
            IsMoveSuspended = true;
            var itween = GetComponent<iTween>();
            if (itween != null)
            {
                iTween.Pause(gameObject);
            }
            OnMoveSuspended();
        }

        public void ResumeMove()
        {
            IsMoveSuspended = false;
            var itween = GetComponent<iTween>();
            if (itween != null)
            {
                iTween.Resume(gameObject);
            }
            OnMoveResumed();
        }

        protected virtual void OnMoveSuspended()
        {
            
        }

        protected virtual void OnMoveResumed()
        {
            
        }

        public MapItemMove MoveOutside(Direction direction, MoveSource moveSource = MoveSource.User)
        {
            var move = MapItemMove.Outside(this, direction);
            move.Source = moveSource;
            return Move(move);
        }

        public bool IsPositionsEquals(MapItem other)
        {
            return other.Index.Equals(Index);
        }

        public override void OnLevelReset()
        {
            IsOutside = false;
            currentMove = null;
            lastMove = null;
            IsMoving = false;
        }


    }
}
