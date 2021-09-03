using System.Collections.Generic;
using UnityEngine;

namespace MatchCubes {
    public class GameObjectPool<T> where T : Component {
        private List<T> _pool;
        private readonly Transform _parent;

        public GameObjectPool() : this(null) {
        }

        public GameObjectPool(Transform parent) {
            _pool = new List<T>();
            _parent = parent;
        }

        public void Add(T t) {
            DeActivateAttachedGameObject(t);
            SetParent(t);
            _pool.Add(t);
        }
        
        private static void DeActivateAttachedGameObject(T t) {
            var go = t.gameObject;
            go.SetActive(false);
        }
        
        private void SetParent(T t) {
            var tr = t.transform;
            tr.SetParent(_parent);
        }
        
        public T RemoveRandom() {
            return RemoveAt(Random.Range(0, _pool.Count));
        }

        private T RemoveAt(int i) {
            var t = _pool[i];
            var lastIndex = _pool.Count - 1;
            _pool[i] = _pool[lastIndex];
            _pool.RemoveAt(lastIndex);
            ActivateAttachedGameObject(t);
            return t;
        }

        private static void ActivateAttachedGameObject(T t) {
            var go = t.gameObject;
            go.SetActive(true);
        }
    }
}
