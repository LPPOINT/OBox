namespace Assets.Scripts.Map.EventSystem
{
    public class Event
    {
        public string Name { get; set; }
        public IEventSelector ObjectProvider { get; set; }
    }
}
