using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using UnityEngine;

namespace Skyunion
{   
    public class ViewBinderAdaptor : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(ViewBinder);
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

		internal class Adaptor : ViewBinder, CrossBindingAdaptorType
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