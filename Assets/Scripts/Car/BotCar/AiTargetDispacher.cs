using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AiTargetDispacher : MonoBehaviour
{
    [SerializeField] private float radius = 5f;
    [SerializeField] CurrencySpawner currencySpawner;
    [SerializeField] private float clampPositionY = 1.14f;
    [SerializeField] private float clampPositionX = 1.14f;
    private Vector3 point;
    public Vector3 GetRandomPointInRadius()
    {
        Vector3 center = transform.position;
        Vector2 randomPoint = Random.insideUnitCircle * radius;
        Vector3 position = new Vector3(center.x + randomPoint.x, 0, center.z + randomPoint.y);
        return position;
    }
    
    
    public Vector3 GetRandomCurrencyPoint()
    {
        Vector3 newPoint = Vector3.zero;
        point = currencySpawner.TryGetRandomActiveCurrencyPosition(out newPoint) ? newPoint : GetRandomPointInRadius();
        point = ClampPosition(point);
        return point;
    }

    private Vector3 ClampPosition(Vector3 position)
    {
        return new Vector3(
            Mathf.Clamp(position.x,  - clampPositionX, clampPositionX),
            0,
            Mathf.Clamp(position.z, - clampPositionY, clampPositionY)
        );
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(point, 0.04f);
        
    }
}
