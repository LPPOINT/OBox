using UnityEngine;

namespace Assets.Scripts.Levels.Style
{
    public class LevelPresetStyle : LevelStyle
    {
        public LevelStylePreset Preset;

        public override Color GetBackgroundGradientColor1()
        {
            return Preset.GetBackgroundGradientColor1();
        }

        public override Color GetBackgroundGradientColor2()
        {
            return Preset.GetBackgroundGradientColor2();
        }

        public override Color GetFrontColor()
        {
            return Preset.GetFrontColor();
        }
    }
}
