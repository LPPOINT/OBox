namespace Assets.Scripts.GameGUI
{
    public class EmptyTranslation : GUITranslation
    {
        protected override void OnActivated()
        {
            OnDone();
        }
    }
}
