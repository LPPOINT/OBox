namespace Assets.Scripts.Map.Decorations
{
    public class EmptyDecoration : Decoration
    {
        protected override void OnDecorationStart()
        {
            base.OnDecorationStart();
        }

        protected override void OnDecorationUpdate()
        {
            base.OnDecorationUpdate();
            OnDecorationEnd();
        }
    }
}
