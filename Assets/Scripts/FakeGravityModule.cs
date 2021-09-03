using System;
using UnityEngine;

namespace MatchCubes {
    [Serializable]
    public struct FakeGravityModule {
        [SerializeField] private AnimationCurve gravityMagnitudeInTime;

        private float _curveEndTime;
        private float _time;

        public void Initialize() {
            _curveEndTime = CalculateCurveLength();
        }

        private float CalculateCurveLength() {
            var keyFrames = gravityMagnitudeInTime.keys;
            return keyFrames[keyFrames.Length - 1].time;
        }

        public void Accelerate(float deltaTime) {
            _time = Mathf.Min(_time + deltaTime, _curveEndTime);
        }

        public float GetGravitySpeed() {
            return gravityMagnitudeInTime.Evaluate(_time);
        }

        public void ResetSpeed() {
            _time = 0;
        }
    }
}