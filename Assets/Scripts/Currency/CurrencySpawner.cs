using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CurrencySpawner : MonoBehaviour
{
    [Header("Spawn Area")]
    [SerializeField] private Vector3 center;
    [SerializeField] private Vector3 size = new Vector3(10f, 0f, 10f);

    [Header("Spawn Settings")]
    [SerializeField] private PoolManager poolManager;

    [SerializeField] private int maxActive = 10;
    [SerializeField] private float spawnInterval = 1.5f;

    [Header("Currency Values")]
    [SerializeField] private int[] values = { 1000, 5000, 10000, 50000 };

    [SerializeField] private  List<GameObject> _activeCurrency = new List<GameObject>();
    private float _timer;
    
    private bool spawnEnabled = false;

    private void OnEnable()
    {
        EventManager.OnGameStart+= EnableSpawn;
    }

    private void OnDisable()
    {
        EventManager.OnGameStart-= EnableSpawn;
    }


    private void EnableSpawn()=> spawnEnabled = true;
    
    private void Update()
    {
        if (!spawnEnabled) return;
        _timer += Time.deltaTime;

        if (_timer >= spawnInterval)
        {
            _timer = 0f;
            TrySpawn();
        }
    }

    private void TrySpawn()
    {
        if (_activeCurrency.Count >= maxActive)
            return;

        if (values == null || values.Length == 0)
        {
            Debug.LogWarning("No currency values assigned.");
            return;
        }

        Vector3 spawnPos = GetRandomPoint();

        GameObject currency = poolManager.Get(PoolManager.PoolType.BankNote);
        currency.transform.position = spawnPos;

        CurrencyItem item = currency.GetComponent<CurrencyItem>();

        int value = GetRandomValue();
        item.SetAmount(value);

        item.OnCollected = (collector, amount) =>
        {
            collector.AddMoney(amount);

            poolManager.Return(PoolManager.PoolType.BankNote, currency);
            _activeCurrency.Remove(currency);
        };

        _activeCurrency.Add(currency);
    }

    private int GetRandomValue()
    {
        int index = Random.Range(0, values.Length);
        return values[index];
    }

    private Vector3 GetRandomPoint()
    {
        float radius = size.x * 0.5f;

        Vector2 randomPoint = Random.insideUnitCircle * radius;

        Vector3 spawnPoint = new Vector3(
            randomPoint.x,
            1f,
            randomPoint.y
        );

        return transform.position + center + spawnPoint;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 drawCenter = transform.position + center;

        float radius = size.x * 0.5f;

        Gizmos.DrawWireSphere(drawCenter, radius);
    }
    
    
    public bool TryGetRandomActiveCurrencyPosition(out Vector3 position)
    {
        position = Vector3.zero;

        if (_activeCurrency.Count == 0)
            return false;
        
        int index = Random.Range(0, _activeCurrency.Count);
        position = _activeCurrency[index].transform.position;

        return true;
    }
    
}
