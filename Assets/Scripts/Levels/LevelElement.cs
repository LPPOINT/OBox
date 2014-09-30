using Assets.Scripts.Map;
using Assets.Scripts.Map.Items;
using UnityEngine;

namespace Assets.Scripts.Levels
{
    public class LevelElement : MonoBehaviour
    {

        public Level Level
        {
            get { return Level.Current; }
        }

        public virtual void OnLevelReset()
        {
            
        }

        public virtual void OnLevelStarted()
        {
            
        }

        public virtual void OnMenuOpen()
        {
            
        }

        public virtual void OnMenuClosed()
        {
            
        }

        public virtual void OnLevelEnded()
        {
            
        }

        public virtual void OnPlayerMoveBegin(Player player, MapItemMove move)
        {
            
        }

        public virtual void OnPlayerMoveEnd(Player player, MapItemMove move)
        {
            
        }

        public virtual void OnLevelStateChanged(LevelState oldState, LevelState newState)
        {
            
        }

    }
}
