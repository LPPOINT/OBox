using Assets.Scripts.Map.Items;
using UnityEditor;

namespace Assets.Editor
{
    [CustomEditor(typeof(Teleporter))]
    public class TeleporterEditor : UnityEditor.Editor
    {

        private Teleporter.TeleporterStyle lastStyle;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var tp = target as Teleporter;

            if (lastStyle != tp.Style)
            {
                lastStyle = tp.Style;
                tp.ApplyStyle();
            }

        }
    }
}
