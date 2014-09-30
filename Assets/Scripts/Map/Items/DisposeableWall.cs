using System;
using System.Globalization;
using Assets.Scripts.Map.Collision;
using Assets.Scripts.Map.Serialization;
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

        public enum DisposeableWallStatus
        {
            NotActivated,
            Activated,
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
                    ForceActivationEnd();
                }
            }

            base.Update();
        }

        private void ForceActivationEnd()
        {
            gameObject.GetComponent<Animator>().Play("WallDone");
            Status = DisposeableWallStatus.Done;
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
            //else if (Status == DisposeableWallStatus.Activated && ( other == Activator || Activator is DisposeableWall))
            //{
            //    ForceActivationEnd();


            //        switch (collision)
            //        {
            //            case MapItemCollision.TouchLeft:
            //                player.Move(Direction.Right);
            //                break;
            //            case MapItemCollision.TouchRight:
            //                player.Move(Direction.Left);
            //                break;
            //            case MapItemCollision.TouchTop:
            //                player.Move(Direction.Bottom);
            //                break;
            //            case MapItemCollision.TouchBottom:
            //                player.Move(Direction.Top);
            //                break;
            //        }
            //}
        }

        public void Activate(MapItem other)
        {
            Status = DisposeableWallStatus.Activated;
            CurrentTime = 0;
            TimeUI.gameObject.SetActive(true);
            Activator = other;
        }

        public override void OnLevelReset()
        {
            base.OnLevelReset();
            Activator = null;
            CurrentTime = 0;
            Status = DisposeableWallStatus.NotActivated;
            gameObject.GetComponent<Animator>().Play("Wallidle");
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
