using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class GameConstants : MonoBehaviour
    {
        public static GameConstants Instance { get; private set; }

        private void Start()
        {
            Instance = this;
        }

    }
}
