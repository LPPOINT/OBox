using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class LevelResultsUI : MonoBehaviour
    {
        public Level Level { get; set; }
        public LevelResultsUIModel Model { get; set; }

        public void OnRetryClick()
        {
            Level.ResetLevel();
        }


    }
}
