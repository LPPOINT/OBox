using UnityEngine;

namespace Assets.Scripts.Levels
{
    public class LevelBackground : MonoBehaviour
    {

        public static LevelBackground Main { get; private set; }

        private void Awake()
        {
            Main = this;
        }

        public void AlignToBackAnchor()
        {
            if (LevelDepth.IsExist)
                LevelDepth.AlignToBack(transform);
        }
        public void AlignToFrontAnchor()
        {
            if (LevelDepth.IsExist)
                LevelDepth.AlignToFront(transform);
        }

    }
}
