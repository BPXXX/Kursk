using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using UnityEngine;

namespace Skyunion
{   
    public class GameViewAdaptor : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(GameView);
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

		internal class Adaptor : GameView, CrossBindingAdaptorType
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

            
            IMethod mLoadUI;
            bool isLoadUIInvoking = false;

            public override void LoadUI(Action action)
            {
                if (mLoadUI == null)
                {
                    mLoadUI = instance.Type.GetMethod("LoadUI", 1);
                }
                if (mLoadUI != null && !isLoadUIInvoking)
                {
                    isLoadUIInvoking = true;
                    appdomain.Invoke(mLoadUI, instance ,action);
                    isLoadUIInvoking = false;
                }
                else
                    base.LoadUI(action);
            }

            IMethod mBindUI;
            bool isBindUIInvoking = false;

            public override void BindUI()
            {
                if (mBindUI == null)
                {
                    mBindUI = instance.Type.GetMethod("BindUI", 0);
                }
                if (mBindUI != null && !isBindUIInvoking)
                {
                    isBindUIInvoking = true;
                    appdomain.Invoke(mBindUI, instance );
                    isBindUIInvoking = false;
                }
                else
                    base.BindUI();
            }

            IMethod mBindSingleUI;
            bool isBindSingleUIInvoking = false;

            public override void BindSingleUI(GameObject obj)
            {
                if (mBindSingleUI == null)
                {
                    mBindSingleUI = instance.Type.GetMethod("BindSingleUI", 1);
                }
                if (mBindSingleUI != null && !isBindSingleUIInvoking)
                {
                    isBindSingleUIInvoking = true;
                    appdomain.Invoke(mBindSingleUI, instance ,obj);
                    isBindSingleUIInvoking = false;
                }
                else
                    base.BindSingleUI(obj);
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