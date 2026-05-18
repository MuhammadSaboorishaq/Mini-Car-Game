using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public enum PoolType
    {
        NpcCar,
        BankNote,
        VFX
    }
    
    [System.Serializable]
    public class Pool
    {
        public PoolType type;
        public GameObject prefab;
        public int initialSize = 10;
    }

    [SerializeField] private Pool[] pools;

    private Dictionary<PoolType, Queue<GameObject>> _poolDictionary;
    private Dictionary<PoolType, GameObject> _prefabLookup;

    private void Awake()
    {
        _poolDictionary = new Dictionary<PoolType, Queue<GameObject>>();
        _prefabLookup = new Dictionary<PoolType, GameObject>();

        foreach (var pool in pools)
        {
            Queue<GameObject> queue = new Queue<GameObject>();

            _prefabLookup[pool.type] = pool.prefab;

            for (int i = 0; i < pool.initialSize; i++)
            {
                GameObject obj = CreateNew(pool.type);
                queue.Enqueue(obj);
            }

            _poolDictionary.Add(pool.type, queue);
        }
    }

    public GameObject Get(PoolType type)
    {
        if (!_poolDictionary.ContainsKey(type))
        {
            Debug.LogWarning("Pool not found: " + type);
            return null;
        }

        Queue<GameObject> pool = _poolDictionary[type];

        if (pool.Count == 0)
        {
            GameObject expanded = CreateNew(type);
            expanded.SetActive(true);
            return expanded;
        }

        GameObject obj = pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void Return(PoolType type, GameObject obj)
    {
        obj.SetActive(false);

        if (!_poolDictionary.ContainsKey(type))
        {
            Debug.LogWarning("Pool not found on return: " + type);
            return;
        }

        _poolDictionary[type].Enqueue(obj);
    }

    private GameObject CreateNew(PoolType type)
    {
        if (!_prefabLookup.ContainsKey(type))
        {
            Debug.LogError("No prefab registered for: " + type);
            return null;
        }

        GameObject obj = Instantiate(_prefabLookup[type]);
        obj.SetActive(false);
        return obj;
    }
}