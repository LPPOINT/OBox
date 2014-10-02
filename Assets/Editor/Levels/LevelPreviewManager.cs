using Assets.Scripts.Levels;
using Assets.Scripts.Meta.Model;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Levels
{
    public class LevelPreviewManager : ScriptableObject
    {

        public static void UpdateCurrentLevelPreview()
        {
            var index = FindObjectOfType<Level>().Index;

            var path = GetLevelPreviewPath(index.LevelNumber, index.WorldNumber);
            Application.CaptureScreenshot(path);
        }


        public static string GetLevelPreviewPath(int levelNumber, WorldNumber worldNumber, bool rel = false)
        {
            if (!rel)
            {
                return Application.dataPath + @"/Editor/Levels/PreviewIcons/World" + (int) worldNumber + "/Level" +
                       levelNumber + ".png";
            }
            return "Assets/Editor/Levels/PreviewIcons/World" + (int)worldNumber + "/Level" +
                       levelNumber + ".png";
        }


    }
}
