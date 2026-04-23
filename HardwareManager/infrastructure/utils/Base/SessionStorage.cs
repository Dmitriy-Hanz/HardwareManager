using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareManager.infrastructure.utils.Base
{
    class SessionStorage
    {
        private static SessionStorage singleton = new SessionStorage();
        private SessionStorage() 
        { 
            storage = new Dictionary<string, object>();
            flags = new HashSet<string>();
        }
        public static SessionStorage GetInstance() => singleton;



        private Dictionary<string, object> storage;
        private HashSet<string> flags;

        public object GetValue(string key)
        {
            object value = null;
            if (key != null && !key.Equals(""))
            {
                storage.TryGetValue(key, out value);
                if (value != null)
                {
                    storage.Remove(key);
                    //throw new Exception($"Missing value in session storage with key \"{key}\"");
                }
            }
            return value;
        }

        public T GetValue<T>(string key)
        {
            object value = null;
            if (key != null && !key.Equals(""))
            {
                storage.TryGetValue(key, out value);
                if (value != null)
                {
                    storage.Remove(key);
                    //throw new Exception($"Missing value in session storage with key \"{key}\"");
                }
            }
            return (T)value;
        }

        public T GetValueOrNull<T>(string key)
        {
            object value = null;
            if (key != null && !key.Equals(""))
            {
                if (storage.ContainsKey(key))
                {
                    storage.TryGetValue(key, out value);
                    storage.Remove(key);
                }
            }
            return (T)value;
        }


        public void PutValue(string key, object value)
        {
            if (key != null && !key.Equals("") && value != null)
            {
                storage.Add(key, value);
            }
        }

        public void SetFlag(string flag)
        {
            if (flag != null && !flag.Equals(""))
            {
                flags.Add(flag);
            }
        }

        public bool CheckFlag(string flag)
        {
            return flags.Remove(flag);
        }

        public bool Contains(string key)
        {
            return storage.ContainsKey(key);
        }
    }
}
