using System.Linq;
using Assets.Scripts.Levels;
using Assets.Scripts.Map.Collision;
using UnityEngine;

namespace Assets.Scripts.Map.Items
{
    public class Teleporter : MapItem
    {
        public Teleporter Other;


        private const string inAnimation = "TeleportIn";
        private const string outAnimation = "TeleportOut";

        public bool WaitBeforeTeleport = true;
        public float WaitTime = 0.5f;

        private Player innerPlayer;
        private Direction innerPlayerDirection;
        private float currentWaitTime;
        private bool isWaiting;

        public enum TeleporterStyle
        {
            Style1,
            Style2,
            Style3
        }

        public TeleporterStyle Style;

        public void ApplyStyle()
        {
            if (Style == TeleporterStyle.Style1) transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (Style == TeleporterStyle.Style2) transform.rotation = Quaternion.Euler(0, 0, 45);
            else if (Style == TeleporterStyle.Style3) transform.rotation = Quaternion.Euler(0, 0, 15);
        }

        public void MakeTeleportation(Player player, Direction direction)
        {

            if (Other == this)
            {
                Debug.LogWarning("MakeTeleportation loop detected");
                return;
            }

            

            PlayInAnimation();
            Other.PlayOutAnimation();

            if (!WaitBeforeTeleport)
            {
                player.SetIndex(Other.Index);
                player.Move(direction, MoveSource.Teleporter);
            }
            else
            {

                player.GetComponent<SpriteRenderer>().enabled = false;
                Level.Current.LockInput();

                innerPlayer = player;
                isWaiting = true;
                innerPlayerDirection = direction;
                currentWaitTime = 0;
            }
        }

        public void PlayInAnimation()
        {
            GetComponent<Animator>().Play(inAnimation);
        }

        public void PlayOutAnimation()
        {
            GetComponent<Animator>().Play(outAnimation);
        }

        protected override void Start()
        {
            base.Start();

            if (Other == null)
            {
                Other = GameMap.FindItemsOfType<Teleporter>().FirstOrDefault(teleporter => teleporter != this && teleporter.Style == Style);
            }

        }

        protected override void Update()
        {
            base.Update();

            if (isWaiting)
            {
                currentWaitTime += Time.deltaTime;
                if (currentWaitTime > WaitTime)
                {
                    Level.Current.Play();
                    innerPlayer.GetComponent<SpriteRenderer>().enabled = true;
                    currentWaitTime = 0;

                    isWaiting = false;

                    innerPlayer.SetIndex(Other.Index);
                    innerPlayer.Move(innerPlayerDirection, MoveSource.Teleporter);

                    innerPlayer = null;

                }
            }

        }

        public override MapItemColliderType GetCollider(MapItem other)
        {
            return MapItemColliderType.GoInside;
        }

        public override void OnItemCollisionEnter(MapItemCollisionType collisionType, MapItem other)
        {



            if (collisionType == MapItemCollisionType.Inside
                && other is Player &&
                ((other as Player).GetLastMove() as ToCellItemMove).To != Other.Index)
            {
                var p = other as Player;
                MakeTeleportation(p, p.GetLastMove().Direction);
            }
        }

        protected override void OnLevelReset()
        {
            base.OnLevelReset();
            isWaiting = false;
            currentWaitTime = 0;
        }
    }
}
