using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts.Map.Decorations
{
    public class Decoration : LevelElement
    {


        public enum DecorationStatus
        {
            Playing,
            Stopped
        }


        public int PlayIndex;
        public DecorationPlaymode Playmode;

        public virtual bool RefreshTileIndexesAfterDone
        {
            get
            {
                return true;
            }
        }

        public virtual bool DisableRendererBeforePlaying
        {
            get
            {
                return false;
            }
        }

        public virtual bool StopAnimatorBeforePlaying
        {
            get
            {
                return true;
            }
        }

        public virtual bool CanBeRoot
        {
            get
            {
                return true;
            }
        }

        public DecorationStatus CurrentStatus { get; private set; }
        public DecorationPlaymode CurrentPlayMode { get; private set; }


        protected Vector3 StartPosition { get; private set; }
        protected Vector3 StartScale { get; private set; }

        public virtual void Reset()
        {
            transform.position = StartPosition;
            transform.localScale = StartScale;
        }

        protected virtual void Start()
        {

        }

        [LevelEventFilter(typeof(DecorationShelduler.DecorationsSheldulerEvent))]
        public  void OnDecorationsEvent(DecorationShelduler.DecorationsSheldulerEvent e)
        {


            if (e.Status == DecorationShelduler.DecorationsSheldulerEvent.DecorationSheldulerStatus.Started)
            {

                StartPosition = transform.position;
                StartScale = transform.localScale;

                if (DisableRendererBeforePlaying)
                {
                    if (GetComponent<Renderer>() != null)
                        GetComponent<Renderer>().enabled = false;
                    foreach (var r in GetComponentsInChildren<Renderer>())
                    {
                        r.enabled = false;
                    }
                }
            }
        }

        public class DecorationEvent : LevelEvent
        {
            public DecorationEvent(DecorationStatus status)
            {
                Status = status;
            }

            public enum DecorationStatus
            {
                Started,
                Done
            }

            public DecorationStatus Status { get; private set; }

        }

        public void Play(DecorationPlaymode playMode)
        {
            CurrentPlayMode = playMode;
            CurrentStatus = DecorationStatus.Playing;
            OnDecorationStart();
        }

        protected virtual void OnDecorationEnd()
        {


            var items = GetComponentsInChildren<MapItem>();

            foreach (var mapItem in items)
            {
                mapItem.RefreshIndex();
            }



            CurrentStatus = DecorationStatus.Stopped;
            FireEvent(new DecorationEvent(DecorationEvent.DecorationStatus.Done));
        }

        private bool isAnimatorWasEnabled;

        protected virtual void OnDecorationStart()
        {
            if (DisableRendererBeforePlaying)
            {
                if(GetComponent<Renderer>() != null)
                    GetComponent<Renderer>().enabled = true;
                foreach (var r in GetComponentsInChildren<Renderer>())
                {
                    r.enabled = true;
                }
            }



            FireEvent(new DecorationEvent(DecorationEvent.DecorationStatus.Started));
        }

        protected virtual void OnDecorationUpdate()
        {
            
        }

        private void Update()
        {
            if(CurrentStatus == DecorationStatus.Playing)
                OnDecorationUpdate();
        }

    }
}
