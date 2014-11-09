using Holoville.HOTween;
using UnityEngine;

namespace Assets.Scripts.GameGUI.Pages.LevelSelection
{
    public class LSActivity : MonoBehaviour
    {

        public bool IsActive { get; private set; }


        public Transform ActivityStartPosition;
        public Transform ActivityEndPosition;
        public Transform AlignPosition;
        public ActivityMode Mode;

        public enum ActivityMode
        {
            Area,
            Trigger
        }


        public float Lenght
        {
            get { return ActivityEndPosition.transform.position.x - ActivityStartPosition.transform.position.x; }
        }

        private float lastSeed = -1;

        public virtual void ReceiveUpdate(float seed)
        {
            if(Mode == ActivityMode.Area) 
                OnActivityUpdate(seed);

            if (AlignPosition != null && lastSeed != -1)
            {
                var offset = seed - lastSeed;

                transform.Translate(offset, 0, 0);
            }
            lastSeed = seed;
        }

        public virtual void OnActivityBecameActivated()
        {
            IsActive = true;
        }
        public virtual void OnActivityBecameDeactivated()
        {
            IsActive = false;
        }
        protected virtual void OnActivityUpdate(float seed)
        {
            
        }



    }
}
