using Assets.Scripts.LevelSelection;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.LevelSelection
{
    [CustomEditor(typeof(LevelIcon))]
    public class LevelIconEditor : UnityEditor.Editor
    {
        private CurvySpline spline;

        public void OnSceneGUI()
        {
            var icon = (LevelIcon) target;
            var oldPos = icon.transform.position;
            var newPos = Handles.PositionHandle(icon.transform.position, Quaternion.Euler(Vector3.zero));

            if (spline == null)
            {
                spline = icon.transform.parent.parent.GetComponentInChildren<CurvySpline>();
                if (spline == null)
                {
                    Debug.LogWarning("Spline not found");
                    return;
                }
            }

            float tf = spline.GetNearestPointTF(newPos);
            icon.transform.position = spline.Interpolate(tf);

            if (GUI.changed)
            {
                EditorUtility.SetDirty(icon);
            }

        }
    }
}
