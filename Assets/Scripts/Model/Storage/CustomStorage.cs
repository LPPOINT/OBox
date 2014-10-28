using System;
using UnityEngine;

namespace Assets.Scripts.Model.Storage
{
    public class CustomStorage : MonoBehaviour
    {
        public enum StorageType
        {
            PlayerPrefs,
            NewGame,
            AllUnlocked
        }

        public StorageType Type;
        public IModelStorage Storage;

        public static CustomStorage Instance { get; private set; }

        private void Awake()
        {
            Instance = this;

            switch (Type)
            {
                case StorageType.PlayerPrefs:
                    Storage = new PlayerPrefsStorage();
                    break;
                case StorageType.NewGame:
                    Storage = new NewGameModelStorage();
                    break;
                case StorageType.AllUnlocked:
                    Storage = new FullUnlockedModelStorage();
                    break;
                default:
                    Storage = new NewGameModelStorage();
                    break;
            }
        }

        public static bool IsExist
        {
            get { return Instance != null; }
        }
    }
}
