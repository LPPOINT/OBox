using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Assets.Editor.Utils.Screenshots;
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

        [MenuItem("Edit/Screenshot/Capture")]
        public static void DoCaptureScreenshot()
        {
            Application.CaptureScreenshot(
                Application.dataPath + "/Editor/Screenshots/" + GetTimestamp(DateTime.Now) + ".png", 2);
        }

        public static void DoCaptureScreenshot(string name)
        {
            Application.CaptureScreenshot(Application.dataPath + "/Editor/Screenshots/" + name + ".png", 2);
        }

        [MenuItem("Edit/Screenshot/Capture and export")]
        public static void DoCaptureAndExportScreenshot()
        {
            var name = GetTimestamp(DateTime.Now);
            DoCaptureScreenshot(name);
            ScreenshotExporter.Export(name);
        }

        public static void DoCaptureAndExportScreenshot(string name)
        {
            DoCaptureScreenshot(name);
            ScreenshotExporter.Export(name);
        }

        [MenuItem("Edit/Screenshot/Open folder")]
        public static void OpenScreenshotsFolder()
        {
            Process.Start(@"C:\Users\Sasha\Documents\OBox\Assets\Editor\Screenshots\");
        }
    }
}
