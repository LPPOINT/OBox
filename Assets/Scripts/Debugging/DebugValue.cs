using System;
using UnityEngine;

namespace Assets.Scripts.Debugging
{
    public class DebugValue
    {
        private DebugValue(string key, object objectValue, Type valueType)
        {
            ValueType = valueType;
            ObjectValue = objectValue;
            Key = key;
        }

        public enum Type
        {
            String,
            Bool,
            Float,
            Undefined
        }

        public string Key { get; private set; }
        public object ObjectValue { get; private set; }
        public Type ValueType { get; private set; }

        public string StringValue
        {
            get { return ObjectValue.ToString(); }
        }
        public bool BoolValue
        {
            get { return (bool) ObjectValue; }
        }
        public float FloatValue
        {
            get
            {
                if (ValueType == Type.Float) return (float) ObjectValue;
                if (ValueType == Type.Bool) return BoolValue ? 1 : 0;
                return -1;
            }
        }

        private static string GetKeyFor(Type type, string key)
        {
            return "Debug." + type + "." + key;
        }

        private static Type GetTypeByKey(string key)
        {
            try
            {
                var split = key.Split('.');
                return (Type)Enum.Parse(typeof(Type), split[1]);
            }
            catch 
            {
                return Type.Undefined;
            }
        }

        private static string GetSubkeyByKey(string key)
        {
            var split = key.Split('.');
            return split[2];
        }

        public void Save()
        {
            var key = GetKeyFor(ValueType, Key);
            
            if(ValueType == Type.String) PlayerPrefs.SetString(key, StringValue);
            else PlayerPrefs.SetFloat(key, FloatValue);

        }

        public DebugValue Load(string key)
        {
            var type = GetTypeByKey(key);
            if (type == Type.Undefined)
            {
                UnityEngine.Debug.LogWarning("Cant load debug value");
                return null;
            }
            if (type == Type.String)
            {
                return String(GetSubkeyByKey(key), PlayerPrefs.GetString(key));
            }
            if (type == Type.Float || type == Type.Bool)
            {
                var value = PlayerPrefs.GetFloat(key);
                if (type == Type.Float) return Float(GetSubkeyByKey(key), value);
                if (type == Type.Bool)
                {
                    var boolValue = value != 0;
                    return Bool(GetSubkeyByKey(key), boolValue);
                }
            }
            return null;
        }

        public static DebugValue String(string key, string value)
        {
            return new DebugValue(key, value, Type.String);
        }

        public static DebugValue Bool(string key, bool value)
        {
            return new DebugValue(key, value, Type.Bool);
        }

        public static DebugValue Float(string key, float value)
        {
            return new DebugValue(key, value, Type.Float);
        }




    }
}
