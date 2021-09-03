using System.Collections.Generic;
using UnityEngine;

namespace MatchCubes {
    public class Board : MonoBehaviour {
        [SerializeField] private Cube[] cubeResources;
        [SerializeField] private int rowCount = 10;
        [SerializeField] private int columnCount = 12;
        [SerializeField] private Color debugBorderColor = Color.green;

        private const float CubeSpawnViewportHeight = 1.05f;
        private const int PopulationPerCube = 100;

        public Transform Parent => transform;
        public CubeMap CubeMap => _cubeMap;
        public GameObjectPool<Cube> CubePool => _cubePool;

        private Column[] _columns;
        private CubeMap _cubeMap;
        private GameObjectPool<Cube> _cubePool;
        private List<CubeChain> _currentCubeChains = new List<CubeChain>();

        private void Awake() {
            InitializeCubePool();
            InitializeCubeMap();
            CreateColumnsAndSpawnCubes();
            _cubeMap.FormChains();
        }

        private void InitializeCubePool() {
            CreateCubePool();
            PopulateCubePool();
        }
        
        private void CreateCubePool() {
            var parent = new GameObject("Cube Pool").transform;
            parent.SetParent(Parent);
            _cubePool = new GameObjectPool<Cube>(parent);
        }

        private void PopulateCubePool() {
            var cubeCount = cubeResources.Length;
            for (int i = 0; i < cubeCount; i++) {
                for (int j = 0; j < PopulationPerCube; j++) {
                    var cubeResource = cubeResources[i];
                    _cubePool.Add(Instantiate(cubeResource));
                }
            }
        }
        
        private void InitializeCubeMap() {
            _cubeMap = new CubeMap();
            _cubeMap.Initialize(this, rowCount, columnCount);
        }
        
        private void CreateColumnsAndSpawnCubes() {
            _columns = new Column[columnCount];
            for (int i = 0; i < columnCount; i++) {
                 var column = new Column(this, i);
                 for (int j = 0; j < rowCount; j++) {
                     column.SpawnNewCubeAtCell(j);
                 }
                 _columns[i] = column;
            }
        }
        
        public Vector3 CoordToSpawnPos(BoardCoordinate boardCoordinate) {
            var worldPos = Vector2.zero;
            worldPos.x = CalculateFirstColumnX() + boardCoordinate.columnIndex;
            worldPos.y = CubeSpawnViewportHeightToWorldHeight();
            return worldPos;
        }


        private float CubeSpawnViewportHeightToWorldHeight() {
            var camera = Camera.main;
            return camera.ViewportToWorldPoint(new Vector3(0, CubeSpawnViewportHeight, 0)).y;
        }

        public Vector3 CoordToPos(BoardCoordinate boardCoordinate) {
            var worldPos = Vector2.zero;
            worldPos.x = CalculateFirstColumnX() + boardCoordinate.columnIndex;
            worldPos.y = CalculateFirstRowY() + boardCoordinate.rowIndex;
            return worldPos;
        }

        private float CalculateFirstColumnX() {
            var halfColumnUnit = columnCount * 0.5f;
            var parentPos = Parent.position;
            var cubeUnit = GameValues.Instance.Storage.cubeUnit;
            var halfCubeUnit = cubeUnit * 0.5f;
            var firstColX = parentPos.x - (halfColumnUnit - halfCubeUnit);
            return firstColX;
        }

        private float CalculateFirstRowY() {
            var halfRowUnit = rowCount * 0.5f;
            var parentPos = Parent.position;
            var cubeUnit = GameValues.Instance.Storage.cubeUnit;
            var halfCubeUnit = cubeUnit * 0.5f;
            var firstRowPos = parentPos.y - (halfRowUnit - halfCubeUnit);
            return firstRowPos;
        }
        
        public void Shuffle() {
            for (int i = 0; i < columnCount; i++) {
                _columns[i].Shuffle(rowCount);
            }
        }
        
        public CubeChain GetNewChain() {
            var chain = CubeChainStack.Instance.PopOrCreate();
            chain.onChainCollapsed -= OnChainCollapse;
            chain.onChainCollapsed += OnChainCollapse;
            _currentCubeChains.Add(chain);
            return chain;
        }
        
        public void DestroyAllChains() {
            for (int i = 0; i < _currentCubeChains.Count; i++) {
                DestroyChainAt(i);
            }

            _currentCubeChains.Clear();
        }

        private void DestroyChainAt(int i) {
            var chain = _currentCubeChains[i];
            chain.onChainCollapsed -= OnChainCollapse;
            chain.DetachCubes();
            CubeChainStack.Instance.Push(chain);
        }

        private void OnChainCollapse() {
            DestroyAllChains();
            _cubeMap.ReformChains();
        }

        private void OnDrawGizmos() {
            DrawDebugBorder();
        }

        private void DrawDebugBorder() {
            var halfRowUnit = rowCount * 0.5f;
            var halfColumnUnit = columnCount * 0.5f;
            var topLeft = transform.TransformPoint(new Vector2(-halfColumnUnit, halfRowUnit));
            var topRight = transform.TransformPoint(new Vector2(halfColumnUnit, halfRowUnit));
            var bottomRight = transform.TransformPoint(new Vector2(halfColumnUnit, -halfRowUnit));
            var bottomLeft = transform.TransformPoint(new Vector2(-halfColumnUnit, -halfRowUnit));

            var oldColor = Gizmos.color;
            Gizmos.color = debugBorderColor;
            {
                Gizmos.DrawLine(topLeft, topRight);
                Gizmos.DrawLine(topRight, bottomRight);
                Gizmos.DrawLine(bottomRight, bottomLeft);
                Gizmos.DrawLine(bottomLeft, topLeft);
            }
            Gizmos.color = oldColor;
        }
    }
}
