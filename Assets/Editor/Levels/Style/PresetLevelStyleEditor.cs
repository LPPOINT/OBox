using Assets.Scripts.Levels;
using Assets.Scripts.Levels.Style;
using UnityEditor;

namespace Assets.Editor.Levels.Style
{
    [CustomEditor(typeof(LevelPresetStyle))]
    public class PresetLevelStyleEditor : UnityEditor.Editor
    {

        private LevelStylePreset lastPreset;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var p = (LevelPresetStyle) target;

            if (p.Preset != lastPreset)
            {
                lastPreset = p.Preset;
                LevelFrontColor.SetColor(p.Preset.FrontColor);
            }


        }
    }
}
