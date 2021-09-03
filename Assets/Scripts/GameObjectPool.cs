using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> where T : Component {
    private List<T> _pool;
    private readonly Transform _parent;

    public GameObjectPool() : this(null) { }

    public GameObjectPool(Transform parent) {
        _pool = new List<T>();
        _parent = parent;
    }

    public void Push(T t) {
        DeActivateAttachedGameObject(t);
        SetParent(t);
        _pool.Add(t);
    }

    private T Pop(int i) {
        var t = _pool[i];
        var lastIndex = _pool.Count - 1;
        _pool[i] = _pool[lastIndex];
        _pool.RemoveAt(lastIndex);
        ActivateAttachedGameObject(t);
        return t;
    }

    public T PopRandom() {
        return Pop(Random.Range(0, _pool.Count));
    }
    
    private static void DeActivateAttachedGameObject(T t) {
        var go = t.gameObject;
        go.SetActive(false);
    }
    
    private static void ActivateAttachedGameObject(T t) {
        var go = t.gameObject;
        go.SetActive(true);
    }
    
    private void SetParent(T t) {
        var tr = t.transform;
        tr.SetParent(_parent);
    }
}
