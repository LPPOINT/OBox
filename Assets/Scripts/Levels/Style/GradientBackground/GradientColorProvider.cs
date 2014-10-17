using UnityEngine;

namespace Assets.Scripts.Levels.Style.GradientBackground
{
    public class GradientColorProvider :  IGradientColorProvider
    {
        public GradientColorProvider(Color color1, Color color2)
        {
            Color1 = color1;
            Color2 = color2;
        }

        public Color Color1;
        public Color Color2;

        public Color GetBackgroundGradientColor1()
        {
            return Color1;
        }

        public Color GetBackgroundGradientColor2()
        {
            return Color2;
        }
    }
}
