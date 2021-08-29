using System;
using UnityEngine;

public class Cube : MonoBehaviour {
    
    [SerializeField] private CubeType cubeType;

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
