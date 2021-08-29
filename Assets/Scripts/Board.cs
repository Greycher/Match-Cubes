using UnityEngine;

public class Board : MonoBehaviour {
    [SerializeField] private Cube cubeRes;
    [SerializeField] private uint rowCount = 10;
    [SerializeField] private uint columnCount = 12;
    [SerializeField] private Color debugBorderColor = Color.green;

    private const float CubeSpawnViewportHeight = 1.1f;
    private const float ColliderWidth = 1f;
    
    private void Awake() {
        SetupColliders();
        PopulateCubes();
    }

    private void SetupColliders() {
        SetupRightCollider();
        SetupLeftCollider();
        SetupBottomCollider();
    }

    private void SetupRightCollider() {
        var collider = gameObject.AddComponent<BoxCollider2D>();
        var halfColumnUnit = columnCount * 0.5f;
        collider.offset = transform.TransformPoint(new Vector3(halfColumnUnit + ColliderWidth * 0.5f, 0, 0));
        collider.size = new Vector3(ColliderWidth, rowCount, 0);
    }
    
    private void SetupLeftCollider() {
        var collider = gameObject.AddComponent<BoxCollider2D>();
        var halfColumnUnit = columnCount * 0.5f;
        collider.offset = transform.TransformPoint(new Vector3(-(halfColumnUnit + ColliderWidth * 0.5f), 0, 0));
        collider.size = new Vector3(ColliderWidth, rowCount, 0);
    }
    
    private void SetupBottomCollider() {
        var collider = gameObject.AddComponent<BoxCollider2D>();
        var halfRowUnit = rowCount * 0.5f;
        collider.offset = transform.TransformPoint(new Vector3(0, -(halfRowUnit + ColliderWidth * 0.5f), 0));
        collider.size = new Vector3(columnCount, ColliderWidth, 0);
    }
    
    private void PopulateCubes() {
        var camera = Camera.main;
        var y = camera.ViewportToWorldPoint(new Vector3(0, CubeSpawnViewportHeight, 0)).y;
        var halfColumnUnit = columnCount * 0.5f;
        for (int i = 0; i < rowCount; i++) {
            var x = -(halfColumnUnit - 0.5f);
            for (int j = 0; j < columnCount; j++) {
                Instantiate(cubeRes, new Vector3(x, y, 0), Quaternion.identity, transform);
                x++;
            }
            y++;
        }
    }

    private void OnDrawGizmos() {
        DrawDebugBorder();
    }

    private void DrawDebugBorder() {
        var halfRowUnit = rowCount * 0.5f;
        var halfColumnUnit = columnCount * 0.5f;
        var topLeft = transform.TransformPoint(new Vector2(-halfColumnUnit, halfRowUnit));
        var topRight = transform.TransformPoint(new Vector2(halfColumnUnit, halfRowUnit));
        var bottomRight = transform.TransformPoint(new Vector2(halfColumnUnit, -halfRowUnit));
        var bottomLeft = transform.TransformPoint(new Vector2(-halfColumnUnit, -halfRowUnit));

        var oldColor = Gizmos.color;
        Gizmos.color = debugBorderColor;
        {
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }
        Gizmos.color = oldColor;
    }
}
