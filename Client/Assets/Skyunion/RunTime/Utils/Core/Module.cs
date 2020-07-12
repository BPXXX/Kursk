using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Skyunion
{
    public class Module : IModule
    {
        protected IPluginManager mPluginManager;
        protected bool mIsInitialized = false;
        protected int mLoadedCount = 0;
        private Action mCompleteAction;
        public Module()
        {
            mPluginManager = PluginManager.Instance();
        }
        public virtual bool Initialized()
        {
            return mIsInitialized;
        }
        public virtual void BeforeInit()
        {
        }
        public virtual void Init()
        {
            OnInitialized();
        }
        protected void OnInitialized()
        {
            mIsInitialized = true;
            mCompleteAction?.Invoke();
        }
        public virtual void WaitInitAsync(Action complete)
        {
            mCompleteAction += complete;
            if(mIsInitialized)
            {
                complete();
            }
        }
        public virtual void AfterInit()
        {
        }
        public virtual void Update()
        {
        }
        public virtual void LateUpdate()
        {
        }
        public virtual void BeforeShut()
        {
        }
        public virtual void Shut()
        {
        }
    }
}
