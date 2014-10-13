using UnityEngine;

namespace Assets.Scripts.Levels
{
    public class LevelDepth : MonoBehaviour
    {

        public static LevelDepth Instance { get; private set; }

        public Transform BackTransform;
        public Transform FrontTransform;
        public Transform GamemapTransform;

        private static float GetDepth(Transform t)
        {
            return t.position.z;
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Multiple LevelDepth detected");
            }
            Instance = this;
        }

        public static float BackDepth { get { return GetDepth(Instance.BackTransform); } }
        public static float FrontDepth { get { return GetDepth(Instance.FrontTransform); } }
        public static float GamemapDepth { get { return GetDepth(Instance.GamemapTransform); } }

        private static void Align(Transform t, float z)
        {
            t.position = new Vector3(t.position.x, t.position.y, z);
        }

        public static void AlignToFront(Transform t)
        {
            Align(t, FrontDepth);
        }

        public static void AlignToBack(Transform t)
        {
            Align(t, BackDepth);
        }

        public static void AlignToGamemap(Transform t)
        {
            Align(t, GamemapDepth);
        }
        

    }
}
