using System;
using UnityEngine;

public class Cube : MonoBehaviour {
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private CubeType type;
    [SerializeField] private FakeGravityModule fakeGravityModule;
    
    public CubeType Type => type;

    public CubeChain Chain {
        get => _chain;
        set => _chain = value;
    }

    private State _state = State.Positioned;
    private Column _column;
    private CubeChain _chain;
    private uint _rowIndex;
    private Vector3 _targetPosition;

    private void Awake() {
        fakeGravityModule.Initialize();
    }

    public void AssignToColumn(Column column, uint rowIndex) {
        _column = column;
        _rowIndex = rowIndex;
        UpdateTargetPosition();
    }

    private void UpdateTargetPosition() {
        _targetPosition = _column.RowIndexToWorldPos(_rowIndex);
        _state = State.Positioning;
    }

    private void Update() {
        if (_state == State.Positioning) {
            CloseToTarget();
        }
    }

    private void CloseToTarget() {
        var deltaTime = Time.deltaTime;
        var gravitySpeed = AccelerateAndGetGravity(deltaTime);
        
        var pos = transform.position;
        pos = Vector3.MoveTowards(pos, _targetPosition, gravitySpeed * deltaTime);
        transform.position = pos;

        CheckIfTargetReached(pos);
    }

    private float AccelerateAndGetGravity(float deltaTime) {
        fakeGravityModule.Accelerate(deltaTime);
        return fakeGravityModule.GetGravitySpeed();
    }

    private void CheckIfTargetReached(Vector3 pos) {
        if (Vector3.SqrMagnitude(_targetPosition - pos) < Mathf.Epsilon * Mathf.Epsilon) {
            _state = State.Positioned;
            fakeGravityModule.ResetSpeed();
        }
    }
    
    public BoardCoordinate GetCoord() {
        return new BoardCoordinate(_rowIndex, _column.ColumnIndex);
    }
    
    public void UpdateVisual(int visualIndex) {
        renderer.sprite = sprites[visualIndex];
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
