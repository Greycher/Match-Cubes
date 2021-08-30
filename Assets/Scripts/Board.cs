using UnityEngine;

public class Board : MonoBehaviour {
    [SerializeField] private Cube[] cubeResources;
    [SerializeField] private uint rowCount = 10;
    [SerializeField] private uint columnCount = 12;
    [SerializeField] private Color debugBorderColor = Color.green;

    private const float CubeSpawnViewportHeight = 1.1f;

    public Transform Parent => transform;

    private Column[] _columns;
    
    private void Awake() {
        PopulateCubes();
    }
    
    private void PopulateCubes() {
        _columns = new Column[columnCount];
        for (uint i = 0; i < columnCount; i++) {
            var columnIndex = i;
            var column = new Column(this, columnIndex);
            for (uint j = 0; j < rowCount; j++) {
                column.SpawnCube();
            }
            _columns[i] = column;
        }
    }

    public Cube GetRandomCubeResource() {
        var rnd = Random.Range(0, cubeResources.Length);
        return cubeResources[rnd];
    }
    
    public Vector3 BoardCoordToSpawnWorldPos(BoardCoordinate boardCoordinate) {
        var halfColumnUnit = columnCount * 0.5f;
        var parentPos = Parent.position;
        var firstRowWorldPos = parentPos.x - (halfColumnUnit - 0.5f);
        
        var worldPos = Vector2.zero;
        worldPos.x = firstRowWorldPos + boardCoordinate.columnIndex;
        worldPos.y = CalculateInitialHeightAboveCamera() + boardCoordinate.rowIndex;
        return worldPos;
    }
    
    private float CalculateInitialHeightAboveCamera() {
        var camera = Camera.main;
        return camera.ViewportToWorldPoint(new Vector3(0, CubeSpawnViewportHeight, 0)).y;
    }
    
    public Vector3 BoardCoordToWorldPos(BoardCoordinate boardCoordinate) {
        var worldPos = Vector2.zero;
        worldPos.x = CalculateColumnHorizontalWorldPos(boardCoordinate.columnIndex);
        worldPos.y = CalculateRowVerticalWorldPos(boardCoordinate.rowIndex);
        return worldPos;
    }

    private float CalculateColumnHorizontalWorldPos(uint columnIndex) {
        var halfColumnUnit = columnCount * 0.5f;
        var parentPos = Parent.position;
        var firstColPos = parentPos.x - (halfColumnUnit - 0.5f);
        return firstColPos + columnIndex;
    }
    
    private float CalculateRowVerticalWorldPos(uint rowIndex) {
        var halfRowUnit = rowCount * 0.5f;
        var parentPos = Parent.position;
        var firstRowPos = parentPos.y - (halfRowUnit - 0.5f);
        return firstRowPos + rowIndex;
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
    
    public struct BoardCoordinate {
        public uint rowIndex;
        public uint columnIndex;

        public BoardCoordinate(uint rowIndex, uint columnIndex) {
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
        }
    }
}
