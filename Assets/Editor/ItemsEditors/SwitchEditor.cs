using Assets.Scripts.Map.Items;
using UnityEditor;

namespace Assets.Editor.ItemsEditors
{
    [CustomEditor(typeof(Switch))]
    public class SwitchEditor : UnityEditor.Editor
    {
        private Switch.SwitchPosition lastPosition;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var sw = target as Switch;
            if (lastPosition != sw.StartPosition)
            {
                lastPosition = sw.StartPosition;
                sw.ApplyStartPosition();
            }
        }
    }
}
