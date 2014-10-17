using System;
using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts.Levels.Style.GradientBackground
{

    [ExecuteInEditMode]
    public class GradientBackground : MonoBehaviour
    {

        private static GradientBackground mainGradient;
        public static GradientBackground MainGradient
        {
            get
            {
                return
                    mainGradient ?? (mainGradient =
                        GameObject.FindGameObjectWithTag("MainGradient").GetComponent<GradientBackground>());
            }
        }

        public Material GradientMaterial;
        public Sprite GradientTexture;

        public IGradientColorProvider ColorProvider;
        public LevelStyle ProviderAsStyle;

        private SpriteRenderer spriteRenderer;

        public bool UseCustomProvider;
        public Color CustomColor1;
        public Color CustomColor2;

        public void AlignToBackAnchor()
        {
            if(LevelDepth.IsExist)
                LevelDepth.AlignToBack(transform);
        }
        public void AlignToFrontAnchor()
        {
            if (LevelDepth.IsExist)
                LevelDepth.AlignToFront(transform);
        }

        protected virtual void Start()
        {


            if (ProviderAsStyle != null)
            {
                ColorProvider = ProviderAsStyle;
            }

            if (UseCustomProvider)
            {
                ColorProvider = new GradientColorProvider(CustomColor1, CustomColor2);
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


            spriteRenderer.sharedMaterial.SetColor("_Color", ColorProvider.GetBackgroundGradientColor1());
            spriteRenderer.sharedMaterial.SetColor("_Color2", ColorProvider.GetBackgroundGradientColor2());
            spriteRenderer.transform.localScale = new Vector3(Math.Abs(spriteRenderer.transform.localScale.x),
                                                              Math.Abs(spriteRenderer.transform.localScale.y),
                                                              Math.Abs(spriteRenderer.transform.localScale.z));

        }


#if UNITY_EDITOR


        private Color lastColor1;
        private Color lastColor2;


        private void Update()
        {

            if (ColorProvider == null)
            {
                if (ProviderAsStyle != null)
                {
                    ColorProvider = ProviderAsStyle;
                    return;
                }
            }
            if (ColorProvider == null) return;

            if (UseCustomProvider &&
                (CustomColor1 != ColorProvider.GetBackgroundGradientColor1() ||
                 CustomColor2 != ColorProvider.GetBackgroundGradientColor2()))
            {
                ColorProvider = new GradientColorProvider(CustomColor1, CustomColor2);
            }

            if (lastColor1 != ColorProvider.GetBackgroundGradientColor1() || lastColor2 != ColorProvider.GetBackgroundGradientColor2())
            {
                lastColor1 = ColorProvider.GetBackgroundGradientColor1();
                lastColor2 = ColorProvider.GetBackgroundGradientColor2();


                spriteRenderer.sharedMaterial.SetColor("_Color", ColorProvider.GetBackgroundGradientColor1());
                spriteRenderer.sharedMaterial.SetColor("_Color2", ColorProvider.GetBackgroundGradientColor2());
            }
        }

#endif


    }
}
