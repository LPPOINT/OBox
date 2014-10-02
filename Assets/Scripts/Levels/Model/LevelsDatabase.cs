using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Levels.Model
{
    public class LevelsDatabase : ScriptableObject
    {

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public List<LevelModel> Levels;
    }
}
