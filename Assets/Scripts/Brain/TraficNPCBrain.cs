using UnityEngine;

public class TraficNPCBrain : MonoBehaviour, ICarBrain
{
    private Transform[] _waypoints;
    private int _index;

    [SerializeField] private float reachDistance = 2f;

    public void SetWaypoints(Transform pathParent)
    {
        _waypoints = new Transform[pathParent.childCount];

        for (int i = 0; i < pathParent.childCount; i++)
            _waypoints[i] = pathParent.GetChild(i);

        _index = 0;
    }

    public Vector3 GetTargetPosition()
    {
        if (_waypoints == null || _waypoints.Length == 0)
            return transform.position;

        return _waypoints[_index].position;
    }

    public bool TryAdvance(Vector3 position)
    {
        if (_waypoints == null || _waypoints.Length == 0)
            return false;

        float dist = Vector3.Distance(position, _waypoints[_index].position);

        if (dist < reachDistance)
        {
            if (_index < _waypoints.Length - 1)
            {
                _index++;
                return false;
            }
            return true; 
        }

        return false;
    }
}