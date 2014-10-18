using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Styles
{
    public class Style : IStyle
    {


        public Style()
        {
            Colors = new List<StyleColorEntry>();
        }

        public Style(List<StyleColorEntry> colors)
        {
            Colors = colors;
        }


        public List<StyleColorEntry>  Colors { get; private set; } 

        public Color GetColor(string name)
        {
            try
            {
                return Colors.FirstOrDefault(entry => entry.Name == name).Color;
            }
            catch
            {
                return Color.white;
            }
        }

        public List<StyleColorEntry> AllColors { get { return Colors; } }
    }
}
