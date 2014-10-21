using SmartLocalization;

namespace Assets.Scripts.Localization
{
    public static class LocalizationUtils
    {

        public static string CreateTextNotFoundNotification(string key, object context = null)
        {
            var l = LanguageManager.Instance.LoadedLanguage;
            return string.Format("[{0}: key '{1}' not found({2})]", l, key,
                context != null ? context.GetType().Name : string.Empty);
        }
    }
}
