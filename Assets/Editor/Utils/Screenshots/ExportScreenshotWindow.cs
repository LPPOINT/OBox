using UnityEditor;

namespace Assets.Editor.Utils.Screenshots
{
    public class ExportScreenshotWindow : EditorWindow
    {

        [MenuItem("Edit/Screenshot/Export")]
        private static void Init()
        {
            var w = CreateInstance<ExportScreenshotWindow>();
            w.Show();
        }
    }
}
