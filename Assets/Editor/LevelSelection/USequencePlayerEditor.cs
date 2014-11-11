using Assets.Scripts.GameGUI.Pages.LevelSelection;
using Assets.Scripts.GameGUI.Pages.LevelSelection.USequencerIntegration;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.LevelSelection
{
    [CustomEditor(typeof(USequencerPlayer))]
    public class USequencePlayerEditor : UnityEditor.Editor
    {

        [SerializeField]
        public float PanelFallDownLenght = 9.8f;

        [SerializeField]
        public bool CameraFollow;

        private Camera cameraToFollow;

        private USequencerInlineUIDrawer drawer = new USequencerInlineUIDrawer();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PanelFallDownLenght = EditorGUILayout.FloatField("PanelFallDownLenght", PanelFallDownLenght);
            CameraFollow = EditorGUILayout.Toggle("Attach Camera", CameraFollow);

            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }



        public  void OnSceneGUI()
        {
            var s = (USequencerPlayer) target;

            drawer.Draw(s, PanelFallDownLenght);

            if (CameraFollow)
            {


                s.TargetCamera.transform.position = new Vector3(s.CalculateGlobalPosition(s.Sequencer.RunningTime), s.TargetCamera.transform.position.y, s.TargetCamera.transform.position.z);
            }

            if (GUI.changed)
                EditorUtility.SetDirty(target);

        }
    }
}
