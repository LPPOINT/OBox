using System;
using UnityEngine;

namespace Assets.Scripts.Camera.Effects
{
    public class CameraBlurEffect : MonoBehaviour
    {


        public static CameraBlurEffect Instance;

        public float FullBlurInTime = 0.5f;
        public float FullBlurOutTime = 0.3f;
        public iTween.EaseType FullBlurInEaseType = iTween.EaseType.easeInOutCirc;
        public iTween.EaseType FullBlurOutEaseType = iTween.EaseType.easeInOutCirc;


        public float FullBlurMaxSize = 9;
        public float PartialBlurSizeOffset = 3;

        public float PartialBlurInTime = 1.5f;
        public float PartialBlurOutTime = 1.5f;
        public iTween.EaseType PartialBlurInEaseType = iTween.EaseType.linear;
        public iTween.EaseType PartialBlurOutEaseType = iTween.EaseType.linear;


        private void Awake()
        {
            Instance = this;
        }

        private Blur blur;

        public bool IsPlaying { get; private set; }
        public bool IsLooped { get; private set; }

        private float maxBlurValue;
        private float currentTo;
        private iTween.EaseType currentEaseType;

        private void PrepareBlur()
        {

            if (blur == null)
                blur = gameObject.AddComponent<Blur>();

            blur.blurShader = Shader.Find("Hidden/FastBlur");
            blur.downsample = 1;
            blur.blurIterations = 1;
        }

        private void OnITweenBlurUpdate(float newValue)
        {
            blur.blurSize = newValue;
        }

        private void OnITweenBlurDone()
        {
            if (!IsLooped && blur != null)
            {
                IsPlaying = false;
                Destroy(blur);
                blur = null;
            }
            else if (IsLooped)
            {

                if (maxBlurValue < currentTo) maxBlurValue = currentTo;

                if (Math.Abs(currentTo - maxBlurValue) < 0.001f)
                {
                    Blur(maxBlurValue, maxBlurValue - Instance.PartialBlurSizeOffset, Instance.PartialBlurOutTime, Instance.PartialBlurInEaseType, IsLooped);
                }
                else
                {
                    Blur(maxBlurValue - Instance.PartialBlurSizeOffset, maxBlurValue, Instance.PartialBlurInTime, Instance.PartialBlurOutEaseType, IsLooped);
                }

            }
        }

        private void Blur(float blurFrom, float blurTo, float blurTime, iTween.EaseType blurEaseType, bool looped = false)
        {
            PrepareBlur();
            IsLooped = looped;

            currentTo = blurTo;
            currentEaseType = blurEaseType;

            if (IsPlaying)
            {
                iTween.Stop(gameObject);
            }

            IsPlaying = true;
            iTween.ValueTo(gameObject, iTween.Hash("onupdate", "OnITweenBlurUpdate",
                "from", blurFrom,
                "to", blurTo,
                "time", blurTime,
                "easetype", blurEaseType,
                "oncomplete", "OnITweenBlurDone",
                "oncompletetarget", gameObject));
        }

        public static void BlurIn()
        {
            if(Instance == null) return;
            Instance.Blur(0, Instance.FullBlurMaxSize, Instance.FullBlurInTime, Instance.FullBlurInEaseType, true);
        }

        public static void BlurOut()
        {
            if (Instance == null) return;
            Instance.Blur(Instance.FullBlurMaxSize, 0, Instance.FullBlurOutTime, Instance.FullBlurOutEaseType, false);
        }
    }
}
