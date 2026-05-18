using System;
using System.Collections;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [Header("PoolManager")]
    [SerializeField] private PoolManager poolManager;

    [Header("Paths (4 total)")]
    [SerializeField] private Transform[] pathParents;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnDelay = 10f;

    private GameObject[] activeCars;

    private void OnEnable()
    {
        EventManager.OnGameStart += ActiveNpCs;
    }
    
    private void OnDisable()
    {
        EventManager.OnGameStart -= ActiveNpCs;
    }


    private void ActiveNpCs()
    {
        activeCars = new GameObject[pathParents.Length];

        for (int i = 0; i < pathParents.Length; i++)
        {
            int index = i;
            StartCoroutine(SpawnLoop(index));
        }
    }

    private IEnumerator SpawnLoop(int pathIndex)
    {
            if (activeCars[pathIndex] == null)
            {
                SpawnCar(pathIndex);
            }

            yield return new WaitForSeconds(spawnDelay);
    }

    private IEnumerator ReSpawnLoop(int pathIndex)
    {
        yield return new WaitForSeconds(spawnDelay);
        if (activeCars[pathIndex] == null) {
            SpawnCar(pathIndex);
        }
    }
    
    private void SpawnCar(int pathIndex)
    {
        Transform path = pathParents[pathIndex];
        GameObject car = poolManager.Get(PoolManager.PoolType.NpcCar);
        car.transform.position = path.position;
        TraficNPCBrain brain = car.GetComponent<TraficNPCBrain>();
        CarController controller = car.GetComponent<CarController>();
        
        brain.SetWaypoints(path);

        void OnCarFinished()
        {
            controller.OnFinished -= OnCarFinished;
            poolManager.Return(PoolManager.PoolType.NpcCar, car);
            activeCars[pathIndex] = null;
            StartCoroutine(ReSpawnLoop(pathIndex));
        }
        
        controller.OnFinished += OnCarFinished;

        activeCars[pathIndex] = car;
    }
}