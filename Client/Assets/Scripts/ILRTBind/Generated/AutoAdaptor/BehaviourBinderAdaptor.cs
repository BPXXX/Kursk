using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using UnityEngine;

namespace Skyunion
{   
    public class BehaviourBinderAdaptor : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(BehaviourBinder);
            }
        }

        public override Type AdaptorType
        {
            get
            {
                return typeof(Adaptor);
            }
        }

        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new Adaptor(appdomain, instance);
        }

		internal class Adaptor : BehaviourBinder, CrossBindingAdaptorType
        {
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adaptor()
            {

            }

            public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            
            IMethod mUpdate;
            bool isUpdateInvoking = false;

            protected override void Update()
            {
                if (mUpdate == null)
                {
                    mUpdate = instance.Type.GetMethod("Update", 0);
                }
                if (mUpdate != null && !isUpdateInvoking)
                {
                    isUpdateInvoking = true;
                    appdomain.Invoke(mUpdate, instance );
                    isUpdateInvoking = false;
                }
                else
                    base.Update();
            }

            IMethod mFixedUpdate;
            bool isFixedUpdateInvoking = false;

            protected override void FixedUpdate()
            {
                if (mFixedUpdate == null)
                {
                    mFixedUpdate = instance.Type.GetMethod("FixedUpdate", 0);
                }
                if (mFixedUpdate != null && !isFixedUpdateInvoking)
                {
                    isFixedUpdateInvoking = true;
                    appdomain.Invoke(mFixedUpdate, instance );
                    isFixedUpdateInvoking = false;
                }
                else
                    base.FixedUpdate();
            }

            IMethod mLateUpdate;
            bool isLateUpdateInvoking = false;

            protected override void LateUpdate()
            {
                if (mLateUpdate == null)
                {
                    mLateUpdate = instance.Type.GetMethod("LateUpdate", 0);
                }
                if (mLateUpdate != null && !isLateUpdateInvoking)
                {
                    isLateUpdateInvoking = true;
                    appdomain.Invoke(mLateUpdate, instance );
                    isLateUpdateInvoking = false;
                }
                else
                    base.LateUpdate();
            }

            IMethod mOnDestroy;
            bool isOnDestroyInvoking = false;

            protected override void OnDestroy()
            {
                if (mOnDestroy == null)
                {
                    mOnDestroy = instance.Type.GetMethod("OnDestroy", 0);
                }
                if (mOnDestroy != null && !isOnDestroyInvoking)
                {
                    isOnDestroyInvoking = true;
                    appdomain.Invoke(mOnDestroy, instance );
                    isOnDestroyInvoking = false;
                }
                else
                    base.OnDestroy();
            }

            IMethod mOnBecameVisible;
            bool isOnBecameVisibleInvoking = false;

            protected override void OnBecameVisible()
            {
                if (mOnBecameVisible == null)
                {
                    mOnBecameVisible = instance.Type.GetMethod("OnBecameVisible", 0);
                }
                if (mOnBecameVisible != null && !isOnBecameVisibleInvoking)
                {
                    isOnBecameVisibleInvoking = true;
                    appdomain.Invoke(mOnBecameVisible, instance );
                    isOnBecameVisibleInvoking = false;
                }
                else
                    base.OnBecameVisible();
            }

            IMethod mOnBecameInvisible;
            bool isOnBecameInvisibleInvoking = false;

            protected override void OnBecameInvisible()
            {
                if (mOnBecameInvisible == null)
                {
                    mOnBecameInvisible = instance.Type.GetMethod("OnBecameInvisible", 0);
                }
                if (mOnBecameInvisible != null && !isOnBecameInvisibleInvoking)
                {
                    isOnBecameInvisibleInvoking = true;
                    appdomain.Invoke(mOnBecameInvisible, instance );
                    isOnBecameInvisibleInvoking = false;
                }
                else
                    base.OnBecameInvisible();
            }

            
            
            public override string ToString()
            {
                IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
                m = instance.Type.GetVirtualMethod(m);
                if (m == null || m is ILMethod)
                {
                    return instance.ToString();
                }
                else
                    return instance.Type.FullName;
            }
        }
    }

	
}