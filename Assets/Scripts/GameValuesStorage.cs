using UnityEngine;

[CreateAssetMenu(fileName = "Game Values Storage", menuName="Scriptable Objects/Game Values Storage")]
public class GameValuesStorage : ScriptableObject {
    public uint clusterBarrage;
    public uint[] cubeVisualBreakpoints;
}
