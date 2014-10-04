namespace Assets.Scripts.Map.Decorations
{
    public class ITweenEventDecorator : Decoration
    {
        public iTweenEvent Event;
        public bool ShouldDisableRendererBeforePlay = true;

        public override bool DisableRendererBeforePlaying
        {
            get { return ShouldDisableRendererBeforePlay; }
        }

        protected override void Start()
        {
            if (Event == null) Event = GetComponent<iTweenEvent>();

            if (Event != null)
            {
                Event.Values["oncomplete"] = "OnITweenDone";
                Event.Values["oncompletetarget"] = gameObject;
            }

            base.Start();

        }

        protected override void OnDecorationStart()
        {
            base.OnDecorationStart();
            if (Event != null)
            {
                Event.Play();
            }
        }

        private void OnITweenDone()
        {
            OnDecorationEnd();
        }

    }
}
