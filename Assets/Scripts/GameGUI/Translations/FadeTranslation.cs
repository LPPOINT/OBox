using UnityEngine;

namespace Assets.Scripts.GameGUI.Translations
{
    public class FadeTranslation : GUITranslation
    {

        public Color? CustomColor = null;
        public float Time = 0.3f;
        public float Amount = 1;


        private void OnITweenFadeDone()
        {
            iTween.CameraFadeDestroy();
            OnDone();
        }

        protected override void OnActivated()
        {
            if (!CustomColor.HasValue)
            {
                CustomColor = UnityEngine.Camera.main.backgroundColor;
            }

            var fadeTexture = iTween.CameraTexture(CustomColor.Value);

            iTween.CameraFadeAdd(fadeTexture);
            iTween.CameraFadeTo(iTween.Hash("amount", Amount, "time", Time, "oncomplete", "OnITweenFadeDone", "oncompletetarget", gameObject));

        }
    }
}
