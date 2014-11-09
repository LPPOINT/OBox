using Assets.Scripts.GameGUI.Pages.LevelSelection;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.LevelSelection
{
    [CustomEditor(typeof(HOTTweenActivity))]
    public class HOTTweenActivityEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Establish duration"))
            {
                var a = target as HOTTweenActivity;
                var t = a.GetComponent<HOTweenComponent>();

                foreach (var gt in t.generatedTweeners)
                {
                    
                }
            }
        }
    }
}
