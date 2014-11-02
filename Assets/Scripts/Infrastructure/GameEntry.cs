using Assets.Scripts.Model;
using Assets.Scripts.Model.Storage;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public static class GameEntry
    {
        public static void Main()
        {
            Debug.Log("Game started");
            GameModel.Create();
            GameGlobalEvents.OnGameStarted();
        }
    }
}
