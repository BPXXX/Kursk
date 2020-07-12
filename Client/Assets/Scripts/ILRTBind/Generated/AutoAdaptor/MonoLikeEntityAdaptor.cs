using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using UnityEngine;

namespace Skyunion
{   
    public class MonoLikeEntityAdaptor : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(MonoLikeEntity);
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

		internal class Adaptor : MonoLikeEntity, CrossBindingAdaptorType
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

            
            IMethod mOnEnable;
            bool isOnEnableInvoking = false;

            public override void OnEnable()
            {
                if (mOnEnable == null)
                {
                    mOnEnable = instance.Type.GetMethod("OnEnable", 0);
                }
                if (mOnEnable != null && !isOnEnableInvoking)
                {
                    isOnEnableInvoking = true;
                    appdomain.Invoke(mOnEnable, instance );
                    isOnEnableInvoking = false;
                }
                else
                    base.OnEnable();
            }

            IMethod mStart;
            bool isStartInvoking = false;

            public override void Start()
            {
                if (mStart == null)
                {
                    mStart = instance.Type.GetMethod("Start", 0);
                }
                if (mStart != null && !isStartInvoking)
                {
                    isStartInvoking = true;
                    appdomain.Invoke(mStart, instance );
                    isStartInvoking = false;
                }
                else
                    base.Start();
            }

            IMethod mUpdate;
            bool isUpdateInvoking = false;

            public override void Update()
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

            IMethod mOnDisable;
            bool isOnDisableInvoking = false;

            public override void OnDisable()
            {
                if (mOnDisable == null)
                {
                    mOnDisable = instance.Type.GetMethod("OnDisable", 0);
                }
                if (mOnDisable != null && !isOnDisableInvoking)
                {
                    isOnDisableInvoking = true;
                    appdomain.Invoke(mOnDisable, instance );
                    isOnDisableInvoking = false;
                }
                else
                    base.OnDisable();
            }

            IMethod mOnDestroy;
            bool isOnDestroyInvoking = false;

            public override void OnDestroy()
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

            IMethod mLateUpdate;
            bool isLateUpdateInvoking = false;

            public override void LateUpdate()
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

            IMethod mFixedUpdate;
            bool isFixedUpdateInvoking = false;

            public override void FixedUpdate()
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