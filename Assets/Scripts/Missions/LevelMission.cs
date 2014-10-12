using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts.Missions
{
    public class LevelMission : LevelElement
    {
        public class MissionDoneEvent : LevelEvent
        {
            
        }

        [Tooltip("128x128 only")]
        public Sprite Icon;

        public string Description;

        protected void RegisterMissionDone()
        {
            FireEvent(new MissionDoneEvent());
        }

    }
}
