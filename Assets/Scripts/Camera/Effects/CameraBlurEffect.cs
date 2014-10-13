using UnityEngine;

namespace Assets.Scripts.Camera.Effects
{
    public class CameraBlurEffect : MonoBehaviour
    {


        public static CameraBlurEffect Instance;


        private void Awake()
        {
            Instance = this;
        }

        private Blur blur;

        private bool destroyAfterDone;

        public bool IsPlaying { get; private set; }

        private void PrepareBlur()
        {

            if (blur == null)
                blur = gameObject.AddComponent<Blur>();

            blur.blurShader = Shader.Find("Hidden/FastBlur");
            blur.downsample = 1;
            blur.blurIterations = 1;
            blur.blurSize = 0;
        }

        private void OnITweenBlurUpdate(float newValue)
        {
            blur.blurSize = newValue;
        }

        private void OnITweenBlurDone()
        {
            IsPlaying = false;
            if (destroyAfterDone && blur != null)
            {
                Destroy(blur);
                blur = null;
            }
        }

        private void Blur(float blurFrom, float blurTo, float blurTime, iTween.EaseType blurEaseType, bool shouldDestroyAfterDone = false)
        {
            PrepareBlur();
            destroyAfterDone = shouldDestroyAfterDone;

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
            Instance.Blur(0, 10, 0.5f, iTween.EaseType.easeInOutCirc, false);
        }

        public static void BlurOut()
        {
            if (Instance == null) return;
            Instance.Blur(10, 0, 0.5f, iTween.EaseType.easeInOutCirc, true);
        }
    }
}
