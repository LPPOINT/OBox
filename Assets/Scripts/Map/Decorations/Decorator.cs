using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Assets.Scripts.Levels;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Map.Decorations
{
    public class Decorator : LevelElement
    {


        public class DecoratorEvent : LevelEvent
        {
            public DecoratorEvent(DecoratorStatus status, DecorationPlaymode playMode)
            {
                PlayMode = playMode;
                Status = status;
            }

            public DecorationPlaymode PlayMode { get; private set; }
            public DecoratorStatus Status { get; private set; }

            public enum DecoratorStatus
            {
                Started,
                Done
            }

        }

        public int CurrentDecorationIndex { get; private set; }
        public List<Decoration> Decorations { get; private set; }

        private List<Decoration> currentDecorations;

        private bool isFirstRun = true;
        public bool Enabled = false;

        private bool isDone;

        public DecorationPlaymode CurrentPlayMode { get; private set; }

        public IEnumerable<Decoration> GetDecorationsByIndex(int index, DecorationPlaymode playmode)
        {
            return Decorations.Where(decoration => decoration.PlayIndex == index && decoration.Playmode == playmode);
        }

        public void InvalidateDecorations()
        {
            Decorations = new List<Decoration>(FindObjectsOfType<Decoration>());

            foreach (var decoration in Decorations)
            {
                decoration.Decorator = this;
            }
        }

        private void Start()
        {
            InvalidateDecorations();
        }


        public void ResetDecorator()
        {
            foreach (var d in Decorations.Where(decoration => decoration.Playmode == CurrentPlayMode))
            {
                d.ResetDecoration();
            }
        }

        public void Play(DecorationPlaymode playMode)
        {
            isDone = false;
            if (!Enabled)
            {
                FireEvent(new DecoratorEvent(DecoratorEvent.DecoratorStatus.Done, playMode));
                return;
            }
            InvalidateDecorations();

            if(!isFirstRun)
                 ResetDecorator();

            isFirstRun = false;
            CurrentDecorationIndex = 0;
            CurrentPlayMode = playMode;

            foreach (var decoration in Decorations)
            {
                decoration.OnDecotorStarted();
            }

            PlayNextDecorations();
        }

        private void PlayNextDecorations()
        {

            if (isDone)
            {
                FireEvent(new DecoratorEvent(DecoratorEvent.DecoratorStatus.Done, CurrentPlayMode));
                return;
            }

            currentDecorations = GetDecorationsByIndex(CurrentDecorationIndex, CurrentPlayMode).ToList();

            if (!currentDecorations.Any())
            {
                FireEvent(new DecoratorEvent(DecoratorEvent.DecoratorStatus.Done, CurrentPlayMode));
                isDone = true;
            }

            foreach (var currentDecoration in currentDecorations)
            {
                currentDecoration.Play();
            }

            CurrentDecorationIndex++;


        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                FireEvent(new DecoratorEvent(DecoratorEvent.DecoratorStatus.Done, CurrentPlayMode));
            }
        }

        internal void RegisterDecorationDone(Decoration decoration)
        {
            var removeResult = currentDecorations.Remove(decoration);

            if (!removeResult)
            {
                Debug.LogWarning("RegisterDecorationDone(): decoration not found");
            }

            if (!currentDecorations.Any())
            {
                PlayNextDecorations();
            }

        }
        

    }
}
