using System;
using UnityEngine;

namespace Assets.Scripts.Levels.Style
{

    public interface IGradientColorProvider
    {
        Color GetBackgroundGradientColor1();
        Color GetBackgroundGradientColor2();
    }
}
