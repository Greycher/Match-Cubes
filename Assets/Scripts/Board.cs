using UnityEngine;

public class Board : MonoBehaviour {
    [SerializeField] private Cube[] cubeResources;
    [SerializeField] private int rowCount = 10;
    [SerializeField] private int columnCount = 12;
    [SerializeField] private Color debugBorderColor = Color.green;

    private const float CubeSpawnViewportHeight = 1.05f;
    private const int PopulationPerCube = 100;

    public Transform Parent => transform;
    public CubeMap CubeMap => _cubeMap;
    public GameObjectPool<Cube> CubePool => _cubePool;

    private Column[] _columns;
    private CubeMap _cubeMap = new CubeMap();
    private GameObjectPool<Cube> _cubePool;
    private GameValuesStorage _gameValuesStorage;

    private void Awake() {
        _cubeMap.Initialize(this, rowCount, columnCount);
        PopulateCubePool();
        CreateColumns();
        CreateColumns();
        SpawnCubes();
        _cubeMap.FormChains();
    }

    private void PopulateCubePool() {
        CreateCubePool();
        var cubeCount = cubeResources.Length;
        for (int i = 0; i < cubeCount; i++) {
            for (int j = 0; j < PopulationPerCube; j++) {
                var cubeResource = cubeResources[i];
                _cubePool.Push(Instantiate(cubeResource));
            }
        }
    }

    private void CreateCubePool() {
        var parent = new GameObject("Cube Pool").transform;
        parent.SetParent(Parent);
        _cubePool = new GameObjectPool<Cube>(parent);
    }

    private void SpawnCubes() {
        for (int i = 0; i < columnCount; i++) {
            for (int j = 0; j < rowCount; j++) {
                _columns[i].SpawnCube();
            }
        }
    }

    public void Shuffle() {
        for (int i = 0; i < columnCount; i++) {
            _columns[i].Shuffle(rowCount);
        }
    }

    private void CreateColumns() {
        _columns = new Column[columnCount];
        for (int i = 0; i < columnCount; i++) {
            _columns[i] = new Column(this, i);
        }
    }

    public Vector3 CoordToSpawnPos(BoardCoordinate boardCoordinate) {
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
    
    public Vector3 CoordtoPos(BoardCoordinate boardCoordinate) {
        var worldPos = Vector2.zero;
        worldPos.x = CalculateColumnHorizontalWorldPos(boardCoordinate.columnIndex);
        worldPos.y = CalculateRowVerticalWorldPos(boardCoordinate.rowIndex);
        return worldPos;
    }

    private float CalculateColumnHorizontalWorldPos(int columnIndex) {
        var halfColumnUnit = columnCount * 0.5f;
        var parentPos = Parent.position;
        var firstColPos = parentPos.x - (halfColumnUnit - 0.5f);
        return firstColPos + columnIndex;
    }
    
    private float CalculateRowVerticalWorldPos(int rowIndex) {
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
}
