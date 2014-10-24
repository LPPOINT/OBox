using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
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

#if UNITY_EDITOR
        public void Save(string assetName)
        {
            Save(this, assetName);
        }


        public static void Save(StylePreset preset, string name)
        {
            AssetDatabase.CreateAsset(preset, "Assets/Resources/" + StylePresetsFolder + "/" + name + ".asset");
            AssetDatabase.Refresh();
        }

#endif

        public static StylePreset Load(string name)
        {
            return Resources.Load<StylePreset>(StylePresetsFolder + "/" + name);
        }

    }
}
