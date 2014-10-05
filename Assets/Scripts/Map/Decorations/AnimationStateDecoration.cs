using UnityEngine;

namespace Assets.Scripts.Map.Decorations
{
    public class AnimationStateDecoration : Decoration
    {
        public string AnimationName;
        private bool isPlaying;
        private Animator animator;

        protected override void OnDecorationStart()
        {
            isPlaying = true;
            animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play(AnimationName);
            }
            else
            {
                Debug.LogWarning("AnimationStateDecoration: animator not found");
                OnDecorationEnd();
            }
            base.OnDecorationStart();
        }

        protected override void OnDecorationUpdate()
        {
            animator = GetComponent<Animator>();
            if (isPlaying && animator != null && !animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationName))
            {
                isPlaying = false;
                OnDecorationEnd();
            }
            else if (animator == null)
            {
                Debug.LogWarning("AnimationStateDecoration: animator not found");
                OnDecorationEnd();
            }
            base.OnDecorationUpdate();
        }
    }
}
