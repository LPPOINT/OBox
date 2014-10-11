using UnityEngine;

namespace Assets.Scripts.Levels.Style
{
    public class LevelStylePreset : ScriptableObject, ILevelStyle
    {
        public Color BackgroundGradientColor1;
        public Color BackgroundGradientColor2;
        public Color FrontColor;

        public Color GetBackgroundGradientColor1()
        {
            return BackgroundGradientColor1;
        }

        public Color GetBackgroundGradientColor2()
        {
            return BackgroundGradientColor2;
        }

        public Color GetFrontColor()
        {
            return FrontColor;
        }
    }
}
