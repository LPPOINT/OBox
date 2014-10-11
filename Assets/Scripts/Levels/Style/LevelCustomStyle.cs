using UnityEngine;

namespace Assets.Scripts.Levels.Style
{
    public class LevelCustomStyle : LevelStyle
    {

        public Color BackgroundGradientColor1;
        public Color BackgroundGradientColor2;
        public Color FrontColor;

        public override Color GetBackgroundGradientColor1()
        {
            return BackgroundGradientColor1;
        }

        public override Color GetBackgroundGradientColor2()
        {
            return BackgroundGradientColor2;
        }

        public override Color GetFrontColor()
        {
            return FrontColor;
        }
    }
}
