namespace DDT.SimpleHttpServer
{
    public interface ISession
    {
        object this[string key] { get; set; }

        T Get<T>(string key);
        object Get(string key);

        void Set(string key, object value);

        void Clear();
    }
}