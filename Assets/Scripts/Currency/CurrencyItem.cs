using UnityEngine;
using System;
using System.Collections.Generic;

public class CurrencyItem : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private List<GameObject> visuals;
    
    public Action<CurrencyCollector, int> OnCollected;
    
    private int _amount;

    public void SetAmount(int value)
    {
        _amount = value;
        ApplyVisual(_amount);
    }
    
    private void ApplyVisual(int value)
    {

        Color color;

        switch (value)
        {
            case 1000:
                UpdateVisuals(0);
                break;

            case 5000:
                UpdateVisuals(1);
                break;

            case 10000:
                UpdateVisuals(2);
                break;

            case 50000:
                UpdateVisuals(3);
                break;

            default:
                break;
        }

    }

    private void UpdateVisuals(int index)
    {
        for (var i = 0; i < visuals.Count; i++)
        {
            visuals[i].SetActive(i == index);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        CurrencyCollector collector = other.GetComponent<CurrencyCollector>();

        if (collector == null)
            return;

        OnCollected?.Invoke(collector, _amount);
    }
    
}