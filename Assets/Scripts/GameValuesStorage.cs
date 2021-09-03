using UnityEngine;

namespace MatchCubes {
    [CreateAssetMenu(fileName = "Game Values Storage", menuName = "Scriptable Objects/Game Values Storage")]
    public class GameValuesStorage : ScriptableObject {
        public int minCollapsableChainLength;
        public int[] cubeVisualBreakpoints;
        public int cubeUnit = 1;
    }
}

