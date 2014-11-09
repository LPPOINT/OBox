using System;
using System.Linq;
using Assets.Scripts.GameGUI.Pages;
using Assets.Scripts.GameGUI.Translations;
using Assets.Scripts.Levels;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Numeration;
using Assets.Scripts.Styles.Background;
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
        protected virtual void Start()
        {
            ActivePage = FindObjectOfType<GUIPage>();     
            EstablishPage(ActivePage);
        }

        public void EstablishBackground(SceneBackgroundBuilder backgroundBuilder, bool hasRestoreOpportunity = false)
        {
            var bg = FindObjectOfType<SceneBackground>();

            if (hasRestoreOpportunity)
            {
                bg.Store();
            }

            if (bg != null)
            {
                backgroundBuilder.Build(bg);
            }
        }


        public GUIPage[] PagesPrefabs;

        public GUITranslation ActiveTranslation { get; private set; }
        public GUIPage ActivePage { get; private set; }

        private void EstablishPage(GUIPage pageInstance)
        {
            if (pageInstance == null || pageInstance.GetComponent<Canvas>() == null)
            {
                return;
            }
            var canvas = pageInstance.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.renderMode = RenderMode.OverlayCamera;
                canvas.worldCamera = UnityEngine.Camera.main;
            }
        }
        public GUIPage InstantiatePage(GUIPage prefab)
        {
            var r = (Instantiate(prefab.gameObject) as GameObject).GetComponent<GUIPage>();
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
            OpenSceneWithLoading(levelPath, GUITranslation.CreateDefaultTranslation());
        }
        public void OpenSceneWithLoading(string scenePath, GUITranslation translation)
        {
            var levelPagePrefab = PagesPrefabs.FirstOrDefault(page => page.Type == GUIPageType.LevelLoading);
            if (levelPagePrefab == null)
            {
                Debug.LogWarning("OpenLevel(): level page prefab not found");
                return;
            }

            

            OpenPage(levelPagePrefab, translation, page => (page as GUILevelLoadingPage).LevelPath = scenePath);

        }
        public void OpenLevel(int levelNumber, WorldNumber worldNumber, bool openBuildLevel = false)
        {
            OpenLevel(levelNumber, worldNumber, GUITranslation.CreateDefaultTranslation(), openBuildLevel);
        }
        public void OpenLevel(int levelNumber, WorldNumber worldNumber, GUITranslation translation, bool openBuildLevel = false)
        {
            var levelPath = LevelIndex.GetScenePath(levelNumber, worldNumber, false, openBuildLevel);
            OpenSceneWithLoading(levelPath, translation);
        }

        private void OnTranslationDone(object sender, EventArgs eventArgs)
        {
            try
            {
                ActiveTranslation.Done -= OnTranslationDone;
                if (ActivePage != null)
                {
                    ActivePage.OnClose();
                    Destroy(ActivePage.gameObject);
                }

                if (ActiveTranslation.To == null)
                {
                    Debug.LogError("OnTranslationDone() ActiveTranslation.To == null");
                    return;
                }

                ActivePage = InstantiatePage(ActiveTranslation.To);
                if (newPageInitialization != null)
                {
                    newPageInitialization(ActivePage);
                }
                ActivePage.OnShow();
                Destroy(ActiveTranslation.gameObject);
                ActiveTranslation = null;
            }
            catch (Exception e)
            {
                Debug.LogError("OnTranslationDone processing error");
            }
        }



    }
}
