namespace Assets.Scripts.Map.EventSystem
{
    public interface IEventAction
    {
        void Execute(Event e, EventParam args);
    }
}
