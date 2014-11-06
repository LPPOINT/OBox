using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Styles.Masking
{
    public class Maskable : MonoBehaviour
    {
        public Mask Mask;

        private Renderer cachedRenderer;
        public Renderer Renderer
        {
            get { return cachedRenderer ?? (cachedRenderer = GetComponent<Renderer>()); }
        }

    }
}
