using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameGUI.Controls
{
    public class SlidePanel : MonoBehaviour
    {
        public IndexPanel Index;
        public GameObject Movable;

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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Next();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Prev();
            }


        }

    }
}
