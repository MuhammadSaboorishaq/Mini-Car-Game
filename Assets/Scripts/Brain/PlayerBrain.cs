using System;
using UnityEngine;

public class PlayerCarBrain : MonoBehaviour, ICarBrain
{
    [SerializeField] private LayerMask groundMask;
    private Camera _cam;
    private bool _stopInput=true;
    private void Awake()
    {
        _cam = Camera.main;
    }

    private void OnEnable()
    {
        EventManager.OnGameStart+= resumeInput;
    }
    
    private void OnDisable()
    {
        EventManager.OnGameStart-= resumeInput;
    }
    
    private void resumeInput() => _stopInput = false;

    public Vector3 GetTargetPosition()
    {
        if (_stopInput || !Input.GetMouseButton(0)) return transform.position;

        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
            return hit.point;

        return transform.position;
    }
}