using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts.Camera.Effects
{
    public class CameraEffectOnLevelStart : LevelElement
    {

        public float HeightOffset = 3;
        public float DepthOffset = 3;
        public float Time = 0.3f;
        public iTween.EaseType EaseType = iTween.EaseType.easeInCirc;

        protected override void OnLevelInitialized()
        {
            PlayIntroduction();
        }

        protected override void OnLevelReset()
        {
            PlayIntroduction();
        }

        private void PlayIntroduction()
        {
            var cam = GetComponent<UnityEngine.Camera>();
            var camp = cam.transform.position;

            iTween.MoveFrom(cam.gameObject,
                iTween.Hash("position", new Vector3(camp.x, camp.y + HeightOffset, camp.z + DepthOffset), "time", Time,
                    "easetype", EaseType));
        }
    }
}
