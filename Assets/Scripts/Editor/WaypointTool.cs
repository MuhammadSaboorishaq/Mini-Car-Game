using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class WaypointTool
{
    private static Transform parent;
    private static bool active;

    static WaypointTool()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    [MenuItem("Tools/Waypoint Tool/Toggle Click Mode")]
    public static void Toggle()
    {
        active = !active;
        Debug.Log("Waypoint Tool: " + (active ? "Enabled" : "Disabled"));
    }

    [MenuItem("Tools/Waypoint Tool/Set Parent (Selected Object)")]
    public static void SetParent()
    {
        if (Selection.activeTransform != null)
        {
            parent = Selection.activeTransform;
            Debug.Log("Waypoint Parent set to: " + parent.name);
        }
        else
        {
            Debug.LogWarning("Select a parent object first in hierarchy.");
        }
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        if (!active || parent == null)
            return;

        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            {
                CreateWaypoint(hit.point);
                e.Use();
            }
        }
    }

    private static void CreateWaypoint(Vector3 position)
    {
        GameObject wp = new GameObject("Waypoint_" + parent.childCount);
        wp.transform.position = position;
        wp.transform.parent = parent;
        wp.AddComponent<SphereGizmo>();

        Undo.RegisterCreatedObjectUndo(wp, "Create Waypoint");
        EditorUtility.SetDirty(wp);
    }
}