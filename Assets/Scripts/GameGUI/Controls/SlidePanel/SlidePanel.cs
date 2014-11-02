using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Assets.Scripts.GameGUI.Controls.SlidePanel
{
    public class SlidePanel : MonoBehaviour
    {
        public SlideIndexPanel SlideIndex;
        public GameObject Movable;
        public RectTransform InputArea;

        public List<SlidePanelNode> Nodes { get; private set; }
        public SlidePanelNode ActiveNode { get; private set; }
        public SlidePanelNode StartNode;

        public class SlidePanelNodeChangedEventArgs : EventArgs
        {
            public SlidePanelNodeChangedEventArgs(SlidePanelNode prevNode, SlidePanelNode currentNode)
            {
                PrevNode = prevNode;
                CurrentNode = currentNode;
            }

            public SlidePanelNode PrevNode { get; private set; }
            public SlidePanelNode CurrentNode { get; private set; }
        }

        public event EventHandler<SlidePanelNodeChangedEventArgs> NodeChanged;

        protected virtual void OnNodeChanged(SlidePanelNodeChangedEventArgs e)
        {
            var handler = NodeChanged;
            if (handler != null) handler(this, e);
        }

        public void Add(GameObject obj)
        {
            
        }

        public void Remove(SlidePanelNode node)
        {

            Nodes.Remove(node);
            
        }

        private readonly List<SlidePanelNode> disabledNodes = new List<SlidePanelNode>(); 

        public void Disable(SlidePanelNode node)
        {
            disabledNodes.Add(node);
        }

        public void Enable(SlidePanelNode node)
        {
            disabledNodes.Remove(node);
        }

        public bool IsDisabled(SlidePanelNode node)
        {
            return disabledNodes.Contains(node);
        }

        public bool IsDisabled(int index)
        {
            var node = GetNodeByIndex(index);
            if (node == null) return false;
            return IsDisabled(node);
        }

        public void Check()
        {
            OnNodeChanged(new SlidePanelNodeChangedEventArgs(ActiveNode, ActiveNode));
        }

        public int ActiveNodeIndex
        {
            get { return Nodes.IndexOf(ActiveNode); }
        }

        public bool IsLocked { get; private set; }

        public SlidePanelNode GetNodeByIndex(int index)
        {
            try
            {
                return Nodes[index];
            }
            catch (Exception e)
            {
                return null;
            }
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
            var lastActive = ActiveNode;
            ActiveNode.IsSelected = false;
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
            ActiveNode.IsSelected = true;
            OnNodeChanged(new SlidePanelNodeChangedEventArgs(lastActive, ActiveNode));

        }

        public void Next()
        {
            if(IsLocked)
                return;

            var currentIndex = Nodes.IndexOf(ActiveNode);

            if(currentIndex + 1 >= Nodes.Count)
                return;

            SetActiveNode(Nodes[currentIndex + 1], true);
            if(SlideIndex != null) SlideIndex.Next();
        }

        public void Prev()
        {
            if (IsLocked)
                return;
            var currentIndex = Nodes.IndexOf(ActiveNode);

            if(currentIndex == 0)
                return;

            SetActiveNode(Nodes[currentIndex - 1], true);

            if (SlideIndex != null) SlideIndex.Prev();
        }

        private void Awake()
        {
            Nodes = new List<SlidePanelNode>(GetComponentsInChildren<SlidePanelNode>());
            ActiveNode = StartNode;
            ActiveNode.IsSelected = true;
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

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.RightArrow)) Next();
            if(Input.GetKeyDown(KeyCode.LeftArrow)) Prev();

        }

    }
}
