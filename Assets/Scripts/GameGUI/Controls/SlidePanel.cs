using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameGUI.Controls
{
    public class SlidePanel : MonoBehaviour
    {
        public IndexPanel Index;
        public GameObject Movable;
        public RectTransform InputArea;

        public List<SlidePanelNode> Nodes { get; private set; }
        public SlidePanelNode ActiveNode { get; private set; }
        public SlidePanelNode StartNode;

        public bool IsLocked { get; private set; }

        public SlidePanelNode GetNodeByIndex(int index)
        {
            return Nodes[index];
        }

        private void OnITweenSlideStart()
        {
            IsLocked = true;
        }

        private void OnITweenSlideDone()
        {
            IsLocked = false;
        }

        public void SetActiveNode(SlidePanelNode node, bool animated)
        {

            var lastX = ActiveNode.transform.position.x;
            ActiveNode = node;
            var activeX = ActiveNode.transform.position.x;

            var offset = activeX - lastX;
            var newPos = new Vector3(Movable.transform.position.x - offset, Movable.transform.position.y,
                Movable.transform.position.z);

            iTween.MoveTo(Movable, 
                iTween.Hash("position", newPos,
                            "time", 0.3f,
                            "oncompletetarget", gameObject,
                            "oncomplete", "OnITweenSlideDone",
                            "onstart", "OnITweenSlideStart",
                            "onstarttarget", gameObject,
                            "easetype", iTween.EaseType.easeOutBack));


        }

        public void Next()
        {
            if(IsLocked)
                return;

            var currentIndex = Nodes.IndexOf(ActiveNode);

            if(currentIndex + 1 >= Nodes.Count)
                return;

            SetActiveNode(Nodes[currentIndex + 1], true);
            if(Index != null) Index.Next();
        }

        public void Prev()
        {
            if (IsLocked)
                return;
            var currentIndex = Nodes.IndexOf(ActiveNode);

            if(currentIndex == 0)
                return;

            SetActiveNode(Nodes[currentIndex - 1], true);

            if (Index != null) Index.Prev();
        }

        private void Start()
        {
            Nodes = new List<SlidePanelNode>(GetComponentsInChildren<SlidePanelNode>());
            ActiveNode = StartNode;
        }

        private float TotalOffset;

        private void Offset(float horizontalOffset)
        {
            TotalOffset += horizontalOffset;
            Movable.transform.position = new Vector3(Movable.transform.position.x + horizontalOffset, Movable.transform.position.y, Movable.transform.position.z);
        }

        private void OffsetDrop()
        {
            if (IsTranslationOffset(TotalOffset))
            {
                OffsetNext(TotalOffset);
            }
            else
                OffsetNext(TotalOffset);

            TotalOffset = 0;
        }

        private void OffsetNext(float offset)
        {
            if (offset > 0)
            {
                Next();
            }
            else
            {
                Prev();
            }
        }

        private void OffsetBack(float offset)
        {
            
        }


        private bool IsTranslationOffset(float offset)
        {
            return Math.Abs(offset) >= InputArea.rect.width/3;
        }

        private bool inOffset = false;
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.RightArrow)) Next();
            if(Input.GetKeyDown(KeyCode.LeftArrow)) Prev();

        }

    }
}
