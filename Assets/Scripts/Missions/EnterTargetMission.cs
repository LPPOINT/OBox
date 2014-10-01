using Assets.Scripts.Levels;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Missions
{
    public class EnterTargetMission : LevelMission
    {
        [LevelEventFilter(typeof(LevelEvent), typeof(Map.Items.Target))]
        public void OnTargetEnter(LevelEvent e)
        {
            Debug.Log("OnTargetEnter");
            RegisterMissionDone();
        }

        public override void OnLevelEvent(LevelEvent e)
        {
            //if (e.Element is Map.Items.Target)
            //{
            //    Debug.Log("OnTargetEnter");
            //    RegisterMissionDone();
            //}
        }
    }
}
