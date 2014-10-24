using System;
using System.Linq;
using Assets.Scripts.Levels;
using Assets.Scripts.Model;
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
            EstablishPage(ActivePage);
        }


        public GUIPage[] PagesPrefabs;

        public GUITranslation ActiveTranslation { get; private set; }
        public GUIPage ActivePage { get; private set; }

        private void EstablishPage(GUIPage pageInstance)
        {
            var canvas = pageInstance.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.renderMode = RenderMode.OverlayCamera;
                canvas.worldCamera = UnityEngine.Camera.main;
            }
        }
        public GUIPage InstantiatePage(GUIPage prefab)
        {
            var r = Instantiate(prefab);
            EstablishPage(r);
            return r;

        }

        private Action<GUIPage> newPageInitialization; 

        public void OpenPage(GUIPage newPagePrefab, Action<GUIPage> initialization = null)
        {
            var translation = ActivePage.GetCustomTranslationTo(newPagePrefab, newPagePrefab.Type) ??
                              GUITranslation.CreateDefaultTranslation();
            OpenPage(newPagePrefab, translation, initialization);
        }
        public void OpenPage(GUIPage newPagePrefab, GUITranslation translation, Action<GUIPage> initialization = null)
        {
            newPageInitialization = initialization;
            ActiveTranslation = translation;
            ActiveTranslation.Done += OnTranslationDone;
            ActiveTranslation.Activate(ActivePage, newPagePrefab);
        }

        public void OpenPage(GUIPageType newpageType, GUITranslation translation, Action<GUIPage> initialization = null)
        {
            var page = PagesPrefabs.FirstOrDefault(guiPage => guiPage.Type == newpageType);
            if (page != null)
            {
                OpenPage(page, translation, initialization);
            }
            else
            {
                Debug.LogWarning("OpenPage(): Page '" + newpageType + "' not found in pages prefabs");
            }
        }
        public void OpenPage(GUIPageType newpageType, Action<GUIPage> initialization = null)
        {
            var page = PagesPrefabs.FirstOrDefault(guiPage => guiPage.Type == newpageType);

            if (page == null)
            {
                Debug.LogWarning("OpenPage(): Page '" + newpageType + "' not found in pages prefabs");
                return;
            }

            var translation = ActivePage.GetCustomTranslationTo(page, page.Type) ??
                  GUITranslation.CreateDefaultTranslation();
            OpenPage(page, translation, initialization);
        }

        public void OpenLevel(string levelPath)
        {
            OpenLevel(levelPath, GUITranslation.CreateDefaultTranslation());
        }
        public void OpenLevel(string levelPath, GUITranslation translation)
        {
            var levelPagePrefab = PagesPrefabs.FirstOrDefault(page => page.Type == GUIPageType.LevelLoading);
            if (levelPagePrefab == null)
            {
                Debug.LogWarning("OpenLevel(): level page prefab not found");
                return;
            }

            

            OpenPage(levelPagePrefab, translation, page => (page as GUILevelLoadingPage).LevelPath = levelPath);

        }
        public void OpenLevel(int levelNumber, WorldNumber worldNumber, bool openBuildLevel = false)
        {
            OpenLevel(levelNumber, worldNumber, GUITranslation.CreateDefaultTranslation(), openBuildLevel);
        }
        public void OpenLevel(int levelNumber, WorldNumber worldNumber, GUITranslation translation, bool openBuildLevel = false)
        {
            var levelPath = LevelName.GetScenePath(levelNumber, worldNumber, false, openBuildLevel);
            OpenLevel(levelPath, translation);
        }

        private void OnTranslationDone(object sender, EventArgs eventArgs)
        {
            ActiveTranslation.Done -= OnTranslationDone;
            ActivePage.OnClose();
            Destroy(ActivePage.gameObject);
            ActivePage = InstantiatePage(ActiveTranslation.To);
            if (newPageInitialization != null)
            {
                newPageInitialization(ActivePage);
            }
            ActivePage.OnShow();
            Destroy(ActiveTranslation.gameObject);
            ActiveTranslation = null;
        }



    }
}
