using UnityEngine;

namespace Assets.Scripts.GameGUI.Controls
{
    public class IndexNode : MonoBehaviour
    {
        private const string IdleMinAnimation = "IndexNodeMinimized";
        private const string IdleMaxAnimation = "IndexNode";

        private const string FillInAnimation = "IndexNodeMinimize";
        private const string FillOutAnimation = "IndexNodeMaximize";


        private void Start()
        {
            SetMin();
        }

        public enum IndexNodeState
        {
            Min,
            Max
        }

        public IndexNodeState CurrentState { get; private set; }

        private void InvokeAnimator(string animationStateName, IndexNodeState nodeState)
        {
            CurrentState = nodeState;
            GetComponent<Animator>().Play(animationStateName);
        }

        public void Minimize()
        {
            InvokeAnimator(FillInAnimation, IndexNodeState.Min);
        }

        public void Maximize()
        {
            InvokeAnimator(FillOutAnimation, IndexNodeState.Max);
        }

        public void SetMin()
        {
            InvokeAnimator(IdleMinAnimation, IndexNodeState.Min);
        }

        public void SetMax()
        {
            InvokeAnimator(IdleMaxAnimation, IndexNodeState.Max);
        }


    }
}
