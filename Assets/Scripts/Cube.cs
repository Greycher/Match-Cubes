using System;
using UnityEngine;

public class Cube : MonoBehaviour {
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CubeType cubeType;
    [SerializeField] private FakeGravityModule fakeGravityModule;

    private void FixedUpdate() {
        fakeGravityModule.ApplyGravity(rb);
    }

    [Serializable]
    private enum CubeType {
        Yellow,
        Blue,
        Green,
        Pink,
        Purple,
        Red
    }
}
