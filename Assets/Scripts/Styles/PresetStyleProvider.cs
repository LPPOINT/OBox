namespace Assets.Scripts.Styles
{
    public class PresetStyleProvider : StyleProvider
    {

        public StylePreset Preset;

        public override IStyle GetStyle()
        {
            return new Style(Preset.Entries);
        }
    }
}
