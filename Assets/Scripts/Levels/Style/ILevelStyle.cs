

using UnityEngine;

namespace Assets.Scripts.Levels.Style
{
    public interface ILevelStyle : IGradientColorProvider
    {

        Color GetFrontColor();
    }
}
