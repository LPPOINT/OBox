using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts.Map.Decorations
{
    public class Decoration : MonoBehaviour
    {



        public Decorator Decorator { get; internal set; }

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


        protected Vector3 StartPosition { get; private set; }
        protected Vector3 StartScale { get; private set; }

        protected virtual void Start()
        {
            CurrentStatus = DecorationStatus.Stopped;
        }

        public virtual void ResetDecoration()
        {

            transform.position = StartPosition;
            transform.localScale = StartScale;
        }


        public virtual void OnDecotorPlay()
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

        public class DecorationEvent : LevelEvent
        {
            public DecorationEvent(DecorationPlayState playState)
            {
                PlayState = playState;
            }

            public enum DecorationPlayState
            {
                Started,
                Done
            }

            public DecorationPlayState PlayState { get; private set; }

        }

        public void Play()
        {
            CurrentStatus = DecorationStatus.Playing;
            OnDecorationStart();
            isEnded = false;
        }

        public void Stop()
        {

            CurrentStatus = DecorationStatus.Stopped;
        }


        private bool isEnded;

        protected virtual void OnDecorationEnd()
        {


            if (isEnded)
            {
                Debug.LogError("IsEnded = true");
            }

            isEnded = true;


            if (RefreshTileIndexesAfterDone)
            {
                var items = GetComponentsInChildren<MapItem>();


                foreach (var mapItem in items)
                {
                    mapItem.RefreshIndex();
                }
            }



            CurrentStatus = DecorationStatus.Stopped;
            Decorator.RegisterDecorationDone(this);
        }

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
