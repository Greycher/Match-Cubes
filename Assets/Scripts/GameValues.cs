using UnityEngine;

public class GameValues : MonoBehaviour {
    [SerializeField] private GameValuesStorage storage;

    public static GameValues Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<GameValues>();
            }
            return _instance;
        }
    }

    public GameValuesStorage Storage => storage;

    private static GameValues _instance;

    private void Awake() {
        _instance = this;
    }
}
