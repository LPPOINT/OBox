using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


        private void SetupNode(SlidePanelNode node, GUIWorldFeature feature)
        {

            var localizedText = LanguageManager.Instance.GetTextValue(feature.Description);
            var image = feature.Image;

            var targetImage = node.transform.FindChild("Image");
            var targetText = node.transform.FindChild("Text");

            targetImage.GetComponent<Image>().sprite = image;
            targetText.GetComponent<Text>().text = localizedText;
        }

        private void SetupFeatureSlider()
        {
            var features = Data.Features;
            var featuresCount = features.Count;
            var featureNodes = FeatureSlider.Nodes;
            var templateNode = featureNodes.First();
            var templateNodeTransform = templateNode.GetComponent<RectTransform>();
            var templateNodePosition = templateNodeTransform.position;
            var offset = templateNodeTransform.rect.width + 20;

            SetupNode(templateNode,  features.First());

            if(featuresCount == 1) return;

            for (var i = 1; i < featuresCount; i++)
            {
                var nodeFeature = features[i];
                var node = (Instantiate(templateNode.gameObject) as GameObject).GetComponent<SlidePanelNode>();

                var rectTransform = node.GetComponent<RectTransform>();

                rectTransform.position = new Vector3(templateNodePosition.x + offset * i, templateNodePosition.y, templateNodePosition.z);
                var scale = 0.8f; // calculate this shit
                rectTransform.localScale = new Vector3(scale, scale, scale);
                rectTransform.parent = templateNode.transform.parent;

                var nl = rectTransform.rect.xMin;
                var nt = rectTransform.rect.yMin;

                var sizeOffsetX = templateNodeTransform.rect.width - rectTransform.rect.width;
                var sizeOffsetY = templateNodeTransform.rect.height - rectTransform.rect.height;

                //rectTransform.rect.Set(templateNodeTransform.rect.xMin + (offset*i), templateNodeTransform.rect.yMin, templateNodeTransform.rect.width, templateNodeTransform.rect.height);
                

                SetupNode(node, nodeFeature);

                FeatureSlider.Add(node);



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
