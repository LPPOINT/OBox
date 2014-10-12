using Assets.Scripts.Levels;
using Assets.Scripts.Levels.Style;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Levels.Style
{
    [CustomEditor(typeof(LevelStylePreset))]
    public class LevelStylePresetEditor : UnityEditor.Editor
    {

        private Texture2D previewGradientTexture;
        private Texture2D previewFrontTexture;

        private void CreateGradientTexture()
        {
            var p = (LevelStylePreset) target;

            var color1 = p.BackgroundGradientColor1;
            var color2 = p.BackgroundGradientColor2;

            previewGradientTexture = new Texture2D(1, 2);
            previewGradientTexture.SetPixel(1, 1, color2);
            previewGradientTexture.SetPixel(1, 2, color1);
            previewGradientTexture.filterMode = FilterMode.Bilinear;
            previewGradientTexture.wrapMode = TextureWrapMode.Clamp;
            previewGradientTexture.Apply();
        }

        private void CreateFrontTexture()
        {
            var p = (LevelStylePreset)target;


            previewFrontTexture = new Texture2D(1, 1);
            previewFrontTexture.SetPixel(1, 1, p.FrontColor);
            previewFrontTexture.filterMode = FilterMode.Bilinear;
            previewFrontTexture.wrapMode = TextureWrapMode.Clamp;
            previewFrontTexture.Apply();
        }

        public override void DrawPreview(Rect previewArea)
        {

            if (previewGradientTexture == null)
                CreateGradientTexture();
            if(previewFrontTexture == null)
                CreateFrontTexture();

            var frontWidth = previewArea.width/5;
            var frontHeight = previewArea.height/5;

            GUI.DrawTexture(previewArea, previewGradientTexture);
            GUI.DrawTexture(
                    new Rect(previewArea.x + previewArea.width / 2 - frontWidth / 2, previewArea.y + previewArea.height / 2 - frontHeight / 2, frontWidth, frontHeight),
                    previewFrontTexture
                );

        }

        public override bool HasPreviewGUI()
        {
           
            return true;
        }
    }
}
