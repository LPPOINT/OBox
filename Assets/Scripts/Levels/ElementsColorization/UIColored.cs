using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Levels.ElementsColorization
{
    public class UIColored : ColoredObject
    {
        public override Color Color
        {
            get { return GetComponent<Graphic>().color; }
            set { GetComponent<Graphic>().color = value; }
        }
    }
}
