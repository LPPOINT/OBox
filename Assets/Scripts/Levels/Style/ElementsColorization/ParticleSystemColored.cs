using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Levels.Style.ElementsColorization
{
    public class ParticleSystemColored : ColoredObject
    {
        public override IEnumerable<Object> GetDirtyObjects()
        {
            return new List<Object>() {GetComponent<ParticleSystem>()};
        }

        public override Color Color { get { return GetComponent<ParticleSystem>().startColor; } set
        {
            GetComponent<ParticleSystem>().startColor = value;
        } }
    }
}
