using System.Collections.Generic;

public class CubeChain {
    private List<Cube> cubes = new List<Cube>();

    public List<Cube> Cubes => cubes;
    
    public void Add(Cube cube) {
        cubes.Add(cube);
        cube.Chain = this;
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
}
