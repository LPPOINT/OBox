using System;
using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts.Levels.Style.GradientBackground
{

    [ExecuteInEditMode]
    public class GradientBackground : MonoBehaviour
    {

        public Material GradientMaterial;
        public Sprite GradientTexture;

        public LevelStyle LevelStyle;

        private SpriteRenderer spriteRenderer;

        protected virtual void Start()
        {


            if (LevelStyle == null)
            {
                LevelStyle = FindObjectOfType<LevelStyle>();
            }

            var thisCam = UnityEngine.Camera.main;
            var farClip = thisCam.farClipPlane;


            var topLeftPosition = thisCam.ViewportToWorldPoint(new Vector3(0, 1, farClip));
            var topRightPosition = thisCam.ViewportToWorldPoint(new Vector3(1, 1, farClip));
            var btmRightPosition = thisCam.ViewportToWorldPoint(new Vector3(1, 0, farClip));

            var w = topRightPosition.x - topLeftPosition.x;
            var h = btmRightPosition.y - topRightPosition.y;

            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

            if (spriteRenderer == null) spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

            spriteRenderer.sharedMaterial = GradientMaterial;
            spriteRenderer.sprite = GradientTexture;
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
            spriteRenderer.transform.position = new Vector3(thisCam.transform.position.x, thisCam.transform.position.y, spriteRenderer.transform.position.z);

            TileSizeUtils.SetScaleBySize(gameObject, spriteRenderer.bounds, w, h);

            if (LevelStyle == null) return;


            spriteRenderer.sharedMaterial.SetColor("_Color", LevelStyle.GetBackgroundGradientColor1());
            spriteRenderer.sharedMaterial.SetColor("_Color2", LevelStyle.GetBackgroundGradientColor2());
            spriteRenderer.transform.localScale = new Vector3(Math.Abs(spriteRenderer.transform.localScale.x),
                                                              Math.Abs(spriteRenderer.transform.localScale.y),
                                                              Math.Abs(spriteRenderer.transform.localScale.z));

        }

#if UNITY_EDITOR


        private Color lastColor1;
        private Color lastColor2;

        private void Update()
        {

            if(LevelStyle == null) return;

            if (lastColor1 != LevelStyle.GetBackgroundGradientColor1() || lastColor2 != LevelStyle.GetBackgroundGradientColor2())
            {
                lastColor1 = LevelStyle.GetBackgroundGradientColor1();
                lastColor2 = LevelStyle.GetBackgroundGradientColor2();


                spriteRenderer.sharedMaterial.SetColor("_Color", LevelStyle.GetBackgroundGradientColor1());
                spriteRenderer.sharedMaterial.SetColor("_Color2", LevelStyle.GetBackgroundGradientColor2());
            }
        }

#endif


    }
}
