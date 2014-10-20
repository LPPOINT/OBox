using Assets.Scripts.Styles;
using UnityEditor;

namespace Assets.Editor.Style
{
    [CustomEditor(typeof(StyleProvider), true)]
    public class StyleProviderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            StyleEditorUtils.ShowApplyStyleButtonFor((StyleProvider)target);
        }
    }
}
