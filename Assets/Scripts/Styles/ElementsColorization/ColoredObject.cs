using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Styles.ElementsColorization
{

    [ExecuteInEditMode]
    public abstract class ColoredObject : MonoBehaviour
    {


        public bool IncludeChildren = true;
        public bool IncludeSelf = true;
        public bool EnableAlphaMatching = true;
        public abstract IEnumerable<Object> GetDirtyObjects();

        public abstract Color Color { get; set; }
    }
}
