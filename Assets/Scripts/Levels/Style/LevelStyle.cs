using UnityEngine;

namespace Assets.Scripts.Levels.Style
{
    public abstract class LevelStyle : MonoBehaviour, ILevelStyle
    {

        private const string MainStyleTag = "MainStyle";
        public static LevelStyle MainStyle { get; private set; }

        private void Awake()
        {
            if (gameObject.tag == MainStyleTag)
            {
                MainStyle = this;
            }
        }

        public LevelStylePreset CreatePreset()
        {
            var p = ScriptableObject.CreateInstance<LevelStylePreset>();

            p.BackgroundGradientColor1 = GetBackgroundGradientColor1();
            p.BackgroundGradientColor2 = GetBackgroundGradientColor2();
            p.FrontColor = GetFrontColor();

            return p;

        }

        public abstract Color GetBackgroundGradientColor1();
        public abstract Color GetBackgroundGradientColor2();
        public abstract Color GetFrontColor();
    }
}
