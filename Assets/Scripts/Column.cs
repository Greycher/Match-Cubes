using UnityEngine;

public class Column {
    public int ColumnIndex => _columnIndex;

    private Board _board;
    private int _columnIndex;
    private int _cubeCount;
    
    public Column(Board board, int columnIndex) {
        _board = board;
        _columnIndex = columnIndex;
    }
    
    public Cube SpawnCube() {
        var rowIndex = _cubeCount;
        var cube = SpawnNewCube(rowIndex);
        RegisterCube(cube, rowIndex);
        _cubeCount++;
        return cube;
    }
    
    public Cube SpawnCubeAtCell() {
        var rowIndex = _cubeCount;
        var cube = SpawnNewCubeAtCell(rowIndex);
        RegisterCube(cube, rowIndex);
        _cubeCount++;
        return cube;
    }

    private Cube SpawnNewCube(int rowIndex) {
        var cube = _board.CubePool.PopRandom();
        cube.transform.SetParent(_board.Parent);
        cube.transform.position = GetSpawnPos(rowIndex);
        cube.SetTargetPosition(GetFinalPos(rowIndex));
        return cube;
    }
    
    private Cube SpawnNewCubeAtCell(int rowIndex) {
        var cube = _board.CubePool.PopRandom();
        cube.transform.SetParent(_board.Parent);
        cube.transform.position = GetFinalPos(rowIndex);
        return cube;
    }

    private Vector3 GetSpawnPos(int rowIndex) {
        var coord = new BoardCoordinate(rowIndex, _columnIndex);
        var pos = _board.CoordToSpawnPos(coord);
        
        if (_cubeCount > 0) {
            var cubeMap = _board.CubeMap;
            var downCube = cubeMap.GetCell(coord.Down());
            var downCubeHeight = downCube.transform.position.y;
            pos.y = Mathf.Max(pos.y, downCubeHeight + 1);
        }
        return pos;
    }
    
    private Vector3 GetFinalPos(int rowIndex) {
        var coord = new BoardCoordinate(rowIndex, _columnIndex);
        return _board.CoordtoPos(coord);
    }
    
    private void RegisterCube(Cube cube, int rowIndex) {
        cube.AssignToCell(this, rowIndex);
        var cubeMap = _board.CubeMap;
        cubeMap.SetCell(cube.GetCoord(), cube);
    }

    public void OnCubeDestroy(Cube cube) {
        DestroyCube(cube);
        ShiftCubesAtUpperRows(cube.RowIndex);
        SpawnCube();
    }

    private void DestroyCubeAt(BoardCoordinate coord) {
        DestroyCube(_board.CubeMap.GetCell(coord));
    }
    
    private void DestroyCube(Cube cube) {
        EmptyBoardCell(cube.Coord);
        PushToPool(cube);
        _cubeCount--;
    }

    private void PushToPool(Cube cube) {
        _board.CubePool.Push(cube);
    }

    private void EmptyBoardCell(BoardCoordinate coord) {
        _board.CubeMap.SetCell(coord, null);
    }

    private void ShiftCubesAtUpperRows(int emptyRowIndex) {
        for (int i = emptyRowIndex; i < _cubeCount - 1; i++) {
            var nextRowIndex = i + 1;
            ShiftCellDown(new BoardCoordinate(nextRowIndex, _columnIndex));
        }
    }

    private void ShiftCellDown(BoardCoordinate coord) {
        var cubeMap = _board.CubeMap;
        var cube = cubeMap.GetCell(coord);
        cubeMap.SetCell(coord.Down(), cube);
        EmptyBoardCell(coord);
    }

    public void Shuffle(int rowCount) {
        //Destroying from top to bottom for avoiding any shift
        for (int j = rowCount; j >= 0; j--) {
            DestroyCubeAt(new BoardCoordinate(j, _columnIndex));
            SpawnCubeAtCell();
        }
    }
}
