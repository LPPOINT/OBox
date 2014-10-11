using UnityEngine;

namespace Assets.Scripts.Levels.ElementsColorization
{
    public class ParticleSystemColored : ColoredObject
    {
        public override Color Color { get { return GetComponent<ParticleSystem>().startColor; } set
        {
            GetComponent<ParticleSystem>().startColor = value;
        } }
    }
}
