using UnityEngine;

namespace Assets.Scripts.Map.Decorations
{
    public class ScaleFromDecoration : Decoration
    {
        public float MinScale = 0f;
        public float Time = 0.3f;


        public override bool RefreshTileIndexesAfterDone
        {
            get { return false; }
        }

        private void OnITweenAnimationDone()
        {
            OnDecorationEnd();
        }

        protected override void OnDecorationStart()
        {
            base.OnDecorationStart();

            iTween.ScaleFrom(gameObject, iTween.Hash(

                "time", Time,
                "oncomplete", "OnITweenAnimationDone",
                "scale", transform.localScale * MinScale,
                "easetype", iTween.EaseType.linear

                ));

        }
    }
}
