using System;

namespace Assets.Scripts.Levels
{
    public class LevelEventFilter : Attribute
    {
        public LevelEventFilter(Type eventType, Type customSenderType, bool shouldProcessInMainHandler = false)
        {
            ShouldProcessInMainHandler = shouldProcessInMainHandler;
            CustomSenderType = customSenderType;
            EventType = eventType;
        }

        public LevelEventFilter(Type eventType)
        {
            EventType = eventType;
            CustomSenderType = null;
            ShouldProcessInMainHandler = false;
        }

        public Type EventType { get; private set; }
        public Type CustomSenderType { get; private set; }
        public bool ShouldProcessInMainHandler { get; private set; }
    }
}
