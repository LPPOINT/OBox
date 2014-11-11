using UnityEngine;
using WellFired;

namespace Assets.Scripts.GameGUI.Pages.LevelSelection.USequencerIntegration
{

    [USequencerFriendlyName("U Sequencer Trigger Base")]
    [USequencerEvent("LevelSelection/USequencerTriggerBase")]
    public  class USequencerTrigger : USEventBase
    {



        public enum TriggerActivationMode
        {
            MovingForward,
            MovingBackward,
            Both
        }

        public virtual void OnStartTriggerFired(UPlaymode playmode) {  }
        public virtual void OnEndTriggerFired(UPlaymode playmode) { }
        public virtual void OnTimeChanged(UPlaymode playmode, float newPosition) { }

        public bool IsAreaTrigger { get { return Duration != -1; } }

        public TriggerActivationMode ActivationMode = TriggerActivationMode.Both;

        private bool CanActivate(UPlaymode playmode)
        {
            return (playmode == UPlaymode.Forward && ActivationMode == TriggerActivationMode.MovingForward)
                   || (playmode == UPlaymode.Backward && ActivationMode == TriggerActivationMode.MovingBackward)
                   || ActivationMode == TriggerActivationMode.Both;
        }

        private float lastTime = -1;
        private UPlaymode ComputePlaymode(float time)
        {
            if (lastTime == -1)
            {
                lastTime = time;
                return UPlaymode.Forward;
            }
            if (time > lastTime)
            {
                lastTime = time;
                return UPlaymode.Forward;
            }
            if (time < lastTime)
            {
                lastTime = time;
                return UPlaymode.Backward;
            }

            return UPlaymode.Forward;

        }

        private bool isEndTriggerFired;
        private bool isStartTriggerFired;

        private bool lastDeltaTimeInitialized = false;
        private float lastDeltaTime;

        public override void ManuallySetTime(float deltaTime) // TODO: fix 
        {
            var playmode = ComputePlaymode(deltaTime);


            if (!lastDeltaTimeInitialized)
            {
                lastDeltaTimeInitialized = true;
                lastDeltaTime = deltaTime;
            }

            if (Duration != -1)
            {
                if (deltaTime < Duration && lastDeltaTime > Duration && CanActivate(playmode))
                {
                    OnEndTriggerFired(playmode);
                }
                else if (deltaTime >= 0 && deltaTime < Duration && CanActivate(playmode))
                {

                    if (lastDeltaTime < 0 && CanActivate(playmode)) OnStartTriggerFired(playmode);

                    isStartTriggerFired = false;
                    isEndTriggerFired = false;

                    OnTimeChanged(playmode, deltaTime);

                }
                else if (deltaTime > Duration && !isEndTriggerFired && CanActivate(playmode))
                {
                    isEndTriggerFired = true;
                    OnEndTriggerFired(playmode);
                }
                else if (deltaTime < 0 && !isStartTriggerFired && CanActivate(playmode))
                {
                    isStartTriggerFired = true;
                    OnStartTriggerFired(playmode);
                }

            }
            else
            {
                if (deltaTime >= 0 && lastDeltaTime < 0 && CanActivate(playmode)) OnStartTriggerFired(playmode);
                if (deltaTime < 0 && lastDeltaTime > 0 && CanActivate(playmode)) OnEndTriggerFired(playmode);
            }

            lastDeltaTime = deltaTime;

            base.ManuallySetTime(deltaTime);
        }

        #region Unused USEventBase functions implementation
        public override void FireEvent()
        {

        }
        public override void ProcessEvent(float runningTime)
        {

        }
        #endregion
    }
}
