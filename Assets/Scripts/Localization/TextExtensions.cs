using UnityEngine.UI;

namespace Assets.Scripts.Localization
{
    public static class TextExtensions
    {
        public static TextLocalization InjectLocalization(this Text text, string key)
        {
            return TextLocalization.Inject(key, text);
        }
    }
}
