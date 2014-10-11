using System;
using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts.Levels.GradientBackground
{

    [ExecuteInEditMode]
    public class GradientBackground : MonoBehaviour
    {

        public Material GradientMaterial;
        public Sprite GradientTexture;

        public Color Color1;
        public Color Color2;

        public GradientPreset CreatePreset()
        {
            var preset = ScriptableObject.CreateInstance<GradientPreset>();
            preset.Color1 = Color1;
            preset.Color2 = Color2;
            return preset;
        }

        private SpriteRenderer spriteRenderer;

        protected virtual void Start()
        {

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

            spriteRenderer.sharedMaterial.SetColor("_Color", Color1);
            spriteRenderer.sharedMaterial.SetColor("_Color2", Color2);
            spriteRenderer.transform.localScale = new Vector3(Math.Abs(spriteRenderer.transform.localScale.x),
                                                              Math.Abs(spriteRenderer.transform.localScale.y),
                                                              Math.Abs(spriteRenderer.transform.localScale.z));

        }

#if UNITY_EDITOR


        private Color lastColor1;
        private Color lastColor2;

        private void Update()
        {


            if (lastColor1 != Color1 || lastColor2 != Color2)
            {
                lastColor1 = Color1;
                lastColor2 = Color2;


                spriteRenderer.sharedMaterial.SetColor("_Color", Color1);
                spriteRenderer.sharedMaterial.SetColor("_Color2", Color2);
            }
        }

#endif


    }
}
