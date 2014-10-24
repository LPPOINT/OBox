using UnityEngine;

namespace Assets.Scripts.GameGUI
{
    public class IndexNode : MonoBehaviour
    {
        private const string IdleMinAnimation = "IdleMin";
        private const string IdleMaxAnimation = "IdleMax";

        private const string FillInAnimation = "FillIn";
        private const string FillOutAnimation = "FillOut";


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
