using UnityEngine;

namespace Assets.Scripts.LevelSelection
{
    public class LSCameraController : MonoBehaviour
    {
        public float speed = 0.2f;
        public CurvySplineBase SplineToDetectBounds;

        private Bounds splineBounds;

        public float Height { get; private set; }

        private void Start()
        {
            if(SplineToDetectBounds != null)
                splineBounds = SplineToDetectBounds.GetBounds(false);
        }

        private void Update()
        {
            var maxLimitY = splineBounds.max.y;
            var minLimitY = splineBounds.min.y;

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                    gameObject.transform.position.y - (1*speed), gameObject.transform.position.z);
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                    gameObject.transform.position.y + (1*speed), gameObject.transform.position.z);
            }
           
        }


    }
}