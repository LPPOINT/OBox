using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SmartLocalization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameGUI.Pages
{
    public class GUILevelLoadingPage : GUIPage
    {
        public string LevelPath;

        public List<string> Cutaways { get; private set; }

        public float CutawayMinTime;
        public float CutawayMaxTime;

        public float LoadingTextMinTime;
        public float LoadingTextMaxTime;

        private float currentTextTime;
        private float currentMaxTextTime;

        public float MinLoadingTime;
        private float currentLoadingTime;

        public Color OutlineColor = Color.white;
        public float OutlineAmount = 7.2f;
        public float OutlineTime = 0.3f;
        public bool IsOutlineEnabled = true;

        public bool AllUpperCase = true;

        public override GUIPageType Type
        {
            get { return GUIPageType.LevelLoading; }
        }

        public override GUIPageMode Mode
        {
            get { return GUIPageMode.Logical; }
        }

        public bool IsEditorMode
        {
            get { return string.IsNullOrEmpty(LevelPath); }
        }

        public AsyncOperation LevelLoadingOperation { get; private set; }

        public Text Progress;
        public Text Loading;

        private bool isCutaway;
        private int lastCutawayIndex = -1;

        private Outline currentOutline;
        private bool playingOutlineMax;

        private void OnITweenOutlineUpdate(float newValue)
        {
            if (currentOutline != null)
            {
                currentOutline.effectDistance = new Vector2(newValue, newValue);
            }
        }

        private void OnITweenOutlineDone()
        {
            if (!playingOutlineMax)
            {
                if (currentOutline != null)
                {
                    Destroy(currentOutline);
                }
            }
            else
            {
                playingOutlineMax = false;
                iTween.ValueTo(Loading.gameObject,
                        iTween.Hash("onupdate", "OnITweenOutlineUpdate", "onupdatetarget", gameObject, "from", OutlineAmount, "to",
                            0, "time", OutlineTime/2, "oncomplete", "OnITweenOutlineDone", "oncompletetarget",
                             gameObject));

            }
        }

        private void PlayOutline()
        {
            if (currentOutline != null)
                Destroy(currentOutline);

            currentOutline = Loading.gameObject.AddComponent<Outline>();
            currentOutline.effectColor = OutlineColor;
            currentOutline.effectDistance = new Vector2(0, 0);

            playingOutlineMax = true;

            iTween.ValueTo(Loading.gameObject,
                iTween.Hash("onupdate", "OnITweenOutlineUpdate", "onupdatetarget", gameObject, "from", 0, "to",
                    OutlineAmount, "time", OutlineTime/2, "oncomplete", "OnITweenOutlineDone", "oncompletetarget",
                    gameObject));

        }

        private string GetCutawayText(int cutawayIndex)
        {
            var key = Cutaways[cutawayIndex];
            return LanguageManager.Instance.GetTextValue(key);
        }
        private string GetRandomCutawayText(int oldCutawayIndex)
        {

            if (Cutaways.Count == 0)
            {
                Debug.LogWarning("GetRandomCutawayText() Cutaways.Length == 0");
                return "GetRandomCutawayText() Cutaways.Length == 0";
            }

            if (Cutaways.Count <= 1)
                oldCutawayIndex = -1;

            var randomIndex = Random.Range(0, Cutaways.Count);

            if (randomIndex == oldCutawayIndex)
                return GetRandomCutawayText(oldCutawayIndex);


            lastCutawayIndex = randomIndex;

            return GetCutawayText(randomIndex);

        }

        private void SetText(string text)
        {
            if (AllUpperCase) text = text.ToUpper();
            Loading.text = text;
        }
        private void SetCutawayText()
        {
            isCutaway = true;
            SetText(GetRandomCutawayText(lastCutawayIndex));
        }
        private void SetLoadingText()
        {
            isCutaway = false;
            SetText(LanguageManager.Instance.GetTextValue("Loading.Loading"));
        }

        private void OnITweenProgressEmulatorUpdate(float newValue)
        {
            Progress.text = ((int) newValue).ToString() + "%";
        }

        private void OnITweenProgressEmulatorDone()
        {
            if(IsEditorMode)
                iTween.ValueTo(gameObject, iTween.Hash("onupdate", "OnITweenProgressEmulatorUpdate", "oncomplete", "OnITweenProgressEmulatorDone", "from", 0, "to", 100, "time", 5, "easetype", iTween.EaseType.easeOutQuad));
        }

        private void Start()
        {

            var values = LanguageManager.Instance.LanguageDatabase.Where(pair => pair.Key.StartsWith("Loading.Cutaway"));
            Cutaways = new List<string>();

            isCutaway = true;

            foreach (var keyValuePair in values)
            {
                Cutaways.Add(keyValuePair.Key);
            }

            if (string.IsNullOrEmpty(LevelPath))
            {
                Debug.LogWarning("LevelPath not initialized. ");

            }

            if (IsEditorMode)
            {
                iTween.ValueTo(gameObject, iTween.Hash("onupdate", "OnITweenProgressEmulatorUpdate", "oncomplete", "OnITweenProgressEmulatorDone", "from", 0, "to", 100, "time", MinLoadingTime - currentLoadingTime, "easetype", iTween.EaseType.easeOutQuad));
            }

            Progress.text = "0%";

            Loading.text = LanguageManager.Instance.GetTextValue("Loading.Loading");

            if (!string.IsNullOrEmpty(LevelPath))
            {
                LevelLoadingOperation = Application.LoadLevelAsync(LevelPath);
                if (LevelLoadingOperation == null)
                {
                    Debug.Log("Somethig wrong while trying to lad level.");
                    LevelPath = string.Empty;
                    return;
                }
                LevelLoadingOperation.allowSceneActivation = false;
            }
        }

        private bool isForcingToEnd = false;

        private void Update()
        {
            if (LevelLoadingOperation != null)
            {
                Progress.text = ((int)(LevelLoadingOperation.progress * 100)).ToString(CultureInfo.InvariantCulture) + "%";
            }

            if (LevelLoadingOperation != null && LevelLoadingOperation.progress >= 88 / 100  && !isForcingToEnd)
            {
                isForcingToEnd = true;
                iTween.ValueTo(gameObject, iTween.Hash("onupdate", "OnITweenProgressEmulatorUpdate", "oncomplete", "OnITweenProgressEmulatorDone", "from", ((int)(LevelLoadingOperation.progress * 100)), "to", 100, "time", 5, "easetype", iTween.EaseType.easeOutQuad));
            }


            currentTextTime += Time.deltaTime;
            currentLoadingTime += Time.deltaTime;


            if (currentLoadingTime > MinLoadingTime && LevelLoadingOperation != null)
            {
                LevelLoadingOperation.allowSceneActivation = true;
            }

            if (currentTextTime > currentMaxTextTime)
            {
                if (IsOutlineEnabled)
                    PlayOutline();
                currentTextTime = 0;
                if (isCutaway)
                {
                    currentMaxTextTime = Random.Range(LoadingTextMinTime, LoadingTextMaxTime);
                    SetLoadingText();
                }
                else
                {
                    currentMaxTextTime = Random.Range(CutawayMinTime, CutawayMaxTime);
                    SetCutawayText();
                }
            }

        }

        public override void OnShow()
        {
            base.OnShow();
        }
    }
}
