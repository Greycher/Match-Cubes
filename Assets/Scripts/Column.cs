using TMPro;
using UnityEngine;

public class Column {
    
    private const float CubeSpawnViewportHeight = 1.1f;

    public uint ColumnIndex => _columnIndex;
    
    private Board _board;
    private Cube _lastCreatedCube;
    private uint _columnIndex;

    public Column(Board board, uint columnIndex) {
        _board = board;
        _columnIndex = columnIndex;
    }

    public Cube SpawnCube() {
        var parent = _board.Parent;
        var halfColumnUnit = _board.ColumnCount * 0.5f;
        var firstRowX = parent.position.x - (halfColumnUnit - 0.5f);
        
        var position = Vector2.zero;
        position.x = firstRowX + _columnIndex;
        position.y = GetInitialHeight();
        
        _lastCreatedCube = Object.Instantiate(_board.GetRandomCubeResource(), position, Quaternion.identity, parent);
        _lastCreatedCube.Board = _board;
        return _lastCreatedCube;
    }

    private float GetInitialHeight() {
        var y = Mathf.NegativeInfinity;
        if (_lastCreatedCube != null) {
            y = _lastCreatedCube.transform.position.y + 1;
        }
        return Mathf.Max(y, GetInitialHeightAboveCamera());
    }

    private float GetInitialHeightAboveCamera() {
        var camera = Camera.main;
        return camera.ViewportToWorldPoint(new Vector3(0, CubeSpawnViewportHeight, 0)).y;
    }
}
