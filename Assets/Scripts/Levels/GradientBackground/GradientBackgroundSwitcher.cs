using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Levels.GradientBackground
{

    [RequireComponent(typeof(GradientBackground))]
    public class GradientBackgroundSwitcher : GradientBackground
    {

        public float MinSwitchTime;
        public float MaxSwitchTime;

        public float MinSwitchingDuration;
        public float MaxSwitchingDuration;

        public List<GradientPreset> Presets; 

        public GradientBackground Background { get; private set; }


        public void NextPreset()
        {
            
        }

        private void Start()
        {
            Background = GetComponent<GradientBackground>();
        }

    }
}
