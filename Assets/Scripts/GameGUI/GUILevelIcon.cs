using System;
using System.Globalization;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameGUI
{
    public class GUILevelIcon : MonoBehaviour
    {

        private void Start()
        {
            VisualizeByModel();
        }

        public GUILevelIconModel Model;


        public Text CompletedLevelText;
        public Text CurrentLevelText;
        public Image LockImage;

        public GameObject ThreeStarsContainer;
        public GameObject TwoStarsContainer;
        public GameObject OneStarContainer;

        public float CurrentLevelScale = 1.4f;
        public float CurrentLevelScaleOffset = 0.2f;
        public float CurrentLevelScaleTime = 0.8f;

        public GUILevelSelectionPage Page { get;  set; }

        private void SetScale(float scale)
        {
            transform.localScale = new Vector3(scale, scale, 1);
        }

        private void SetStarsVisibility(bool visible)
        {
            ThreeStarsContainer.gameObject.SetActive(visible);
            TwoStarsContainer.gameObject.SetActive(visible);
            OneStarContainer.gameObject.SetActive(visible);
        }

        public Color FrontColor;

        private void StartScaleAnimation()
        {
            iTween.ScaleTo(gameObject, iTween.Hash(
                "scale", new Vector3(CurrentLevelScale - CurrentLevelScaleOffset, CurrentLevelScale - CurrentLevelScaleOffset, 1), 
                "time", CurrentLevelScaleTime,
                "looptype", iTween.LoopType.pingPong,
                "easetype", iTween.EaseType.easeInOutCirc));
        }

        private void StopScaleAnimation()
        {
            iTween.Stop(gameObject);
        }

        private void EstablishStars()
        {
            SetStarsVisibility(false);
            GameObject targetContainer;
            switch (Model.Stars)
            {
                case StarsCount.OneStar:
                    targetContainer = OneStarContainer;
                    break;
                case StarsCount.TwoStar:
                    targetContainer = TwoStarsContainer;
                    break;
                case StarsCount.ThreeStar:
                    targetContainer = ThreeStarsContainer;
                    break;
                default:
                    targetContainer = OneStarContainer;
                    break;
            }
            targetContainer.SetActive(true);
        }

        private void EstablishText(Text textToEstablish, Text textToHide)
        {
            textToEstablish.gameObject.SetActive(true);
            textToHide.gameObject.SetActive(false);
            textToEstablish.text = Model.Number.ToString(CultureInfo.InvariantCulture);
        }

        public void VisualizeByModel()
        {
            SetScale(Model.Type == GUILevelIconModel.IconType.CurrentLevel ? CurrentLevelScale : 1);

            if(Model.Type == GUILevelIconModel.IconType.CurrentLevel)
                StartScaleAnimation();
            else 
                StopScaleAnimation();

            if (Model.Type == GUILevelIconModel.IconType.LockedLevel)
            {
                LockImage.gameObject.SetActive(true);
                CurrentLevelText.gameObject.SetActive(false);
                CompletedLevelText.gameObject.SetActive(false);
            }
            else 
            {
                LockImage.gameObject.SetActive(false);

                if(Model.Type == GUILevelIconModel.IconType.CurrentLevel)
                    EstablishText(CurrentLevelText, CompletedLevelText);
                else 
                    EstablishText(CompletedLevelText, CurrentLevelText);

                if (Model.Type == GUILevelIconModel.IconType.CurrentLevel)
                {
                    SetStarsVisibility(false);
                }
                else
                {
                    EstablishStars();
                }

            }

        }

        public void OnClick()
        {
            if(Model.Type != GUILevelIconModel.IconType.LockedLevel)
                GUISupervisor.Instance.OpenLevel(Model.Number, Page.World);
        }
    }
}
