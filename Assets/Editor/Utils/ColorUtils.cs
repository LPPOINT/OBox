using UnityEngine;
using UnityEngine.UI;

namespace Assets.Editor.Utils
{

    public enum RendererColor
    {
        White,
        StyledBrown,
        Custom
    }

    public static class ColorUtils
    {

        public static Color StyledBrownColor = new Color(0.57254902f, 0.27843137254f, 0.31764705882f, 1);


        public static void SetRendererColor(SpriteRenderer renderer, RendererColor color)
        {
            if (color == RendererColor.StyledBrown)
            {
                renderer.color = StyledBrownColor;
            }
            else if (color == RendererColor.White)
            {
                renderer.color = Color.white;
            }
        }

        public static RendererColor CalculateRendererColor(SpriteRenderer renderer)
        {
            if(renderer.color == Color.white) return RendererColor.White;
            if(renderer.color == StyledBrownColor) return RendererColor.StyledBrown;
            return RendererColor.Custom;
        }


        public static void SetUIElementColor(Graphic uiElement, RendererColor color)
        {
            if (color == RendererColor.StyledBrown)
            {
                uiElement.color = StyledBrownColor;
            }
            else if (color == RendererColor.White)
            {
                uiElement.color = new Color(1, 1, 1, uiElement.color.a);
            }
        }

        public static RendererColor CalculateUIElementColor(Graphic uiElement)
        {
            if (uiElement.color.r == 255 && uiElement.color.g == 255 && uiElement.color.b == 255) return RendererColor.White;
            if (uiElement.color == StyledBrownColor) return RendererColor.StyledBrown;
            return RendererColor.Custom;
        }

    }
}
