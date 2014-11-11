using Assets.Scripts.GameGUI.Pages.LevelSelection.USequencerIntegration;
using UnityEngine;

namespace Assets.Scripts.GameGUI.Pages.LevelSelection
{
    public abstract class LSCameraControllerBase : MonoBehaviour
    {
        public UnityEngine.Camera Camera { get; private set; }

        private void Start()
        {
            Camera = UnityEngine.Camera.main;
        }


        protected void SetDirty()
        {
            foreach (var i in USequencerPlayer.Instances)
            {
                i.UpdateSequence();
            }
        }
    }
}
