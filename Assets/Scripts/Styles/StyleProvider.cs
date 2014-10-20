using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Styles
{
    public abstract class StyleProvider : MonoBehaviour
    {


        public abstract IStyle GetStyle();
    }
}
