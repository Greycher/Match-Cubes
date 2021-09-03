using UnityEngine;

namespace MatchCubes {
    public class Cube : MonoBehaviour {
        [SerializeField] private SpriteRenderer renderer;
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private CubeType type;
        [SerializeField] private FakeGravityModule fakeGravityModule;

        private const int DefaultVisualIndex = 0;

        public CubeType Type => type;
        public CubeChain Chain => _chain;
        public bool IsPositioned => _state == State.Positioned;

        private State _state = State.Positioned;
        private Column _column;
        private CubeChain _chain;
        private int _rowIndex;
        private Vector3 _targetPosition;

        private void Awake() {
            fakeGravityModule.Initialize();
        }

        public void AssignToCell(Column column, int rowIndex) {
            _column = column;
            _rowIndex = rowIndex;
        }

        public void FreeFromCell() {
            _column = null;
            _rowIndex = -1;
            ResetSpeed();
        }

        public void SetTargetPosition(Vector3 targetPos) {
            _targetPosition = targetPos;
            _state = State.Positioning;
        }

        private void Update() {
            if (_state == State.Positioning) {
                CheckIfTargetReached(transform.position);
                CloseToTarget(Time.deltaTime);
            }
        }
        
        private void CheckIfTargetReached(Vector3 pos) {
            if (Vector3.SqrMagnitude(_targetPosition - pos) < Mathf.Epsilon * Mathf.Epsilon) {
                ResetSpeed();
            }
        }

        private void CloseToTarget(float deltaTime) {
            var gravitySpeed = AccelerateAndGetGravity(deltaTime);
            var pos = transform.position;
            pos = Vector3.MoveTowards(pos, _targetPosition, gravitySpeed * deltaTime);
            
            //Make sure the cube doesn't get a head of
            //the one in the lower
            if (_rowIndex > 0) {
                var downCubeHeight = _column.GetCubeHeight(_rowIndex - 1);
                var cubeUnit = GameValues.Instance.Storage.cubeUnit;
                pos.y = Mathf.Max(pos.y, downCubeHeight + cubeUnit);
            }

            transform.position = pos;
        }

        private float AccelerateAndGetGravity(float deltaTime) {
            fakeGravityModule.Accelerate(deltaTime);
            return fakeGravityModule.GetGravitySpeed();
        }

        private void ResetSpeed() {
            fakeGravityModule.ResetSpeed();
            _state = State.Positioned;
        }

        public BoardCoordinate GetCoord() {
            return new BoardCoordinate(_rowIndex, _column.ColumnIndex);
        }

        public void UpdateVisual(int visualIndex) {
            renderer.sprite = sprites[visualIndex];
        }

        public void OnPointed() {
            _chain.TryCollapse();
        }

        public void Collapse() {
            _column.DestroyAtCell(_rowIndex);
        }

        public void AttachToChain(CubeChain chain) {
            _chain = chain;
        }

        public void DetachFromChain() {
            _chain = null;
            UpdateVisual(DefaultVisualIndex);
        }

        private void OnDrawGizmosSelected() {
            var cubes = _chain.Cubes;
            for (int i = 0; i < cubes.Count; i++) {
                var tr = cubes[i].transform;
                Gizmos.DrawSphere(tr.position, 0.3f);
            }
        }

        private enum State {
            Positioning,
            Positioned
        }
    }
}
