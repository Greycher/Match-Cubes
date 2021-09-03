using UnityEngine;

namespace MatchCubes {
    public class Column {
        public int ColumnIndex => _columnIndex;

        private Board _board;
        private int _columnIndex;
        private int _cubeCount;

        public Column(Board board, int columnIndex) {
            _board = board;
            _columnIndex = columnIndex;
        }

        public Cube SpawnNewCube() {
            var rowIndex = _cubeCount;
            var cube = CreateInstance();
            cube.transform.position = GetSpawnPos(rowIndex);
            cube.SetTargetPosition(GetFinalPos(rowIndex));
            RegisterCube(cube, rowIndex);
            _cubeCount++;
            return cube;
        }

        public Cube SpawnNewCubeAtCell(int rowIndex) {
            var cube = CreateInstance();
            cube.transform.position = GetFinalPos(rowIndex);
            RegisterCube(cube, rowIndex);
            _cubeCount++;
            return cube;
        }

        private Cube CreateInstance() {
            var cube = _board.CubePool.RemoveRandom();
            cube.transform.SetParent(_board.Parent);
            return cube;
        }

        private Vector3 GetSpawnPos(int rowIndex) {
            var coord = GetCellCoord(rowIndex);
            var pos = _board.CoordToSpawnPos(coord);

            if (rowIndex > 0) {
                var downCubeHeight = GetCubeHeight(rowIndex - 1);
                var cubeUnit = GameValues.Instance.Storage.cubeUnit;
                pos.y = Mathf.Max(pos.y, downCubeHeight + cubeUnit);
            }

            return pos;
        }
        
        private BoardCoordinate GetCellCoord(int rowIndex) {
            return new BoardCoordinate(rowIndex, _columnIndex);
        }

        public float GetCubeHeight(int rowIndex) {
            var cubeMap = _board.CubeMap;
            var downCube = cubeMap.GetCell(GetCellCoord(rowIndex));
            return downCube.transform.position.y;
        }

        private Vector3 GetFinalPos(int rowIndex) {
            var coord = GetCellCoord(rowIndex);
            return _board.CoordToPos(coord);
        }

        private void RegisterCube(Cube cube, int rowIndex) {
            cube.AssignToCell(this, rowIndex);
            var cubeMap = _board.CubeMap;
            cubeMap.SetCell(GetCellCoord(rowIndex), cube);
        }

        public void DestroyAtCell(int rowIndex) {
            DestroyCubeAt(rowIndex);
            ShiftCellsAtUpperRows(rowIndex);
            SpawnNewCube();
        }

        private void DestroyCubeAt(int rowIndex) {
            var cube = _board.CubeMap.GetCell(GetCellCoord(rowIndex));
            cube.FreeFromCell();
            _board.CubePool.Add(cube);
            EmptyBoardCell(rowIndex);
            _cubeCount--;
        }

        private void EmptyBoardCell(int rowIndex) {
            _board.CubeMap.SetCell(GetCellCoord(rowIndex), null);
        }

        private void ShiftCellsAtUpperRows(int rowIndex) {
            for (int i = rowIndex; i < _cubeCount; i++) {
                ShiftCellDown(i + 1);
            }
        }

        private void ShiftCellDown(int rowIndex) {
            var cubeMap = _board.CubeMap;
            var coord = GetCellCoord(rowIndex);
            var cube = cubeMap.GetCell(coord);
            var downCoord = coord.Down();
            RegisterCube(cube, downCoord.rowIndex);
            cube.SetTargetPosition(_board.CoordToPos(downCoord));
            EmptyBoardCell(rowIndex);
        }

        public void Shuffle(int rowCount) {
            //Destroying from top to bottom for avoiding any shift
            for (int rowIndex = rowCount - 1; rowIndex >= 0; rowIndex--) {
                DestroyCubeAt(rowIndex);
                SpawnNewCubeAtCell(rowIndex);
            }
        }
    }
}