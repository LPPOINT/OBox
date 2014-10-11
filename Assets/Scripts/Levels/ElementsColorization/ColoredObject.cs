using UnityEngine;

namespace Assets.Scripts.Levels.ElementsColorization
{
    public abstract class ColoredObject : MonoBehaviour
    {
        public abstract Color Color { get; set; }
    }
}
