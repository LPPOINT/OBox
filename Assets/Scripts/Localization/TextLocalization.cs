using System.Linq;
using System.Net.Mime;
using SmartLocalization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Localization
{

    public class TextLocalization : MonoBehaviour
    {
        public string Key;
        public Text Text;

        private void Awake()
        {

            
        }

        private void Start()
        {
            if (Text == null) Text = GetComponent<Text>();
            if (string.IsNullOrEmpty(Key))
            {
                Debug.LogWarning("TextLocalization: wrong key passed");
            }

            ApplyText();
            LanguageManager.Instance.OnChangeLanguage += manager => ApplyText();

        }


        public void ApplyText()
        {
            if (Text == null)
            {
                Debug.LogWarning("ApplyText(): Text component not found");
                return;
            }

            Text.text = LanguageManager.Instance.GetTextValue(Key);
        }

        public static TextLocalization Inject(string key, Text text)
        {
            var l = text.gameObject.AddComponent<TextLocalization>();
            l.Key = key;
            l.Text = text;

            return l;
        }

    

    }
}
