using Assets.Scripts.Levels;
using Assets.Scripts.Map.Items;
using UnityEngine;

namespace Assets.Scripts.Map
{
    [RequireComponent(typeof(Player))]
    public class PlayerController : LevelElement
    {
        public Player Player { get; private set; }

        private void Awake()
        {
            Player = GetComponent<Player>();
        }

        public bool CanControl
        {
            get { return Level.Current.State == LevelState.Playing; }
        }

        protected virtual void Update()
        {
            
        }

    }
}
