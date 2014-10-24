using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameGUI
{
    public class GUISupervisor : MonoBehaviour
    {

        public static GUISupervisor Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            ActivePage = FindObjectOfType<GUIPage>();     
        }


        public GUIPage[] PagesPrefabs;

        public GUITranslation ActiveTranslation { get; private set; }
        public GUIPage ActivePage { get; private set; }

        public GUIPage InstantiatePage(GUIPage prefab)
        {
            var r = Instantiate(prefab);
            r.GetComponent<Canvas>().worldCamera = UnityEngine.Camera.main;
            return r;

        }

        public void OpenPage(GUIPage newPagePrefab)
        {
            var translation = ActivePage.GetCustomTranslationTo(newPagePrefab, newPagePrefab.Type) ??
                              GUITranslation.CreateDefaultTranslation();
            OpenPage(newPagePrefab, translation);
        }

        public void OpenPage(GUIPage newPagePrefab, GUITranslation translation)
        {
            ActiveTranslation = translation;
            ActiveTranslation.Done += OnTranslationDone;
            ActiveTranslation.Activate(ActivePage, newPagePrefab);
        }

        public void OpenPage(GUIPageType newpageType, GUITranslation translation)
        {
            var page = PagesPrefabs.FirstOrDefault(guiPage => guiPage.Type == newpageType);
            if (page != null)
            {
                OpenPage(page, translation);
            }
            else
            {
                Debug.LogWarning("OpenPage(): Page '" + newpageType + "' not found in pages prefabs");
            }
        }

        public void OpenPage(GUIPageType newpageType)
        {
            var page = PagesPrefabs.FirstOrDefault(guiPage => guiPage.Type == newpageType);

            if (page == null)
            {
                Debug.LogWarning("OpenPage(): Page '" + newpageType + "' not found in pages prefabs");
                return;
            }

            var translation = ActivePage.GetCustomTranslationTo(page, page.Type) ??
                  GUITranslation.CreateDefaultTranslation();
            OpenPage(page, translation);
        }

        private void OnTranslationDone(object sender, EventArgs eventArgs)
        {
            ActiveTranslation.Done -= OnTranslationDone;
            Destroy(ActivePage.gameObject);
            ActivePage = InstantiatePage(ActiveTranslation.To);
            Destroy(ActiveTranslation.gameObject);
            ActiveTranslation = null;
        }

    }
}
