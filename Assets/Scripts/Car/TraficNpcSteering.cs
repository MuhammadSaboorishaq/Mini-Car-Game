using UnityEngine;

public class TraficNpcSteering : CarSteering
{
    [SerializeField] private float maxSteerAngle = 45f;
    [SerializeField] private float steerSpeed = 5f;
    [SerializeField] private float minTurnSpeed = 0.5f;

    private float _currentSteer;
    public override void UpdateRotation(Vector3 target)
    {
        ApplyAngularDamping();

        Vector3 direction = target - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f) return;

        Vector3 forward = transform.forward;

        float angleToTarget = Vector3.SignedAngle(forward, direction.normalized, Vector3.up);
        float targetSteer = Mathf.Clamp(angleToTarget / maxSteerAngle, -1f, 1f);
        _currentSteer = Mathf.Lerp(_currentSteer, targetSteer, steerSpeed * Time.fixedDeltaTime);
        float speedFactor = Mathf.Clamp01(_rb.linearVelocity.magnitude);
        float turnMultiplier = Mathf.Lerp(1f, minTurnSpeed, speedFactor);
        float rotationAmount = _currentSteer * maxTurnSpeed * turnMultiplier * Time.fixedDeltaTime;

        Quaternion deltaRotation = Quaternion.Euler(0f, rotationAmount, 0f);

        _rb.MoveRotation(_rb.rotation * deltaRotation);
    }
}
