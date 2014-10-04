namespace Assets.Scripts.Map.Decorations
{
    public class TimeDecoration : Decoration
    {
        public float Time;
        private float currentTime;


        protected override void OnDecorationStart()
        {
            currentTime = 0;
        }

        protected override void OnDecorationUpdate()
        {
            currentTime += UnityEngine.Time.deltaTime;
            if (currentTime > Time)
            {
                OnDecorationEnd();
            }
        }
    }
}
