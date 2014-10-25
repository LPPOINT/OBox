using System.Collections.Generic;

namespace Assets.Scripts.Debugging
{
    public static class DebugInspector
    {

        public static bool Enabled { get; set; }

        private static List<DebugValue> values = new List<DebugValue>();
        public static List<DebugValue> Values
        {
            get { return values; }
            private set { values = value; }
        }


        public static void Save()
        {
            
        }

        public static void Load()
        {
            
        }

    }
}
