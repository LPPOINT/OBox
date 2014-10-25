using System;
using UnityEngine;

namespace Assets.Scripts.GameGUI
{
    public class GUITranslation : MonoBehaviour
    {


        private static T CreateTranslation<T>() where T : GUITranslation
        {
            foreach (var t in FindObjectsOfType<GUITranslation>())
            {
                Destroy(t.gameObject);
            }

            var newTranslationObj = new GameObject("Translation");
            return newTranslationObj.AddComponent<T>();

        }


        public static EmptyTranslation CreateEmptyTranslation()
        {
            return CreateTranslation<EmptyTranslation>();
        }

        public static GUITranslation CreateDefaultTranslation()
        {
            return CreateTranslation<FadeTranslation>();
        }

        public static FadeTranslation CreateFadeTranslation()
        {
            return CreateTranslation<FadeTranslation>();
        }

        public event EventHandler Done;

        protected virtual void OnDone()
        {
            IsActive = false;
            var handler = Done;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void OnUpdate()
        {
            
        }

        protected virtual void OnActivated()
        {
            
        }

        public GUIPage From { get; private set; }
        public GUIPage To { get; private set; }

        public bool IsActive { get; private set; }


        private void Start()
        {
            IsActive = false;
        }

        private void Update()
        {
            if (IsActive)
            {
                OnUpdate();
            }
        }

        public void Activate(GUIPage from, GUIPage to)
        {
            if (IsActive)
            {
                return;
            }

            From = from;
            To = to;

            IsActive = true;
            OnActivated();
        }

    }
}
