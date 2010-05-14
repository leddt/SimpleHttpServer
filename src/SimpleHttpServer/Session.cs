using System.Collections.Generic;

namespace DDT.SimpleHttpServer
{
    public class Session : ISession
    {
        private readonly Dictionary<string, object> inner = new Dictionary<string,object>();

        public object this[string key]
        {
            get { return inner.ContainsKey(key) ? inner[key] : null; }
            set { inner[key] = value; }
        }

        public T Get<T>(string key)
        {
            return (T) this[key];
        }

        public object Get(string key)
        {
            return this[key];
        }

        public void Set(string key, object value)
        {
            this[key] = value;
        }

        public void Clear()
        {
            inner.Clear();
        }
    }
}