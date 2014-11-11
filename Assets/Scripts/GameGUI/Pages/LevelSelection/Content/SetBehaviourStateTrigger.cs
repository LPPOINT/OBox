using Assets.Scripts.GameGUI.Pages.LevelSelection.USequencerIntegration;
using UnityEngine;
using WellFired;

namespace Assets.Scripts.GameGUI.Pages.LevelSelection.Content
{

    [USequencerEvent("LevelSelection/Set Behaviour State")]
    [USequencerFriendlyName("Set Behaviour State")]
    public class SetBehaviourStateTrigger : USequencerTrigger
    {
        public Behaviour Behaviour;
        public bool StatusInsideTrigger = true;
        public bool StatusOutsideTrigger = false;


        private void ChangeState(UPlaymode playmode, bool fromStart)
        {
            if (Behaviour == null || !IsAreaTrigger) return;

            if (playmode == UPlaymode.Forward && fromStart) Behaviour.enabled = StatusInsideTrigger;
            if (playmode == UPlaymode.Forward && !fromStart) Behaviour.enabled = StatusOutsideTrigger;
            if (playmode == UPlaymode.Backward && fromStart) Behaviour.enabled = StatusOutsideTrigger;
            if (playmode == UPlaymode.Backward && !fromStart) Behaviour.enabled = StatusInsideTrigger;
        }

        public override void OnStartTriggerFired(UPlaymode playmode)
        {
            ChangeState(playmode, true);
        }

        public override void OnEndTriggerFired(UPlaymode playmode)
        {
            ChangeState(playmode, false);
        }
    }
}
