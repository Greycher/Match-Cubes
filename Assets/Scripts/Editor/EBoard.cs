using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace MatchCubes {
    [CustomEditor(typeof(Board))]
    public class EBoard : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if (Application.isPlaying) {
                if (GUILayout.Button("Shuffle")) {
                    Shuffle();
                }

            }
        }

        private void Shuffle() {
            var board = target as Board;
            board.Shuffle();
            board.CubeMap.ReformChains();
        }
    }
}
