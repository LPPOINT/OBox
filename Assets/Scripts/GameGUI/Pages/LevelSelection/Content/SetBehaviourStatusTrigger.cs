using UnityEngine;
using WellFired;

namespace Assets.Scripts.GameGUI.Pages.LevelSelection.Content
{

    [USequencerFriendlyName("Set Behaviour Status")]
    [USequencerEvent("LevelSelection/Set Behaviour Status")]
    public class SetBehaviourStatusTrigger : USequencerTriggerBase
    {
        public Behaviour Behaviour;
        public bool IsEnabled;


        public override void Execute()
        {
            if(Behaviour != null)
                Behaviour.enabled = IsEnabled;
        }
    }
}
