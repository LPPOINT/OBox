using Assets.Scripts.Levels.Style.ElementsColorization;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Levels.Style
{
    public static class LevelStyleUtils
    {

        public static IEnumerable<ColoredObject> GetAllColoredElements()
        {
            return Object.FindObjectsOfType<ColoredObject>();

        }

        public static LevelStyle MainStyle
        {
            get { return LevelStyle.MainStyle; }
        }

        public static GradientBackground.GradientBackground MainBackground
        {
            get { return GradientBackground.GradientBackground.MainGradient; }
        }

        public static void SetColor(Color color)
        {
            var coloredObjects = GetAllColoredElements();
            SetColor(coloredObjects, color);

        }

        public static void SetColor()
        {
            SetColor(MainStyle.GetFrontColor());

        }

        public static void SetColor(IEnumerable<ColoredObject> targets, Color color)
        {

            foreach (var coloredObject in targets)
            {
                coloredObject.Color = color;
#if UNITY_EDITOR
                var dirty = coloredObject.GetDirtyObjects();

                foreach (var obj in dirty)
                {
                    EditorUtility.SetDirty(obj);
                }
#endif
            }
        }

        public static void SetColor(GameObject target, Color color)
        {
            var objs = target.GetComponentsInChildren<ColoredObject>();
            SetColor(objs, color);
        }
    }
}
