using UnityEngine;

namespace Assets.Scripts.Levels.ElementsColorization
{

    [ExecuteInEditMode]
    public abstract class ColoredObject : MonoBehaviour
    {


        public abstract Color Color { get; set; }
    }
}
