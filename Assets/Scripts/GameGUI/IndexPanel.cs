﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameGUI
{


    public class IndexPanel : MonoBehaviour
    {

        private IEnumerator Start()
        {
            nodes = new List<IndexNode>(GetComponentsInChildren<IndexNode>());
            TotalNodes = nodes.Count;

            yield return new WaitForEndOfFrame();

            if (TotalNodes >= FirstSelected - 1)
            {
                nodes[FirstSelected].SetMax();
                CurrentNode = FirstSelected;
            }

        }

        public int TotalNodes { get; private set; }
        public int CurrentNode { get; private set; }


        public int FirstSelected;

        private List<IndexNode> nodes; 

        public IndexNode GetNodeByNumber(int number)
        {
            return nodes[number];
        }

        public void SetNodeWithoutAnyAnimations(int node)
        {
             EnsureMinimized(node);
             GetNodeByNumber(CurrentNode).SetMin();
             CurrentNode = node;
             GetNodeByNumber(CurrentNode).SetMax();
        }

        public void SwapNodes(int lastMax, int newMax)
        {
            GetNodeByNumber(lastMax).Minimize();
            CurrentNode = newMax;
            EnsureMinimized(newMax);
            GetNodeByNumber(newMax).Maximize();
        }

        public void EnsureMinimized(int currentMax)
        {
            for (var i = 0; i < nodes.Count; i++)
            {
                if (i != currentMax)
                {
                    if (nodes[i].CurrentState == IndexNode.IndexNodeState.Max)
                    {
                        nodes[i].SetMin();
                    }
                }
            }
        }

        public void Next()
        {
            if (CurrentNode != TotalNodes - 1)
            {
                SwapNodes(CurrentNode, CurrentNode + 1);
            }
            else
            {
                Shake();
            }
        }

        public void Prev()
        {
            if (CurrentNode != 0)
            {
                SwapNodes(CurrentNode, CurrentNode-1);
            }
            else
            {
                Shake();
            }
        }

        public void Shake()
        {
            iTween.ShakePosition(gameObject, new Vector3(2, 2, 0), 0.3f);
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
