using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Camera.Effects;
using Assets.Scripts.GameGUI.Controls.SlidePanel;
using Assets.Scripts.Model.Unlocks;
using SmartLocalization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameGUI
{
    public class GUIWorldUnlockPopup : MonoBehaviour
    {

        public static GUIWorldUnlockPopup Current { get; private set; }

        public SlidePanel FeatureSlider;

        public GUIWorldData Data;
        public IWorldUnlockHandler UnlockHandler;

        private void SetupFeatureSlider()
        {
            var features = Data.Features;
            var featuresCount = features.Count;
            var featureNodes = FeatureSlider.Nodes;
            var isCuttings = false;

            for (var i = 0; i < featureNodes.Count; i++)
            {
                if (i + 1 > featuresCount)
                {
                    isCuttings = true;
                }

                if (!isCuttings)
                {
                    var feature = features[i];
                    var node = featureNodes[i];

                    var localizedText = LanguageManager.Instance.GetTextValue(feature.Description);
                    var image = feature.Image;

                    var targetImage = node.transform.FindChild("Image");
                    var targetText = node.transform.FindChild("Text");

                    targetImage.GetComponent<Image>().sprite = image;
                    targetText.GetComponent<Text>().text = localizedText;
                }
                else
                {
                    
                    featureNodes[i].gameObject.SetActive(false);
                    FeatureSlider.Disable(featureNodes[i]);
                    FeatureSlider.Check();
                }
            }

       
        }



        private void Start()
        {
            Current = this;
            SetupFeatureSlider();
        }

        public void Unlock()
        {
            UnlockHandler.OnUnlockPerformed(new SingleWorldUnlockContext(Data.Number));
            GUIPage.Find<GUIWorldSelectionPage>().UpdateWorldNode(Data.Number);
            CameraBlurEffect.BlurOut();
            Destroy(gameObject);
        }

        public void Close()
        {
            CameraBlurEffect.BlurOut();
            Destroy(gameObject);
            UnlockHandler.OnUnlockCanceled();
        }


    }
}
