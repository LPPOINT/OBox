using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class LevelResultsUI : MonoBehaviour
    {
        public Level Level { get; set; }

        public void OnRetryClick()
        {
            Level.Reset();
        }

        public void OnMenuClick()
        {
            Level.LoadLevel(1);
        }

        public void OnNextLevelClick()
        {
            Level.LoadNextLevel();
        }

    }
}
