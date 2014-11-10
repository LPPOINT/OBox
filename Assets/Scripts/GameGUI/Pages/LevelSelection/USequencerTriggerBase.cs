using UnityEngine;
using WellFired;

namespace Assets.Scripts.GameGUI.Pages.LevelSelection
{
    public abstract class USequencerTriggerBase : USEventBase
    {
        protected USequencerTriggerBase()
        {
            if (Duration == -1) Duration = 0.01f;
        }

        public abstract void Execute();

        private bool wasExecuted;

        public override void FireEvent()
        {
            wasExecuted = false;
        }

        public override void ProcessEvent(float runningTime)
        {

            if (!wasExecuted)
            {
                Execute();
                wasExecuted = true;
            }
        }
    }
}
