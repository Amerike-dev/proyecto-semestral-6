using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(BoxCollider))]
public class BuildingZoneArea : MonoBehaviour
{
    [Tooltip("Color del gizmo del área de construcción.")]
    public Color gizmoColor = new Color(0f, 0.6f, 1f, 0.15f);

    BoxCollider _box;

    public Bounds WorldBounds
    {
        get
        {
            EnsureBox();
            var b = _box.bounds;
            return b;
        }
    }

    void EnsureBox()
    {
        if (_box == null) _box = GetComponent<BoxCollider>();
    }

    public Vector3 ClampInside(Vector3 position, Vector3 halfExtents)
    {
        var b = WorldBounds;
        float x = Mathf.Clamp(position.x, b.min.x + halfExtents.x, b.max.x - halfExtents.x);
        float z = Mathf.Clamp(position.z, b.min.z + halfExtents.z, b.max.z - halfExtents.z);
        return new Vector3(x, position.y, z);
    }

    void OnDrawGizmos()
    {
        EnsureBox();
        if (_box == null) return;

        Gizmos.color = gizmoColor;
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.DrawCube(_box.bounds.center, _box.bounds.size);

        Gizmos.color = Color.cyan;
        var b = _box.bounds;
        Vector3 floorCenter = new Vector3(b.center.x, b.min.y, b.center.z);
        Vector3 floorSize = new Vector3(b.size.x, 0.01f, b.size.z);
        Gizmos.DrawCube(floorCenter, floorSize);
    }
}