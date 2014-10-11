using UnityEngine;

namespace Assets.Scripts.Levels.ElementsColorization
{
    public class RendererColored : ColoredObject
    {
        public override Color Color
        {
            get { return GetComponent<SpriteRenderer>().color; }
            set { GetComponent<SpriteRenderer>().color = value; }
        }
    }
}
