using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameGUI.Controls
{
    [ExecuteInEditMode]
    public class SlidePanel : MonoBehaviour
    {
        public IndexPanel Index;

        public List<SlidePanelNode> Nodes { get; private set; }
        public SlidePanelNode ActiveNode { get; private set; }

        public SlidePanelNode GetNodeByIndex(int index)
        {
            return Nodes[index];
        }

        public SlidePanelNode GetNodeByName(string nodeName)
        {
            return Nodes.FirstOrDefault(node => node.Name == nodeName);
        }

        private string lastActiveNodeName;
        public string ActiveNodeName;

        public void SetActiveNode(string nodeName, bool animated)
        {

            ActiveNodeName = nodeName;
            lastActiveNodeName = nodeName;

            if (!animated)
            {
               
            }
        }

        public void Next()
        {
            
        }

        public void Prev()
        {
            
        }

        private void Start()
        {
            Nodes = new List<SlidePanelNode>(GetComponentsInChildren<SlidePanelNode>());
        }

        private void Update()
        {


            if (ActiveNodeName == string.Empty )
            {
                var n = GetNodeByIndex(0).name;
                SetActiveNode(n, false);
            }
            else if (ActiveNodeName != lastActiveNodeName || ActiveNodeName == string.Empty)
            {
                SetActiveNode(ActiveNodeName, false);
            }
            else
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
}
