using UnityEngine;

namespace Assets.Scripts.GameGUI.Pages.LevelSelection
{
    public class EnableBehaviourActivity : LSActivity
    {

        public Behaviour Behaviour;

        private void Start()
        {
            if(Behaviour == null)
                return;
            Behaviour.enabled = false;
        }

        public override void OnActivityBecameActivated()
        {
            base.OnActivityBecameActivated();
            if (Behaviour == null)
                return;
            Behaviour.enabled = true;
        }

        public override void OnActivityBecameDeactivated()
        {
            if (Behaviour == null)
                return;
            Behaviour.enabled = false;
            base.OnActivityBecameDeactivated();
        }
    }
}
