using System;
using UnityEngine;

public class CarSteering : MonoBehaviour
{
    [SerializeField] protected float maxTurnSpeed = 360f;
    [SerializeField] protected float angularDamping = 8f;

    protected Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        StopAndResetRotation();
    }

    public virtual void UpdateRotation(Vector3 target)
    {
        ApplyAngularDamping();

        Vector3 direction = target - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f) return;

        Quaternion targetRot = Quaternion.LookRotation(direction.normalized);

        float step = maxTurnSpeed * Time.fixedDeltaTime;

        _rb.MoveRotation(Quaternion.RotateTowards(
            _rb.rotation,
            targetRot,
            step
        ));
    }

    protected void ApplyAngularDamping()
    {
        _rb.angularVelocity = Vector3.Lerp(
            _rb.angularVelocity,
            Vector3.zero,
            angularDamping * Time.fixedDeltaTime
        );
    }
    
    
    public void StopAndResetRotation()
    {
        _rb.angularVelocity = Vector3.zero;
    }
}