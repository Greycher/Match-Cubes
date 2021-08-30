using UnityEngine;

public class Column {
    public uint ColumnIndex => _columnIndex;
    
    private Board _board;
    private uint _columnIndex;
    private uint _elementCount;

    public Column(Board board, uint columnIndex) {
        _board = board;
        _columnIndex = columnIndex;
    }
    
    public Cube SpawnCube() {
        var worldPos = _board.BoardCoordToSpawnWorldPos(new Board.BoardCoordinate(_elementCount, _columnIndex));
        var cube = Object.Instantiate(_board.GetRandomCubeResource(), worldPos, Quaternion.identity, _board.Parent);
        var rowIndex = _elementCount++;
        cube.AssignToColumn(this, rowIndex);
        return cube;
    }

    public Vector3 RowIndexToWorldPos(uint rowIndex) {
        return _board.BoardCoordToWorldPos(new Board.BoardCoordinate(rowIndex, _columnIndex));
    }
}
