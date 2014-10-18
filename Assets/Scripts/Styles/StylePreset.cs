using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Styles
{
    public class StylePreset : ScriptableObject
    {

        public const string StylePresetsFolder = "Styles";




        public List<StyleColorEntry> Entries;

        public static StylePreset Create(IStyle baseStyle)
        {
            var preset = CreateInstance<StylePreset>();
            preset.Entries = baseStyle.AllColors;
            return preset;
        }

        public void Save(string assetName)
        {
            Save(this, assetName);
        }

        public static void Save(StylePreset preset, string name)
        {
            AssetDatabase.CreateAsset(preset, "Assets/Resources/" + StylePresetsFolder + "/" + name + ".asset");
            AssetDatabase.Refresh();
        }

        public static StylePreset Load(string name)
        {
            return Resources.Load<StylePreset>(StylePresetsFolder + "/" + name);
        }

    }
}
