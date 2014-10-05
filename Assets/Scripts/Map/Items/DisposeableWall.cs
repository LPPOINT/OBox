using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Assets.Scripts.Levels;
using Assets.Scripts.Map.Collision;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Map.Items
{
    public class DisposeableWall : Wall
    {
        public float Time;
        public Text TimeUI;

        public bool ShouldActivateAfterStart;

        public bool IsTransmitteable;
        public float MinTimeBeforeTransmission;
        public float MaxTimeBeforeTransmission;
        public float TimeBeforeTransmission { get; private set; }
        private bool isTransmitted;


        private const string IdleAnimation = "DWallIdle";
        private const string ShowingAnimation = "DWallShowing";
        private const string DisposingAnimation = "DWallDone";

        public class DisposeableWallStatusChangedEvent : LevelEvent
        {
            public DisposeableWallStatusChangedEvent(DisposeableWallStatus oldStatus, DisposeableWallStatus newStatus)
            {
                NewStatus = newStatus;
                OldStatus = oldStatus;
            }


            public DisposeableWallStatus OldStatus { get; private set; }
            public DisposeableWallStatus NewStatus { get; private set; }
        }

        public class DisposeableWallActivatedEvent : DisposeableWallStatusChangedEvent
        {
            public DisposeableWallActivatedEvent() : base(DisposeableWallStatus.NotActivated, DisposeableWallStatus.Activated)
            {
            }
        }

        public class DisposeableWallDestroyedEvent : DisposeableWallStatusChangedEvent
        {
            public DisposeableWallDestroyedEvent()
                : base(DisposeableWallStatus.Activated, DisposeableWallStatus.Done)
            {
            }
        }

        public override void OnDebugClick(IEnumerable<DebugClickModifers> modifers)
        {
            base.OnDebugClick(modifers);
            if (!modifers.Any())
            {
                if(Status == DisposeableWallStatus.NotActivated) Activate(null);
                else if(Status == DisposeableWallStatus.Activated) Dispose();
            }
        }


        private void UpdateTransmissionTime()
        {
            TimeBeforeTransmission = Random.Range(MinTimeBeforeTransmission, MaxTimeBeforeTransmission);
        }
        public void Transmit()
        {
            isTransmitted = true;

            var left = GameMap.GetNextItem(this, Direction.Left);
            var right = GameMap.GetNextItem(this, Direction.Right);
            var top = GameMap.GetNextItem(this, Direction.Top);
            var bottom = GameMap.GetNextItem(this, Direction.Bottom);

            if (left is DisposeableWall)
            {
                (left as DisposeableWall).Activate(this);
            }
            if (right is DisposeableWall)
            {
                (right as DisposeableWall).Activate(this);
            }
            if (top is DisposeableWall)
            {
                (top as DisposeableWall).Activate(this);
            }
            if (bottom is DisposeableWall)
            {
                (bottom as DisposeableWall).Activate(this);
            }

        }

        public float CurrentTime { get; private set; }

        public override void OnLevelStateChanged(LevelState oldState, LevelState newState)
        {
            if(newState == LevelState.Playing && Status == DisposeableWallStatus.Paused) Status = DisposeableWallStatus.Activated;
            else if(newState == LevelState.Paused && Status == DisposeableWallStatus.Activated) Status = DisposeableWallStatus.Paused;
        }

        public enum DisposeableWallStatus
        {
            NotActivated,
            Activated,
            Paused,
            Done
        }
        public DisposeableWallStatus Status { get; private set; }

        public MapItem Activator { get; private set; }

        protected override void Start()
        {
            base.Start();
            Status = DisposeableWallStatus.NotActivated;
            TimeUI.text = ((int)Time).ToString();

            UpdateTransmissionTime();

            if (ShouldActivateAfterStart)
            {
                Activate(GameMap.Player);
            }

        }

        protected override void Update()
        {

            if(Status == DisposeableWallStatus.Done)
                return;

            if (Status == DisposeableWallStatus.Activated)
            {
                CurrentTime += UnityEngine.Time.deltaTime;


                var remTime = ((int) (Time - CurrentTime));
                TimeUI.text = remTime.ToString(CultureInfo.InvariantCulture);

                if (!isTransmitted && CurrentTime > TimeBeforeTransmission && IsTransmitteable)
                {
                    Transmit();
                }

                if (remTime <= 0)
                {
                    Dispose();
                }
            }

            base.Update();
        }

        public void Dispose()
        {
            gameObject.GetComponent<Animator>().Play(DisposingAnimation);
            Status = DisposeableWallStatus.Done;
            FireEvent(new DisposeableWallDestroyedEvent());
        }


        public override MapItemColliderType GetCollider(MapItem other)
        {
            if (Status == DisposeableWallStatus.Done)
            {
                return MapItemColliderType.MoveThrow;
            }
            return MapItemColliderType.StopNear;
        }

        public override void OnItemCollisionEnter(MapItemCollisionType collisionType, MapItem other)
        {


            if (!(other is Player))
            {
                return;
            }
            var player = other as Player;


            if (collisionType == MapItemCollisionType.TouchBottom && player.GetLastDirection() != Direction.Top
                || collisionType == MapItemCollisionType.TouchLeft && player.GetLastDirection() != Direction.Right
                || collisionType == MapItemCollisionType.TouchTop && player.GetLastDirection() != Direction.Bottom
                || collisionType == MapItemCollisionType.TouchRight && player.GetLastDirection() != Direction.Left
                || collisionType == MapItemCollisionType.Inside)
            {
                return;
            }

            base.OnItemCollisionEnter(collisionType, other);

            if (Status == DisposeableWallStatus.NotActivated)
            {
                Activate(other);
            }

            else if (Status == DisposeableWallStatus.Activated 
                && (other == Activator || Activator is DisposeableWall) 
                && other.GetActiveMove() is EmptyItemMove)
            {



                Dispose();


                switch (collisionType)
                {
                    case MapItemCollisionType.TouchLeft:
                        player.Move(Direction.Right);
                        break;
                    case MapItemCollisionType.TouchRight:
                        player.Move(Direction.Left);
                        break;
                    case MapItemCollisionType.TouchTop:
                        player.Move(Direction.Bottom);
                        break;
                    case MapItemCollisionType.TouchBottom:
                        player.Move(Direction.Top);
                        break;
                }
            }
        }

        public void Activate(MapItem other)
        {
            Status = DisposeableWallStatus.Activated;
            CurrentTime = 0;
            TimeUI.gameObject.SetActive(true);
            Activator = other;
            FireEvent(new DisposeableWallActivatedEvent());
        }

        public override void OnLevelStarted()
        {
            base.OnLevelReset();
            Activator = null;
            CurrentTime = 0;
            Status = DisposeableWallStatus.NotActivated;
            gameObject.GetComponent<Animator>().Play(IdleAnimation);
            isTransmitted = false;
            TimeUI.text = ((int)Time).ToString();

            UpdateTransmissionTime();

            if (ShouldActivateAfterStart)
            {
                Activate(GameMap.Player);
            }
        }

    }
}
