using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Skyunion
{
    public class ObjectPoolItem : MonoBehaviour
    {
        public int m_max_spawn_number_high = 3;

        public int m_max_spawn_number_mid = 2;

        public int m_max_spawn_number_low = 1;

        private int m_max_spawn_number = ObjectPoolMgr.m_default_max_spawn_number;

        private List<ObjectPoolItem> m_childPoolItem;

        private string m_pool_name;

        private Vector3 m_org_scale = Vector3.one;

        public int maxSpawnNumber
        {
            get
            {
                return this.m_max_spawn_number;
            }
            set
            {
                this.m_max_spawn_number = value;
            }
        }

        public string poolName
        {
            get
            {
                return this.m_pool_name;
            }
            set
            {
                this.m_pool_name = value;
            }
        }

        private void UpdateMaxSpawnNumber()
        {
            if (CoreUtils.GetGraphicLevel() == CoreUtils.GraphicLevel.HIGH)
            {
                this.maxSpawnNumber = this.m_max_spawn_number_high;
            }
            else if (CoreUtils.GetGraphicLevel() == CoreUtils.GraphicLevel.MEDIUM)
            {
                this.maxSpawnNumber = this.m_max_spawn_number_mid;
            }
            else
            {
                this.maxSpawnNumber = this.m_max_spawn_number_low;
            }
        }

        private void Awake()
        {
            this.m_org_scale = base.transform.lossyScale;
            this.UpdateMaxSpawnNumber();
        }

        public void Disable(float delay)
        {
            base.StartCoroutine(this._disable(delay));
        }

        private IEnumerator _disable(float delay)
        {
            yield return new WaitForSeconds(delay);
            try
            {
                Disable();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }

        public virtual void Reset()
        {
        }

        public bool IsActive()
        {
            return base.gameObject.activeInHierarchy;
        }

        public virtual void Disable()
        {
            ObjectPoolMgr.GetInstance().GetPool(this.poolName).DeSpawn(base.gameObject, this.poolName);
        }

        public virtual void OnObjectSpawn()
        {
            base.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
        }

        public virtual void OnObjectDespawn()
        {
            this.PreDespawnChild();
            base.transform.localScale = this.m_org_scale;
            base.SendMessage("OnDespawn", SendMessageOptions.DontRequireReceiver);
        }

        public void SearchParent()
        {
            Transform parent = base.transform.parent;
            if (parent != null)
            {
                ObjectPoolItem componentInParent = parent.GetComponentInParent<ObjectPoolItem>();
                if (componentInParent != null)
                {
                    componentInParent.RegisterChild(this);
                }
            }
        }

        public void RegisterChild(ObjectPoolItem obj)
        {
            if (this.m_childPoolItem == null)
            {
                this.m_childPoolItem = new List<ObjectPoolItem>();
            }
            this.m_childPoolItem.Add(obj);
        }

        private void PreDespawnChild()
        {
            if (this.m_childPoolItem != null)
            {
                for (int i = 0; i < this.m_childPoolItem.Count; i++)
                {
                    if (this.m_childPoolItem[i] != null)
                    {
                        this.m_childPoolItem[i].Disable();
                    }
                }
                this.m_childPoolItem.Clear();
            }
        }
    }
}