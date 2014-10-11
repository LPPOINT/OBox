using Assets.Scripts.Levels.GradientBackground;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomEditor(typeof(GradientPreset))]
    public class GradientPresetEditor : UnityEditor.Editor
    {

        private Texture2D previewTexture;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        private void CreateTexture()
        {

            var preset = (GradientPreset) target;

            var color1 = preset.Color1;
            var color2 = preset.Color2;

            previewTexture = new Texture2D(1, 2);
            previewTexture.SetPixel(1, 1, color2);
            previewTexture.SetPixel(1, 2, color1);
            previewTexture.filterMode = FilterMode.Bilinear;
            previewTexture.wrapMode = TextureWrapMode.Clamp;
            previewTexture.Apply();
        }

        public override void DrawPreview(Rect previewArea)
        {
            if (previewTexture == null)
                CreateTexture();

            GUI.DrawTexture(previewArea, previewTexture);

        }

        public override bool HasPreviewGUI()
        {
            return true;
        }
    }
}
