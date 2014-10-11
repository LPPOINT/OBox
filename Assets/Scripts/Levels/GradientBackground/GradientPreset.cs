using System;
using UnityEngine;

namespace Assets.Scripts.Levels.GradientBackground
{
    public class GradientPreset : ScriptableObject
    {
        public Color Color1;
        public Color Color2;

        public string GenerateName()
        {
            return "Preset" + Color1 + "to" + Color2;
        }

    }
}
