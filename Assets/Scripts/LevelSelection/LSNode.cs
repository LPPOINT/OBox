using System;
using Assets.Scripts.Styles;
using UnityEngine;

namespace Assets.Scripts.LevelSelection
{
    public class LSNode : MonoBehaviour
    {

        public enum LSNodeSplineConnection
        {
            NoConnection,
            CreateNewSpline,
            MoveSpline
        }

        public LSNodeSplineConnection SplineConnection = LSNodeSplineConnection.MoveSpline;

        public LSTrigger PrevNodeLoadTrigger;
        public LSTrigger NextNodeLoadTrigger;

        public StyleProvider Style;

        public Transform BoundMin;
        public Transform BoundsMax;
        public UnityEngine.Camera EditorCamera;

        public CurvySplineBase Spline;

        public float Height
        {
            get { return Math.Abs(BoundsMax.transform.position.y - BoundMin.transform.position.y); }
        }


        private LSNodesSupervisor supervisor;

        private bool isInitialized;

        private void Update()
        {
            if (!isInitialized)
            {
                supervisor = FindObjectOfType<LSNodesSupervisor>();

                if (supervisor != null) // in LSCore level
                {
                    Debug.Log("Initialized");
                    supervisor.RegisterNode(this);
                    Destroy(EditorCamera.gameObject);
                    isInitialized = true;
                }

            }
        }

        private void Start()
        {

        }

    }
}
