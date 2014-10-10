namespace Assets.Scripts.Map.Decorations
{
    public class TipTextDecoration : AnimationStateDecoration
    {
        private const string ShowingAnimation = "TipTextShowing";
        private const string DisposingAnimation = "TipTextDisposing";

        protected override void OnDecorationStart()
        {

            if (Playmode == DecorationPlaymode.In) AnimationName = ShowingAnimation;
            if (Playmode == DecorationPlaymode.Out) AnimationName = DisposingAnimation;

            base.OnDecorationStart();
        }
    }
}
