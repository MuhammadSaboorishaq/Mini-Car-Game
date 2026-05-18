using System;
using UnityEngine;

public class TrailResetter : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;


    private void OnEnable()
    {
        ResetTrail();
    }

    private void OnDisable()
    {
        ResetTrail();
    }

    private void ResetTrail()
    {
        trailRenderer.enabled = false;
        trailRenderer.Clear();
        trailRenderer.enabled = true;
    }
}
