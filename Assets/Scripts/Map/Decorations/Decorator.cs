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


        private List<Decoration> sceneDecorations;
        private List<Decoration> currentDecorations;
        private DecorationPlaymode currentPlaymode;

        public int CurrentIndex { get; private set; }
        public int MaxIndex { get; private set; }
        public int MinIndex { get; private set; }

        private void FetchSceneDecorations()
        {
            sceneDecorations = new List<Decoration>(FindObjectsOfType<Decoration>());

            MaxIndex = sceneDecorations.Max(decoration => decoration.PlayIndex);
            MinIndex = sceneDecorations.Min(decoration => decoration.PlayIndex);

            foreach (var d in sceneDecorations)
            {
                d.Decorator = this;
            }
        }
        private IEnumerable<Decoration> GetDecorationsByIndex(int index, DecorationPlaymode playmode)
        {
            var res =  sceneDecorations.Where(decoration => decoration.PlayIndex == index && decoration.Playmode == playmode);
            return res;
        }

        private void NotifyDecoratorDone()
        {
            FireEvent(new DecoratorEvent(DecoratorEvent.DecoratorStatus.Done, currentPlaymode));
        }
        private void NotifyDecoratorStarted()
        {
            FireEvent(new DecoratorEvent(DecoratorEvent.DecoratorStatus.Started, currentPlaymode));
        }

        public bool IsPlaying { get; private set; }

        public void Terminate()
        {
            Debug.Log("Decorator.Terminate()");
            currentDecorations.Clear();
            IsPlaying = false;
        }

        private void ResetDecorator()
        {
            if(sceneDecorations == null) return;
            foreach (var d in sceneDecorations.Where(decoration => decoration.Playmode == currentPlaymode))
            {
                d.ResetDecoration();
            }
        }

        public void Play(DecorationPlaymode playmode)
        {
            if (IsPlaying)
            {
                return;
            }

            ResetDecorator();

            IsPlaying = true;
            FetchSceneDecorations();
            CurrentIndex = MinIndex;
            currentPlaymode = playmode;

            foreach (var d in sceneDecorations.Where(decoration => decoration.Playmode == currentPlaymode))
            {
                d.OnDecotorPlay();
            }


            PlayNewIndex();
            NotifyDecoratorStarted();
        }

        private void PlayNewIndex()
        {

            MaxIndex = sceneDecorations.Where((decoration => decoration.Playmode == currentPlaymode)).Max(decoration =>  decoration.PlayIndex);
            MinIndex = sceneDecorations.Where((decoration => decoration.Playmode == currentPlaymode)).Min(decoration => decoration.PlayIndex);


            if (CurrentIndex > MaxIndex)
            {
                IsPlaying = false;
                NotifyDecoratorDone();
                return;
            }

            currentDecorations = new List<Decoration>(GetDecorationsByIndex(CurrentIndex, currentPlaymode));

            foreach (var d in currentDecorations)
            {
                d.Play();
            }

        }

        internal void RegisterDecorationDone(Decoration decoration)
        {

            if (!IsPlaying)
            {
                Debug.LogError("IN RegisterDecorationDone() When IsPlaying = false");
                return;
            }

            var removeResult = currentDecorations.Remove(decoration);
            if (!removeResult)
            {
                Debug.LogError("UNEXPECTED DECORATION PASSED TO RegisterDecorationDone(). Decorator will be terminated");
                NotifyDecoratorDone();
                Terminate();
                return;
            }
            if (!currentDecorations.Any())
            {
                CurrentIndex++;
                PlayNewIndex();
            }
        }
    }
}
