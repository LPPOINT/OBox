using UnityEngine;

namespace Assets.Scripts.Map.Decorations
{
    public class FadeToDecoration : Decoration
    {
        public float MinAlpha = 0.5f;
        public float Time = 0.3f;


        private void ApplyAlpha(float alpha)
        {
            var renderers = GetComponentsInChildren<SpriteRenderer>();

            foreach (var spriteRenderer in renderers)
            {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            }
        }

        private void OnITweenAnimationDone()
        {
            OnDecorationEnd();
        }

        protected override void OnDecorationStart()
        {

            iTween.FadeFrom(gameObject, iTween.Hash(

                "time", Time,
                "oncomplete", "OnITweenAnimationDone",
                "alpha", MinAlpha,
                "includechildren", true

                ));

        }
    }
}
