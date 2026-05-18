using UnityEngine;

public class BotPlayerBrain : MonoBehaviour ,ICarBrain
{
    private Vector3? _targetPosition=null;

    public Vector3? TargetPosition
    {
        get => _targetPosition;
        set => _targetPosition = value;
    }


    public Vector3 GetTargetPosition()
    {
        return TargetPosition ?? transform.position;
    }
}
