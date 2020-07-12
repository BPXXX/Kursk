using System;
using UnityEngine;


namespace Skyunion
{
    public class EnvSoundHandler : MonoBehaviour
    {
        private bool m_isPoolItem;

        private bool m_isRegistered;

        public string m_name = string.Empty;

        public float m_maxDxf = 8000f;

        private void Awake()
        {
            if (base.GetComponent("ObjectPoolItem") != null)
            {
                this.m_isPoolItem = true;
            }
        }

        private void Start()
        {
            if (!this.m_isPoolItem)
            {
                this.RegisterSelf();
            }
        }

        private void OnDestroy()
        {
            this.UnRegisterSelf();
        }

        private void OnSpawn()
        {
            if (this.m_isPoolItem)
            {
                this.RegisterSelf();
            }
        }

        private void OnDespawn()
        {
            if (this.m_isPoolItem)
            {
                this.UnRegisterSelf();
            }
        }

        private void RegisterSelf()
        {
            if (!this.m_isRegistered)
            {
                this.m_isRegistered = true;
                CoreUtils.audioService.Register(this);
            }
        }

        private void UnRegisterSelf()
        {
            if (this.m_isRegistered)
            {
                this.m_isRegistered = false;
                CoreUtils.audioService.UnRegister(this);
            }
        }

        public static void SetName(EnvSoundHandler self, string name)
        {
            self.m_name = name;
        }

        public static string GetName(EnvSoundHandler self)
        {
            return self.m_name;
        }

        public static void SetMaxDxf(EnvSoundHandler self, float dxf)
        {
            self.m_maxDxf = dxf;
        }

        public static float GetMaxDxf(EnvSoundHandler self)
        {
            return self.m_maxDxf;
        }
    }
}
