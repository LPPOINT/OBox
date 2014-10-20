using System;
using Assets.Scripts.Styles.Gradient;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.LevelSelection
{
    public class LSNodesSupervisor : MonoBehaviour
    {

        private void Start()
        {
            CurrentLoadingState = LSNodeLoadingState.NoLoading;
            CurrentNodeIndex = 1;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                LoadNextNode();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                LoadPrevNode();
            }
        }

        public LSNode NextNode { get; private set; }
        public LSNode CurrentNode { get; private set; }
        public LSNode PrevNode { get; private set; }

        public GradientBackground Background;

        public int CurrentNodeIndex { get; private set; }

        public static string GetLSNodeLevelName(int nodeNumber)
        {
            return "LS" + nodeNumber;
        }

        public enum LSNodeLoadingState
        {
            NoLoading,
            LoadingNext,
            LoadingPrev
        }

        public LSNodeLoadingState CurrentLoadingState { get; private set; }

        public void LoadNextNode()
        {
            LoadNode(GetLSNodeLevelName(CurrentNodeIndex++), LSNodeLoadingState.LoadingNext);
        }
        public void LoadPrevNode()
        {
            LoadNode(GetLSNodeLevelName(CurrentNodeIndex--), LSNodeLoadingState.LoadingPrev);
        }

        public void LoadNode(string nodeLevelName, LSNodeLoadingState loadingState)
        {
            CurrentLoadingState = loadingState;
            Application.LoadLevelAdditiveAsync(nodeLevelName);
        }

        private void Unload(LSNode node)
        {
            Destroy(node.gameObject);
            Debug.Log("LSNode unloaded");
        }
        private void UnloadNext()
        {
            Unload(NextNode);
        }
        private void UnloadPrev()
        {
            Unload(PrevNode);
        }

        public void RegisterNode(LSNode node)
        {
            if (CurrentLoadingState == LSNodeLoadingState.NoLoading)
            {
                Debug.LogWarning("LSSupervisor: Unexpected RegisterNode() call");
                return;
            }
            AlignGradientToNode(node);

            PlaceNode(node, CurrentLoadingState);
            CurrentLoadingState = LSNodeLoadingState.NoLoading;



            CurrentNode = node;

        }

        private void AlignGradientToNode(LSNode node)
        {
            if(Background == null)
                return;

            Background.ColorProvider = node.Style;

        }

        private void ConnectSplines(CurvySplineBase top, CurvySplineBase bottom,
            LSNode.LSNodeSplineConnection connectionType)
        {
           
        }
        private void PlaceNode(LSNode node, LSNodeLoadingState loadingState)
        {
            if(loadingState == LSNodeLoadingState.NoLoading) throw new InvalidOperationException();

            if (loadingState == LSNodeLoadingState.LoadingNext)
            {
                var currentY = CurrentNode != null ? CurrentNode.transform.position.y : 0;
                node.transform.position = new Vector3(node.transform.position.x, currentY + node.Height, node.transform.position.z);
            }

        }

    }
}
