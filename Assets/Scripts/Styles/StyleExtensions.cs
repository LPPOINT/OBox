using UnityEngine;

namespace Assets.Scripts.Styles
{
    public static class StyleExtensions
    {
        public static Color GetBackgroundGradientColor1(this IStyle style)
        {
            return style.GetColor(StyleNames.BackgroundGradientColor1);
        }
        public static Color GetBackgroundGradientColor2(this IStyle style)
        {
            return style.GetColor(StyleNames.BackgroundGradientColor2);
        }
        public static Color GetFrontColor(this IStyle style)
        {
            return style.GetColor(StyleNames.LevelFrontColor);
        }
    }
}
