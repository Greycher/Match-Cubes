using System;
using System.Collections.Generic;

namespace MatchCubes {

    public class CubeChain {
        public List<Cube> Cubes => cubes;
        public int Length => cubes.Count;

        private List<Cube> cubes = new List<Cube>();
        public Action onChainCollapsed;

        public void Add(Cube cube) {
            cube.AttachToChain(this);
            cubes.Add(cube);
        }

        public void UpdateCubeVisuals() {
            var breakPointIndex = DetectVisualIndex();
            for (int i = 0; i < cubes.Count; i++) {
                cubes[i].UpdateVisual(breakPointIndex);
            }
        }

        private int DetectVisualIndex() {
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

            return breakPointIndex;
        }

        public bool TryCollapse() {
            var storage = GameValues.Instance.Storage;
            var chainLength = cubes.Count;
            if (chainLength >= storage.minCollapsableChainLength) {
                if (AreAllCubesPositioned()) {
                    Collapse();
                    return true;
                }
            }

            return false;
        }

        private bool AreAllCubesPositioned() {
            for (int i = 0; i < cubes.Count; i++) {
                var cube = cubes[i];
                if (!cube.IsPositioned) {
                    return false;
                }
            }

            return true;
        }

        private void Collapse() {
            for (int i = 0; i < cubes.Count; i++) {
                cubes[i].Collapse();
            }

            onChainCollapsed?.Invoke();
        }

        public void DetachCubes() {
            for (int i = 0; i < cubes.Count; i++) {
                cubes[i].DetachFromChain();
            }
        }

        public void Reset() {
            cubes.Clear();
        }
    }
}
