using System.Collections.Concurrent;

namespace TinyURL.Cache;

public class LRUCache<K,V>
{
    private readonly int _maxCapacity;
    private readonly ConcurrentDictionary<K, V> _cache = new ();
    private readonly LinkedList<K> _linkedList = new ();
    private readonly object lockObject = new ();

    public LRUCache(int maxCapacity)
    {
        _maxCapacity = maxCapacity;
    }

    public bool TryGetValue(K key, out V? output)
    {
        if (!_cache.TryGetValue(key, out output)) return false;
        
        lock (lockObject)
        {
            _linkedList.Remove(key);
            _linkedList.AddLast(key);
        }
        return true;
    }

    public void Set(K key, V value)
    {
        lock (lockObject)
        {
            if (_cache.Count >= _maxCapacity)
            {
                var oldestKey = _linkedList.First != null ? _linkedList.First.Value : default;
                if (oldestKey != null)
                {
                    _linkedList.RemoveFirst();
                    _cache.TryRemove(oldestKey, out _);
                }
            }
            
            _cache[key] = value;
            _linkedList.Remove(key);
            _linkedList.AddLast(key);
        }
    }
}