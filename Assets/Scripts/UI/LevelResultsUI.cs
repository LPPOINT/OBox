using Assets.Scripts.Levels;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Map
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
