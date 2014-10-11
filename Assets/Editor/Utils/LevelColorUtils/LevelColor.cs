using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Levels.ElementsColorization;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Utils.LevelColorUtils
{
    public static class LevelColor
    {

        public static IEnumerable<ColoredObject> GetAllColoredElements()
        {
            return Object.FindObjectsOfType<ColoredObject>();

        }

        public static void SetColor(Color color)
        {
            var coloredObjects = GetAllColoredElements();

            foreach (var coloredObject in coloredObjects)
            {
                coloredObject.Color = color;
                EditorUtility.SetDirty(coloredObject.gameObject);
            }

        }
    }
}
