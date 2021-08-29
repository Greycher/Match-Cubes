using System;
using UnityEngine;

[Serializable]
public struct FakeGravityModule {
    [SerializeField] private float gravitySpeed;
    [SerializeField] private Vector2 verticalSpeedBoundaries;

    public void ApplyGravity(Rigidbody2D rb) {
        var vel = rb.velocity;
        vel.y += gravitySpeed * Time.fixedDeltaTime;
        vel.y = Mathf.Clamp(vel.y, verticalSpeedBoundaries.x, verticalSpeedBoundaries.y);
        rb.velocity = vel;
    }
}
