using UnityEngine;

namespace Assets.Scripts.Levels.GradientBackground
{
    public class RandomBackround : GradientBackground
    {
        public Color From1;
        public Color To1;

        public Color From2;
        public Color To2;

        protected override void Start()
        {

            Color1 = new Color(
                        Random.Range(From1.r, To1.r),
                        Random.Range(From1.g, To1.g),
                        Random.Range(From1.b, To1.b)
                );


            Color2 = new Color(
                        Random.Range(From2.r, To2.r),
                        Random.Range(From2.g, To2.g),
                        Random.Range(From2.b, To2.b)
                );

            base.Start();
        }
    }
}
