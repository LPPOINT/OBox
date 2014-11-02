using System;
using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public static class GameGlobalEvents
    {
        public static event EventHandler<LevelEventArgs> LevelEvent;
        public static event EventHandler GameStarted;

        public static void OnGameStarted()
        {
            var handler = GameStarted;
            if (handler != null) handler(null, EventArgs.Empty);
        }

        public static void OnLevelEvent(LevelEventArgs e)
        {
         

            var handler = LevelEvent;
            if (handler != null) handler(null, e);
        }
    }

    public class LevelEventArgs : EventArgs
    {
        public LevelEventArgs(LevelEvent @event, Level level)
        {
            Event = @event;
            Level = level;
        }

        public LevelEvent Event { get; private set; }
        public Level Level { get; private set; }
    }

}
