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

        [ContextMenu("Convert to custom style")]
        public void ConvertToCustom()
        {

            if (Application.isPlaying)
            {
                Debug.Log("ConvertToCustom() call detected in play mode.");
                return;
            }

            var custom = gameObject.AddComponent<LevelCustomStyle>();
            custom.FrontColor = GetFrontColor();
            custom.BackgroundGradientColor1 = GetBackgroundGradientColor1();
            custom.BackgroundGradientColor2 = GetBackgroundGradientColor2();

            if (GradientBackground.GradientBackground.MainGradient != null && GradientBackground.GradientBackground.MainGradient.LevelStyle == this)
            {
                GradientBackground.GradientBackground.MainGradient.LevelStyle = custom;
            }

            DestroyImmediate(this);
        }

    }
}
