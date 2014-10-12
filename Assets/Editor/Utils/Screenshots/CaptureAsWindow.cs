using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Utils
{
    public class CaptureAsWindow : EditorWindow
    {
        [MenuItem("Edit/Screenshot/Capture As")]
        private static void Init()
        {
            var w = CreateInstance<CaptureAsWindow>();

            w.title = "Capture As";
            w.minSize = new Vector2(300, 150);
            w.maxSize = new Vector2(350, 150);

            w.Show();
        }

        private string screenshotName = "Scr1";
        private bool export;

        private void OnGUI()
        {

            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Trying to capture screenshot in edit mode.", MessageType.Warning);
            }

            screenshotName = EditorGUILayout.TextField("Screenshot name:", screenshotName);
            export = EditorGUILayout.Toggle("Export to Yandex.Disk:", export);

            if (GUILayout.Button("Capture and save"))
            {
                if(export) ScreenshotUtils.DoCaptureAndExportScreenshot(screenshotName);
                else ScreenshotUtils.DoCaptureScreenshot(screenshotName);
            }
        }

    }
}
