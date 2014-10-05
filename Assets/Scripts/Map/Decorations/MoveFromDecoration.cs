﻿
using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts.Map.Decorations
{
    public class MoveFromDecoration : Decoration
    {
        public iTween.EaseType EaseType = iTween.EaseType.linear;
        public Vector3 From;
        public float Time = 1;


        public override bool DisableRendererBeforePlaying
        {
            get { return true; }
        }

        protected override void Start()
        {

            base.Start();
        }


        private void OnITweenAnimationEnd()
        {
            OnDecorationEnd();
        }

        protected override void OnDecorationStart()
        {
            base.OnDecorationStart();


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

        //[LevelEventFilter(typeof(Decorator.DecoratorEvent))]
        //public void OnSheldulerStart(Decorator.DecoratorEvent e)
        //{
        //    if (e.Status == Decorator.DecoratorEvent.DecoratorStatus.Started)
        //    {
        //        foreach (var r in GetComponentsInChildren<Renderer>())
        //        {
        //            r.enabled = false;
        //        }
        //    }
        //}

    }
}
