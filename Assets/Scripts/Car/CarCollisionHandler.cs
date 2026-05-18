using System;
using UnityEngine;

public class CarCollisionHandler : MonoBehaviour
{
    [Header("Collision Layers")]
    [SerializeField] private LayerMask trafficLayers;
    [SerializeField] private LayerMask collectableLayers;
    [SerializeField] private LayerMask playerLayer;

    public event Action<Collision> OnTrafficCollision;
    public event Action<Collision> OnCollectableCollision;
    public event Action<Collision> OnPlayerCollision;
    
    private void OnTriggerEnter(Collider collider)
    {
        GameObject hitObject = collider.gameObject;

        int layerMask = 1 << hitObject.layer;
        
        if ((collectableLayers & layerMask) != 0)
        {
            OnCollectableCollision?.Invoke(null);
            return;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject hitObject = other.gameObject;

        int layerMask = 1 << hitObject.layer;

        if ((trafficLayers & layerMask) != 0)
        {
            OnTrafficCollision?.Invoke(other);
            return;
        }
        
        if ((playerLayer & layerMask) != 0)
        {
            OnPlayerCollision?.Invoke(other);
            return;
        }
    }
    
}