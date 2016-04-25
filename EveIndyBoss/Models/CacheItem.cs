namespace EveIndyBoss.Models
{
    public class CacheItem<T>
    {
        private CacheItem(T data, bool found)
        {
            Data = data;
            Found = found;
        }

        public T Data { get; private set; }
        public bool Found { get; protected set; }

        public static CacheItem<T> CreateFound(T data)
        {
            return new CacheItem<T>(data, true);
        }

        public static CacheItem<T> CreateNotFound()
        {
            return new CacheItem<T>(default(T), false);
        }
    }
}