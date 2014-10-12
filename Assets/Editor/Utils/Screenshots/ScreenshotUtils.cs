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
        public static IEnumerable DoCaptureScreenshot()
        {
            // We should only read the screen after all rendering is complete
            yield return new WaitForEndOfFrame();

            // Create a texture the size of the screen, RGB24 format
            var width = Screen.width;
            var height = Screen.height;
            var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
            // Read screen contents into the texture
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();

            // Encode texture into PNG
            var bytes = tex.EncodeToPNG();

            using (var f = File.Create(@"C:\Users\Sasha\Documents\OBox\Assets\Editor\Screenshots\Scr.png"))
            {
                f.Write(bytes, 0, bytes.Length);
            }

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
