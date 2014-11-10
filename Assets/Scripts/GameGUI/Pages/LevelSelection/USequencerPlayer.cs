﻿using UnityEngine;
using WellFired;

namespace Assets.Scripts.GameGUI.Pages.LevelSelection
{
    public class USequencerPlayer : MonoBehaviour
    {

        public static USequencerPlayer Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public USSequencer Sequencer;
        public Transform EndPosition;
        public Transform StartPosition;
        public UnityEngine.Camera TargetCamera;

        public float Lenght
        {
            get { return EndPosition.transform.position.x - StartPosition.transform.position.x; }
        }

        public float Duration
        {
            get { return Sequencer.Duration; }
        }

        public float CalculateTime(float globalPosition)
        {
            return Sequencer.Duration * (globalPosition - StartPosition.transform.position.x) / Lenght;
        }

        public float CalculateLocalPosition(float time)
        {
            return Lenght * time / Sequencer.Duration;
        }

        public float CalculateGlobalPosition(float time)
        {
            return StartPosition.transform.position.x + CalculateLocalPosition(time);
        }

        public void UpdateSequence()
        {
            var thisCam = TargetCamera ?? UnityEngine.Camera.main;
            var farClip = thisCam.farClipPlane;


            var topLeftPosition = thisCam.ViewportToWorldPoint(new Vector3(0, 1, farClip));
            var topRightPosition = thisCam.ViewportToWorldPoint(new Vector3(1, 1, farClip));

            var minX = topLeftPosition.x;
            var maxX = topRightPosition.x;
            var centerX = thisCam.transform.position.x;

            var sequencerTime = CalculateTime(centerX);
            if(sequencerTime < 0 || sequencerTime > Sequencer.Duration)
                return;
            Sequencer.SetPlaybackTime(sequencerTime);
        }
    }
}
