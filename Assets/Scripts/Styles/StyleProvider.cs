using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Styles
{
    public abstract class StyleProvider : MonoBehaviour
    {

        public static StyleProvider Main { get; private set; }

        private void Awake()
        {
            if (gameObject.tag == "MainStyle")
            {
                Main = this;
            }
        }

        public abstract IStyle GetStyle();
    }
}
