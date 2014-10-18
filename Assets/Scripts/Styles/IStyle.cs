using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Styles
{
    public interface IStyle 
    {
        Color GetColor(string name);

        List<StyleColorEntry> AllColors { get; }

    }
}
