using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Levels.GradientBackground
{

    [ExecuteInEditMode]
    public class RandomPresetBackground : GradientBackground
    {

        [Tooltip("Leave this empty to select from all presets")]
        public List<GradientPreset> AllowedPresets;

        protected override void Start()
        {

            if (AllowedPresets == null || !AllowedPresets.Any())
            {
                AllowedPresets = new List<GradientPreset>(Resources.LoadAll<GradientPreset>("GradientPresets/"));
                Debug.Log(AllowedPresets.Count);
            }

            var presetIndex = Random.Range(0, AllowedPresets.Count - 1);
            var preset = AllowedPresets[presetIndex];

            Color1 = preset.Color1;
            Color2 = preset.Color2;

            base.Start();
        }
    }
}
