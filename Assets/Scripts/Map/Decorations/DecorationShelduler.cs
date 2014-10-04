using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts.Map.Decorations
{
    public class DecorationShelduler : LevelElement
    {
        public int CurrentDecorationIndex { get; private set; }
        public List<Decoration> Decorations { get; private set; }

        private List<Decoration> currentDecorations;

        private bool isFirstRun = true;

        public DecorationPlaymode CurrentPlayMode { get; private set; }

        public IEnumerable<Decoration> GetDecorationsByIndex(int index, DecorationPlaymode playmode)
        {
            return Decorations.Where(decoration => decoration.PlayIndex == index && decoration.Playmode == playmode);
        }

        public void InvalidateDecorations()
        {
            Decorations = new List<Decoration>(FindObjectsOfType<Decoration>());
        }

        private void Start()
        {
            InvalidateDecorations();
        }

        public class DecorationsSheldulerEvent : LevelEvent
        {
            public DecorationsSheldulerEvent(DecorationSheldulerStatus status, DecorationPlaymode playMode)
            {
                PlayMode = playMode;
                Status = status;
            }

            public DecorationPlaymode PlayMode { get; private set; }
            public DecorationSheldulerStatus Status { get; private set; }

            public enum DecorationSheldulerStatus
            {
                Started,
                Done
            }

        }

        public void Reset()
        {
            foreach (var d in Decorations.Where(decoration => decoration.Playmode == CurrentPlayMode))
            {
                d.Reset();
            }
        }

        public void Play(DecorationPlaymode playMode)
        {

            if(!isFirstRun)
                 Reset();

            isFirstRun = false;

            InvalidateDecorations();
            CurrentDecorationIndex = 0;
            CurrentPlayMode = playMode;

            FireEvent(new DecorationsSheldulerEvent(DecorationsSheldulerEvent.DecorationSheldulerStatus.Started, playMode));

            PlayNextDecorations();
        }

        private void PlayNextDecorations()
        {
            currentDecorations = GetDecorationsByIndex(CurrentDecorationIndex, CurrentPlayMode).ToList();

            if (!currentDecorations.Any())
            {
                FireEvent(new DecorationsSheldulerEvent(DecorationsSheldulerEvent.DecorationSheldulerStatus.Done, CurrentPlayMode));
            }

            foreach (var currentDecoration in currentDecorations)
            {
                currentDecoration.Play(CurrentPlayMode);
            }

            CurrentDecorationIndex++;
        }

        [LevelEventFilter(typeof (Decoration.DecorationEvent))]
        public void OnDecorationEvent(Decoration.DecorationEvent e)
        {

            if (e.Status == Decoration.DecorationEvent.DecorationStatus.Done)
            {
                currentDecorations.Remove(e.Element as Decoration);

                if (!currentDecorations.Any())
                {
                    PlayNextDecorations();
                }

            }
        }

    }
}
