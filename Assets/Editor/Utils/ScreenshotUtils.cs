using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Utils
{
    public class ScreenshotUtils : ScriptableObject
    {

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        [MenuItem("Edit/Capture Screenshot")]
        public static void DoCaptureScreenshot()
        {
            Application.CaptureScreenshot(Application.dataPath + "/Editor/Screenshots/" + GetTimestamp(DateTime.Now) + ".png", 2);
        }
    }
}
