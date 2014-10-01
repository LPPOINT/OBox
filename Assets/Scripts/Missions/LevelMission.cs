using Assets.Scripts.Levels;

namespace Assets.Scripts.Missions
{
    public class LevelMission : LevelElement
    {
        public class MissionDoneEvent : LevelEvent
        {
            
        }

        protected void RegisterMissionDone()
        {
            FireEvent(new MissionDoneEvent());
        }

    }
}
