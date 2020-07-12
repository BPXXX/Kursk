using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
using UnityEngine;
using System;
using Skyunion;

namespace ILRTBind
{
    public class ILRTBind
    {
        public static unsafe void Init(AppDomain appdomain)
        {
            // ×¢²áÊÊÅäÆ÷
            ILRuntimeHelper.Init(appdomain);
            // ×¢²áValue Type Binder
            RegisterValueTypeBinder(appdomain);

            // ×¢²áAdaptor
            RegisterCrossBindingAdaptor(appdomain);

            // ×¢²áÖØ¶¨Ïò·½·¨
            RegisterCLRMethodRedirection(appdomain);

            //// ×¢²áÎ¯ÍÐ
            RegisterDelegates(appdomain);
        }

        private static unsafe void RegisterValueTypeBinder(AppDomain appdomain)
        {
            appdomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());
            appdomain.RegisterValueTypeBinder(typeof(Quaternion), new QuaternionBinder());
            appdomain.RegisterValueTypeBinder(typeof(Vector2), new Vector2Binder());
        }

        private static unsafe void RegisterCrossBindingAdaptor(AppDomain appdomain)
        {
        }

        private static unsafe void RegisterCLRMethodRedirection(AppDomain appdomain)
        {
            CLRBindings.Initialize(appdomain);
            ILRuntime.Runtime.Generated.CLRBindings.Initialize(appdomain);
        }

        private static void RegisterDelegates(AppDomain appdomain)
        {
            appdomain.DelegateManager.RegisterMethodDelegate<Skyunion.IAsset>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Type, System.Object>();
            appdomain.DelegateManager.RegisterDelegateConvertor<Skyunion.CreateInstance>((act) =>
            {
                return new Skyunion.CreateInstance((type) =>
                {
                    return ((Func<System.Type, System.Object>)act)(type);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<Skyunion.NetEvent>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.IO.MemoryStream>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.ArraySegment<System.Byte>, Skyunion.NetPackInfo>();
            appdomain.DelegateManager.RegisterDelegateConvertor<Skyunion.ProtocolResolverDelegate>((act) =>
            {
                return new Skyunion.ProtocolResolverDelegate((segmentBytes) =>
                {
                    return ((Func<System.ArraySegment<System.Byte>, Skyunion.NetPackInfo>)act)(segmentBytes);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.Int32>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.Int32, System.Single>();
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
            {
                return new UnityEngine.Events.UnityAction(() =>
                {
                    ((Action)act)();
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<System.Single, System.Single, System.Single>();
            appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance>();

            appdomain.DelegateManager.RegisterMethodDelegate<Skyunion.NetEvent, System.Int32>();
        }
    }
}