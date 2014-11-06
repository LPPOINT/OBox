using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Styles.Masking
{
    [ExecuteInEditMode]
    public class Mask : MonoBehaviour
    {
        public IEnumerable<Maskable> GetMaskTargets()
        {
            return FindObjectsOfType<Maskable>().Where(maskable => maskable.Mask == this);
        }


        private Rect GetIntersects(Bounds r1, Bounds r2)
        {
            return new Rect();
        }

        private Rect GetIntersects(Renderer r1, Renderer r2)
        {


            return GetIntersects(r1.bounds, r2.bounds);
        }

        public void Visualize()
        {
            
        }

    }
}
