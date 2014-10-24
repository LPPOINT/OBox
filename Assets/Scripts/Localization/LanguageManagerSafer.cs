using UnityEngine;

namespace Assets.Scripts.Localization
{
    public class LanguageManagerSafer : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
