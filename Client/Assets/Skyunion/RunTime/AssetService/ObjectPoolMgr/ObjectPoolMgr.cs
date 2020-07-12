using System;
using System.Collections.Generic;
using UnityEngine;

namespace Skyunion
{
    internal class ObjectPoolMgr
    {
        public static readonly int m_default_max_spawn_number = 9999;

        private static readonly ObjectPoolMgr m_instance = new ObjectPoolMgr();

        private static Dictionary<string, ObjectPool> m_poolList = new Dictionary<string, ObjectPool>();

        public GameObject m_handler;

        public float m_clean_pool_time = 60f;

        public static ObjectPoolMgr GetInstance()
        {
            return m_instance;
        }

        public bool IsAnyInPool(UnityEngine.Object obj)
        {
            GameObject gameObject = (GameObject)obj;
            ObjectPoolItem component = gameObject.GetComponent<ObjectPoolItem>();
            return component && this.GetPool(component.poolName).avaliableCount != 0;
        }

        public ObjectPool GetPool(string name)
        {
            if (!m_poolList.ContainsKey(name))
            {
                m_poolList.Add(name, new ObjectPool());
            }
            return m_poolList[name];
        }

        public void ClearPool(string name)
        {
            if (!m_poolList.ContainsKey(name))
            {
                return;
            }
            m_poolList[name].Clear();
            m_poolList.Remove(name);
        }

        public void ClearAllPool()
        {
            foreach (ObjectPool current in m_poolList.Values)
            {
                current.Clear();
            }
            m_poolList.Clear();
        }

        public void TryCleanPool()
        {
            foreach (ObjectPool current in m_poolList.Values)
            {
                current.TryClean();
            }
        }
    }
}