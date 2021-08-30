public class CubeMap {
    private Cube[] _map;
    private uint _rowCount;
    private uint _columnCount;

    public void Initialize(uint rowCount, uint columnCount) {
        _map = new Cube[rowCount * columnCount];
        _rowCount = rowCount;
        _columnCount = columnCount;
    }

    public Cube GetCube(BoardCoordinate coordinate) {
        return _map[coordinate.rowIndex * _columnCount + coordinate.columnIndex];
    }

    public void SetCube(BoardCoordinate coordinate, Cube cube) { 
        _map[coordinate.rowIndex * _columnCount + coordinate.columnIndex] = cube;
    }

    public void FormChains() {
        for (int i = 0; i < _map.Length; i++) {
            var cube = _map[i];
            if (cube.Chain == null) {
                var newChain = new CubeChain();
                newChain.Add(cube);
                LookAdjacentCubesToExpandChain(cube);
                newChain.UpdateCubeVisuals();
            }
        }
    }

    private void LookAdjacentCubesToExpandChain(Cube cube) {
        var coord = cube.GetCoord();
        var cubeType = cube.Type;
        
        var lastRowIndex = _rowCount - 1;
        if (coord.rowIndex < lastRowIndex) {
            AddCubeToChainIfSameType(cube.Chain, coord.Up(), cubeType);
        }
        
        var lastColumnIndex = _columnCount - 1;
        if (coord.columnIndex < lastColumnIndex) {
            AddCubeToChainIfSameType(cube.Chain, coord.Right(), cubeType);
        }
        
        if (coord.rowIndex > 0) {
            AddCubeToChainIfSameType(cube.Chain, coord.Down(), cubeType);
        }
        
        if (coord.columnIndex > 0) {
            AddCubeToChainIfSameType(cube.Chain, coord.Left(), cubeType);
        }
    }
    
    private void AddCubeToChainIfSameType(CubeChain chain, BoardCoordinate coord, CubeType cubeType) {
        var cube = GetCube(coord);
        if (cube.Chain != null) return;

        if (cubeType == cube.Type) {
            chain.Add(cube);
            LookAdjacentCubesToExpandChain(cube);
        }
    }
}
