using Assets.Scripts.Map;
using Assets.Scripts.Map.Items;
using UnityEngine;

namespace Assets.Scripts.Levels
{
    public class Godmode : LevelElement
    {

        private void Start()
        {
            Controller = FindObjectOfType<KeyboardPlayerController>();
            Player = FindObjectOfType<Player>();
            Map = FindObjectOfType<GameMap>();
        }

        public KeyboardPlayerController Controller { get; private set; }
        public Player Player { get; private set; }
        public GameMap Map { get; private set; }


        public bool ShowLabel = true;

        public void UpdateGodmode()
        {
            Controller.LockLeftDirection = Map.GetFirstWallOfDirection(Player, Direction.Left) == null;
            Controller.LockRightDirection = Map.GetFirstWallOfDirection(Player, Direction.Right) == null;
            Controller.LockTopDirection = Map.GetFirstWallOfDirection(Player, Direction.Top) == null;
            Controller.LockBottomDirection = Map.GetFirstWallOfDirection(Player, Direction.Bottom) == null;
        }


        protected override void OnLevelStarted()
        {
            UpdateGodmode();
        }

        protected override void OnPlayerMoveEnd(Player player, MapItemMove move)
        {
            UpdateGodmode();
        }
    }
}
