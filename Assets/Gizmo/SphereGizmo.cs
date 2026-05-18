using UnityEngine;

public class SphereGizmo : MonoBehaviour
{
    [SerializeField] private float radius = 0.06f;
    [SerializeField] private Color gizmoColor = Color.yellow;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
