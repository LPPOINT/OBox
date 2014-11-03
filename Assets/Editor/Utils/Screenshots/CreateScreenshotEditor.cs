using System.Diagnostics;
using Assets.Scripts.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Editor.Utils.Screenshots
{
    [CustomEditor(typeof(CreateScreenshot))]
    public class CreateScreenshotEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Create"))
            {
                var scr = (CreateScreenshot) target;
                scr.Create();
                Process.Start(@"C:\Users\Sasha\Documents\OBox\Assets\Editor\Screenshots\");
            }
        }
    }
}
