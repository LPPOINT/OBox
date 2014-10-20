using System;
using Assets.Scripts.Levels;
using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts.Styles.Gradient
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
        public StyleProvider ColorProvider;


        private SpriteRenderer spriteRenderer;


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

        public void Visualize()
        {
            if (ColorProvider == null)
            {
                var custom = gameObject.AddComponent<CustomStyleProvider>();
                ColorProvider = custom;
            }

            try
            {
                if (gameObject == null)
                {
                    Debug.LogWarning("GradientBackground.go == null. WTF????");
                    return;
                }
            }
            catch (Exception e)
            {
                return;
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


            spriteRenderer.sharedMaterial.SetColor("_Color", ColorProvider.GetStyle().GetBackgroundGradientColor1());
            spriteRenderer.sharedMaterial.SetColor("_Color2", ColorProvider.GetStyle().GetBackgroundGradientColor2());
            spriteRenderer.transform.localScale = new Vector3(Math.Abs(spriteRenderer.transform.localScale.x),
                                                              Math.Abs(spriteRenderer.transform.localScale.y),
                                                              Math.Abs(spriteRenderer.transform.localScale.z));
        }


#if UNITY_EDITOR


        private Color lastColor1;
        private Color lastColor2;


        private void Update()
        {

            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

            if (ColorProvider == null)
            {
                var custom = gameObject.AddComponent<CustomStyleProvider>();
                ColorProvider = custom;
            }



            if (lastColor1 != ColorProvider.GetStyle().GetBackgroundGradientColor1() || lastColor2 != ColorProvider.GetStyle().GetBackgroundGradientColor2())
            {
                lastColor1 = ColorProvider.GetStyle().GetBackgroundGradientColor1();
                lastColor2 = ColorProvider.GetStyle().GetBackgroundGradientColor2();

                var newColor1 = ColorProvider.GetStyle().GetBackgroundGradientColor1();
                var newColor2 = ColorProvider.GetStyle().GetBackgroundGradientColor2();


                spriteRenderer.sharedMaterial.SetColor("_Color", newColor1);
                spriteRenderer.sharedMaterial.SetColor("_Color2", newColor2);
            }
        }

#endif


    }
}
