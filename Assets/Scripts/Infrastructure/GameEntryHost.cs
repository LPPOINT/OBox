using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class GameEntryHost : MonoBehaviour
    {
        private void Start()
        {
            GameEntry.Main();
        }
    }
}
