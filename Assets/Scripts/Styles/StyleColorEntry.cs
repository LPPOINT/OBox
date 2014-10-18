using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Styles
{

    [Serializable]
    public class StyleColorEntry
    {
        public StyleColorEntry(Color color, string name)
        {
            Color = color;
            Name = name;
        }
        public StyleColorEntry()
        {

        }

        public Color Color;
        public string Name;
    }


}
