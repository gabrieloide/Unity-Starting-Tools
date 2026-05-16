using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.Pool
{
    public class ObjectPooler : MonoBehaviour
    {
        private static ObjectPooler _instance;
        public static ObjectPooler Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject("ObjectPooler");
                    _instance = obj.AddComponent<ObjectPooler>();
                    DontDestroyOnLoad(obj);
                }
                return _instance;
            }
        }

        private Dictionary<string, Queue<GameObject>> _poolDictionary = new Dictionary<string, Queue<GameObject>>();

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (prefab == null)
            {
                Debug.LogWarning("[ObjectPooler] Prefab is null.");
                return null;
            }

            string poolKey = prefab.name;

            if (!_poolDictionary.ContainsKey(poolKey))
            {
                _poolDictionary.Add(poolKey, new Queue<GameObject>());
            }

            GameObject objToSpawn;

            if (_poolDictionary[poolKey].Count > 0)
            {
                objToSpawn = _poolDictionary[poolKey].Dequeue();
                
                if (objToSpawn == null)
                {
                    objToSpawn = Instantiate(prefab);
                    objToSpawn.name = poolKey;
                }
            }
            else
            {
                objToSpawn = Instantiate(prefab);
                objToSpawn.name = poolKey;
            }

            objToSpawn.transform.SetParent(parent);
            objToSpawn.transform.SetPositionAndRotation(position, rotation);
            objToSpawn.SetActive(true);

            return objToSpawn;
        }

        public void Despawn(GameObject obj)
        {
            if (obj == null) return;

            string poolKey = obj.name;
            obj.SetActive(false);
            obj.transform.SetParent(transform);

            if (!_poolDictionary.ContainsKey(poolKey))
            {
                _poolDictionary.Add(poolKey, new Queue<GameObject>());
            }

            _poolDictionary[poolKey].Enqueue(obj);
        }
    }
}
