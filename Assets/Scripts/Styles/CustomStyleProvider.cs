using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Styles
{
    public class CustomStyleProvider : StyleProvider
    {

        [HideInInspector][SerializeField]public List<StyleColorEntry> Colors; 

        public override IStyle GetStyle()
        {
            return new Style(Colors);
        }
    }
}
