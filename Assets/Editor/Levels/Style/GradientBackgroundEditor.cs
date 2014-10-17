using Assets.Scripts.Levels.Style;
using Assets.Scripts.Levels.Style.GradientBackground;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Levels.Style
{
    [CustomEditor(typeof(GradientBackground))]
    public class GradientBackgroundEditor : UnityEditor.Editor
    {


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var gb = (GradientBackground) target;

            //if (gb.UseCustomProvider)
            //{

            //    var old1 = gb.CustomColor1;
            //    var old2 = gb.CustomColor2;

            //    gb.CustomColor1 = EditorGUILayout.ColorField("Color1", gb.CustomColor1);
            //    gb.CustomColor1 = EditorGUILayout.ColorField("Color2", gb.CustomColor2);

            //    if (old1 != gb.CustomColor1 || old2 != gb.CustomColor2)
            //    {
            //        EditorUtility.SetDirty(gb);
            //    }

            //}


        }
    }
}
