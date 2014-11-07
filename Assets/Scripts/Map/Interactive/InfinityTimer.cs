
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Map.Interactive
{
    public class InfinityTimer : MonoBehaviour
    {
        public Text TimerText;
        public float StartTime = 10f;
        public string TimeFormat = "mm:ss";

        public float BlinkingStartTime = 4f;
        public float BlinkingDisabledTime = 0.3f;
        public float BlinkingEnabledTime = 0.6f;


        public AudioSource TickAudio;
        public AudioSource ResetAudio;

        public bool IsInfinity = true;

        public TimerDirection Direction;

        public enum TimerDirection
        {
            Up,
            Down
        }

        public float CurrentTime { get; private set; }


        private bool isBlinking = false;
        private float currentBlinkingTime;
        private float currentBlinkingMaxTime;
        private bool isVisible = true;

        private void SetVisibility(bool visibility)
        {
            isVisible = visibility;
            TimerText.enabled = isVisible;
        }

        private void StartBlinking()
        {
            isBlinking = true;
            currentBlinkingTime = 0;
            currentBlinkingMaxTime = BlinkingDisabledTime;
        }

        private void StopBlinking()
        {
            isBlinking = false;
            currentBlinkingTime = 0;
            SetVisibility(true);
        }

        private int lastSoundTime;

        private void OnTimerUpdate(float value)
        {
            CurrentTime = value;
            var ts = TimeSpan.FromSeconds(value);
            var date = new DateTime(ts.Ticks);
            TimerText.text = date.ToString(TimeFormat);
            if (ts.Seconds != lastSoundTime && TickAudio != null)
            {
                Debug.Log("PLay");
                TickAudio.Play();
                lastSoundTime = ts.Seconds;
            }
        }

        private void Update()
        {
            if (CurrentTime < BlinkingStartTime && !isBlinking && Direction == TimerDirection.Down)
            {
                StartBlinking();
            }
            if (isBlinking)
            {
                currentBlinkingTime += Time.deltaTime;
                if (currentBlinkingTime > currentBlinkingMaxTime)
                {
                    currentBlinkingTime = 0;
                    SetVisibility(!isVisible);
                    currentBlinkingMaxTime = isVisible ? BlinkingEnabledTime : BlinkingDisabledTime;
                }
            }
        }

        private void StartTimer(float from, float to, float seconds, iTween.EaseType easeType = iTween.EaseType.linear)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", from, "to", to, "time", seconds, "onupdate", "OnTimerUpdate", "oncomplete", "OnTimerDone", "easetype", easeType));
        }

        public event EventHandler TimerDone;
        
        protected virtual void FireDoneEvent()
        {
            var handler = TimerDone;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private void OnTimerDone()
        {

            if(Direction == TimerDirection.Down)
                FireDoneEvent();

            if(!IsInfinity)
                return;

            if (Direction == TimerDirection.Down)
            {
                Direction = TimerDirection.Up;
                if (ResetAudio != null)
                {
                    ResetAudio.Play();
                }
                StartTimer(0, StartTime+1, StartTime / 8, iTween.EaseType.easeOutCirc);
            }
            else if (Direction == TimerDirection.Up)
            {
                StopBlinking();
                Direction = TimerDirection.Down;
                StartTimer(StartTime, 0, StartTime);
            }
        }

        private void Start()
        {
            
            if (TimerText == null) 
                TimerText = GetComponent<Text>();
            CurrentTime = StartTime;
            Direction = TimerDirection.Down;
            StartTimer(StartTime, 0, StartTime);
        }

    }
}
