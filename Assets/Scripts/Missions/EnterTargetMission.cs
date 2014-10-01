using Assets.Scripts.Levels;

namespace Assets.Scripts.Missions
{
    public class EnterTargetMission : LevelMission
    {
        [LevelEventFilter(typeof(LevelEvent), typeof(Map.Items.Target))]
        public void OnTargetEnter(LevelEvent e)
        {
            RegisterMissionDone();
        }

    }
}
