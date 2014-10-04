
using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts.Map.Decorations
{
    public class MoveFromDecoration : Decoration
    {
        public iTween.EaseType EaseType = iTween.EaseType.linear;
        public Vector3 From;
        public float Time = 1;

        private Vector3 startPosition;

        public override bool DisableRendererBeforePlaying
        {
            get { return true; }
        }

        protected override void Start()
        {
            startPosition = transform.position;
            base.Start();
        }

        public Vector3 StartPosition { get; private set; }

        private void OnITweenAnimationEnd()
        {
            OnDecorationEnd();
        }

        protected override void OnDecorationStart()
        {
            base.OnDecorationStart();
            StartPosition = transform.position;

            foreach (var r in GetComponentsInChildren<SpriteRenderer>())
            {
                r.enabled = true;
            }

            iTween.MoveFrom(gameObject,
                iTween.Hash("position", From,
                            "time", Time,
                            "easetype", EaseType,
                            "oncomplete", "OnITweenAnimationEnd"));

        }

        //[LevelEventFilter(typeof(DecorationShelduler.DecorationsSheldulerEvent))]
        //public void OnSheldulerStart(DecorationShelduler.DecorationsSheldulerEvent e)
        //{
        //    if (e.Status == DecorationShelduler.DecorationsSheldulerEvent.DecorationSheldulerStatus.Started)
        //    {
        //        foreach (var r in GetComponentsInChildren<Renderer>())
        //        {
        //            r.enabled = false;
        //        }
        //    }
        //}

    }
}
