using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Items
{
    public class ObjectPooler : MonoBehaviour
    {
        [Serializable]
        public class Pool
        {
            public string tag;
            public List<GameObject> prefab;
            public int size;
        }

        public List<Pool> pools;

        public Dictionary<string, Queue<GameObject>> poolDictionary;

        private void Start()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (var pool in pools)
            {
                var objectPool = new Queue<GameObject>();

                for (var i = 0; i < pool.size; i++)
                {
                    //loop through the GameObject list inside the pool and instantiate each
                    foreach (var o in pool.prefab)
                    {
                        var obj = Instantiate(o);
                        obj.SetActive(false);
                        objectPool.Enqueue(obj);
                    }
                }

                poolDictionary.Add(pool.tag, objectPool);
            }
        }
    }
}