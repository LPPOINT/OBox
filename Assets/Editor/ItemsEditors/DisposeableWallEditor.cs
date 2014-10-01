using Assets.Scripts.Map.Items;
using UnityEditor;

namespace Assets.Editor.ItemsEditors
{

    [CustomEditor(typeof(DisposeableWall))]
    public class DisposeableWallEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var dw = (DisposeableWall) target;

            if (((int) dw.Time).ToString() != dw.TimeUI.text)
            {
                dw.TimeUI.text = ((int) dw.Time).ToString();
            }

        }
    }
}
