using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Skyunion
{
    internal class ObjectPool
    {
        private Stack<GameObject> m_pool = new Stack<GameObject>();

        private string m_PoolObjName = string.Empty;

        private int m_spawn_number;

        private int m_max_spawn_number = ObjectPoolMgr.m_default_max_spawn_number;

        private float m_last_despawn_time = 3.40282347E+38f;

        public float lastDespawnTime
        {
            get
            {
                return this.m_last_despawn_time;
            }
            set
            {
                this.m_last_despawn_time = value;
            }
        }

        public int avaliableCount
        {
            get
            {
                return this.m_pool.Count;
            }
            set
            {
            }
        }

        public void Spawn(string ResourceName, Action<GameObject> action)
        {
            if (this.m_PoolObjName == string.Empty)
            {
                this.m_PoolObjName = ResourceName;
            }
            Spawn(action);
        }

        private void Spawn(Action<GameObject> action)
        {
            this.lastDespawnTime = 3.40282347E+38f;
            if (this.m_pool.Count > 0)
            {
                GameObject gameObject = this.m_pool.Pop();
                gameObject.transform.SetParent(null);
                gameObject.SetActive(true);
                gameObject.GetComponent<ObjectPoolItem>().OnObjectSpawn();
                action?.Invoke(gameObject);
                return;
            }
            //> 先不管上限
            //if (this.m_max_spawn_number >= 0 && this.m_spawn_number >= this.m_max_spawn_number)
            //{
            //    action?.Invoke(null);
            //    return;
            //}
            //if(m_PoolObjName == "")
            //{
            //    return;
            //}
            CoreUtils.assetService.LoadAssetAsync<GameObject>(m_PoolObjName, (IAsset asset) =>
            {
                GameObject gameObject2 = CoreUtils.assetService.Instantiate(asset.asset() as GameObject);
                ObjectPoolItem component = gameObject2.GetComponent<ObjectPoolItem>();
                if (component != null)
                {
                    component.poolName = this.m_PoolObjName;
                    component.OnObjectSpawn();
                    this.m_max_spawn_number = component.maxSpawnNumber;
                    this.m_spawn_number++;
                }
                else
                {
                    Debug.LogError($"{m_PoolObjName} not ObjectPoolItem");
                }
                action?.Invoke(gameObject2);
            });
        }

        public void DeSpawn(GameObject obj, string ResourceName)
        {
            this.lastDespawnTime = Time.realtimeSinceStartup;
            if (this.m_pool.Contains(obj))
            {
                Debug.LogError("ObjectPoolMgr : despawn object already in stack = " + obj.name);
                return;
            }
            if (this.m_PoolObjName != ResourceName)
            {
                return;
            }
            obj.transform.SetParent(ObjectPoolMgr.GetInstance().m_handler.transform);
            obj.GetComponent<ObjectPoolItem>().OnObjectDespawn();
            obj.SetActive(false);
            this.m_pool.Push(obj);
        }

        public void Clear()
        {
            while (this.m_pool.Count != 0)
            {
                UnityEngine.Object.Destroy(this.m_pool.Pop());
            }
            this.m_pool.Clear();
            this.m_spawn_number = 0;
        }

        public void TryClean()
        {
            if (Time.realtimeSinceStartup - this.lastDespawnTime >= ObjectPoolMgr.GetInstance().m_clean_pool_time)
            {
                this.Clear();
            }
        }
    }
}
