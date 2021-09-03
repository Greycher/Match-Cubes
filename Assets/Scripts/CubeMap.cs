using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeMap {
    private Board _board;
    private Cube[] _map;
    private int _rowCount;
    private int _columnCount;
    private int _shufflelimit;

    private const int ShuffleLimitInAFrame = 4;

    public void Initialize(Board board, int rowCount, int columnCount) {
        _board = board;
        _map = new Cube[rowCount * columnCount];
        _rowCount = rowCount;
        _columnCount = columnCount;
    }

    public Cube GetCell(BoardCoordinate coordinate) {
        return _map[coordinate.rowIndex * _columnCount + coordinate.columnIndex];
    }

    public void SetCell(BoardCoordinate coordinate, Cube cube) { 
        _map[coordinate.rowIndex * _columnCount + coordinate.columnIndex] = cube;
    }
    
    public void ReformChains() {
        ResetChains();
        FormChains();
    }

    public void FormChains() {
        _shufflelimit = ShuffleLimitInAFrame;
        InternalFormChains();
    }
    
    private void InternalFormChains() {
        var anyChainWithSufficientCube = false;
        var gameValues = GameValues.Instance.Storage;
        for (int i = 0; i < _map.Length; i++) {
            var cube = _map[i];
            if (cube.Chain == null) {
                var chain = FormAChain(cube);
                if (chain.Length > gameValues.minCollapsableChainLength) {
                    anyChainWithSufficientCube = true;
                }
            }
        }
        
        //Shuffle limit is just precaution due to the possibility of 
        //not having a chain with sufficient cube forever
        if (!anyChainWithSufficientCube && _shufflelimit > 0) {
            Shuffle();
            InternalFormChains();
        }
    }

    private void Shuffle() {
        _board.Shuffle();
        _shufflelimit--;
        Debug.Log("Shuffled");
    }

    private CubeChain FormAChain(Cube cube) {
        var newChain = new CubeChain(this);
        newChain.Add(cube);
        LookAdjacentCubesToExpandChain(cube);
        newChain.UpdateCubeVisuals();
        return newChain;
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
        var cube = GetCell(coord);
        if (cube.Chain != null) return;

        if (cubeType == cube.Type) {
            chain.Add(cube);
            LookAdjacentCubesToExpandChain(cube);
        }
    }

    private void ResetChains() {
        for (int i = 0; i < _map.Length; i++) {
            var cube = _map[i];
            cube.DetachFromChain();
        }
    }
}
