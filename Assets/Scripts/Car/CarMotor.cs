using System;
using UnityEngine;

public class CarMotor : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 25f;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _rb.useGravity = false;
        _rb.constraints =
            RigidbodyConstraints.FreezePositionY |
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;
    }

    private void OnDisable()
    {
        Stop();
    }

    public virtual void MoveTowards(Vector3 target, float stoppingDistance)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0f;

        if (direction.magnitude < stoppingDistance)
        {
            Decelerate();
            return;
        }

        Vector3 desiredVelocity = direction.normalized * maxSpeed;

        _rb.linearVelocity = Vector3.MoveTowards(
            _rb.linearVelocity,
            desiredVelocity,
            acceleration * Time.fixedDeltaTime
        );
    }

    public void Decelerate()
    {
        _rb.linearVelocity = Vector3.MoveTowards(
            _rb.linearVelocity,
            Vector3.zero,
            deceleration * Time.fixedDeltaTime
        );
    }
    
    public void Stop()
    {
        _rb.linearVelocity = Vector3.zero;
    }
}