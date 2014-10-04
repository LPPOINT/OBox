﻿namespace Assets.Scripts.Map.Decorations
{
    public class ScaleToDecoration : Decoration
    {
        public float MinScale = 0f;
        public float Time = 0.3f;


        private void OnITweenAnimationDone()
        {
            OnDecorationEnd();
        }

        protected override void OnDecorationStart()
        {

            iTween.ScaleTo(gameObject, iTween.Hash(

                "time", Time,
                "oncomplete", "OnITweenAnimationDone",
                "scale", transform.localScale * MinScale,
                "includechildren", true

                ));

        }
    }
}
