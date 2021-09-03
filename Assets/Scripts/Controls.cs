using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.EventSystems;

 public class Controls : MonoBehaviour, IPointerDownHandler {
     private Dictionary<int, Cube> _cubeCache = new Dictionary<int, Cube>();

     public void OnPointerDown(PointerEventData eventData) {
         if (TryHitGameObjectAtPointedLocation(eventData.pressPosition, out GameObject gameObject)) {
             if (TryGetCube(gameObject, out Cube cube)) {
                 cube.OnPointed();
             }
         }
     }

     private bool TryHitGameObjectAtPointedLocation(Vector3 pointerPosition, out GameObject go) {
         Debug.Log("Shooting Ray");
         var camera = Camera.main;
         var ray = camera.ScreenPointToRay(pointerPosition);
         var hitInfo = Physics2D.Raycast(ray.origin, Vector2.zero);
         if (hitInfo.collider != null) {
             Debug.Log("A Game Object Hit");
             var coll = hitInfo.collider;
             var rb = coll.attachedRigidbody;
             go = rb != null ? rb.gameObject : coll.gameObject;
             return true;
         }
         
         go = null;
         return false;
     }

     private bool TryGetCube(GameObject go, out Cube cube) {
        var instanceID = go.GetInstanceID();
        if (!_cubeCache.TryGetValue(instanceID, out cube)) {
            cube = go.GetComponent<Cube>();
            _cubeCache.Add(instanceID, cube);
        }

        return cube != null;
     }
 }
