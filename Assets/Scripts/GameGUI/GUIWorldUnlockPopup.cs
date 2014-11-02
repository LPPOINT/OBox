using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Camera.Effects;
using Assets.Scripts.GameGUI.Controls.SlidePanel;
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
        public IGUIWorldUnlockHandler UnlockHandler;

        private void SetupFeatureSlider()
        {
            var features = Data.Features;
            var featuresCount = features.Count;
            var featureNodes = FeatureSlider.Nodes;
            var cutPosition = -1;

            for (var i = 0; i < featureNodes.Count; i++)
            {
                if (i + 1 > featuresCount)
                {
                    cutPosition = i;
                    break;
                }

                var feature = features[i];
                var node = featureNodes[i];

                var localizedText = LanguageManager.Instance.GetTextValue(feature.Description);
                var image = feature.Image;

                var targetImage = node.transform.FindChild("Image");
                var targetText = node.transform.FindChild("Text");

                targetImage.GetComponent<Image>().sprite = image;
                targetText.GetComponent<Text>().text = localizedText;
            }

            try
            {
                if (cutPosition != -1)
                {
                    for (var i = cutPosition; i < featureNodes.Count; i++)
                    {

                        Destroy(featureNodes[i].gameObject);
                        //FeatureSlider.Remove(featureNodes[i]);
                        FeatureSlider.Check();
                    }

                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error while cutting extra nodes");
            }
        }

        private void Start()
        {
            Current = this;
            SetupFeatureSlider();
        }

        public void Unlock()
        {
            
        }

        public void Close()
        {
            CameraBlurEffect.BlurOut();
            Destroy(gameObject);
        }


    }
}
