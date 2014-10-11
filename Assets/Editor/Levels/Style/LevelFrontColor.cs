using System.Collections.Generic;
using Assets.Scripts.Levels.ElementsColorization;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Levels.Style
{
    public static class LevelFrontColor
    {

        public static Color CurrentColor { get; private set; }

        public static IEnumerable<ColoredObject> GetAllColoredElements()
        {
            return Object.FindObjectsOfType<ColoredObject>();

        }

        public static void SetColor(Color color)
        {
            var coloredObjects = GetAllColoredElements();

            CurrentColor = color;

            foreach (var coloredObject in coloredObjects)
            {
                coloredObject.Color = color;
                EditorUtility.SetDirty(coloredObject.gameObject);
                if (coloredObject.GetComponent<SpriteRenderer>() != null)
                {
                    EditorUtility.SetDirty(coloredObject.GetComponent<SpriteRenderer>());
                }
            }

        }
    }
}
