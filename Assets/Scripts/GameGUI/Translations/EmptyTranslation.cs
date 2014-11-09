namespace Assets.Scripts.GameGUI.Translations
{
    public class EmptyTranslation : GUITranslation
    {
        protected override void OnActivated()
        {
            OnDone();
        }
    }
}
