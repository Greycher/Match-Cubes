using System.Collections.Generic;

public class CubeChain {
    public List<Cube> Cubes => cubes;
    public int Length => cubes.Count;
    
    private List<Cube> cubes = new List<Cube>();
    private readonly CubeMap _cubeMap;

    public CubeChain(CubeMap cubeMap) {
        _cubeMap = cubeMap;
    }
    
    public void Add(Cube cube) {
        cube.AttachToChain(this);
        cubes.Add(cube);
    }

    public void UpdateCubeVisuals() {
        var storage = GameValues.Instance.Storage;
        var breakPoints = storage.cubeVisualBreakpoints;
        var chainLength = cubes.Count;
        int breakPointIndex = 0;
        for (int i = breakPoints.Length - 1; i >= 0; i--) {
            var minChainLength = breakPoints[i];
            if (chainLength >= minChainLength) {
                breakPointIndex = i;
                break;
            }
        }

        for (int i = 0; i < cubes.Count; i++) {
            cubes[i].UpdateVisual(breakPointIndex);
        }
    }

    public void OnPointed() {
        var storage = GameValues.Instance.Storage;
        var chainLength = cubes.Count;
        if (chainLength >= storage.minCollapsableChainLength) {
            CollapseAndReformChains();
        }
    }

    private void CollapseAndReformChains() {
        for (int i = 0; i < cubes.Count; i++) {
            cubes[i].Collapse();
        }
        _cubeMap.ReformChains();
    }
}
