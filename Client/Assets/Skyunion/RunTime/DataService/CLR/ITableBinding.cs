using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    public unsafe class ITableBinding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Skyunion.ITable<ILRuntime.Runtime.Intepreter.ILTypeInstance>);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("QueryRecord", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, QueryRecord_0);
            args = new Type[]{};
            method = type.GetMethod("QueryRecords", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, QueryRecords_1);


        }


        static StackObject* QueryRecord_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @id = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.ITable<ILRuntime.Runtime.Intepreter.ILTypeInstance> instance_of_this_method = (Skyunion.ITable<ILRuntime.Runtime.Intepreter.ILTypeInstance>)typeof(Skyunion.ITable<ILRuntime.Runtime.Intepreter.ILTypeInstance>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.QueryRecord(@id);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* QueryRecords_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.ITable<ILRuntime.Runtime.Intepreter.ILTypeInstance> instance_of_this_method = (Skyunion.ITable<ILRuntime.Runtime.Intepreter.ILTypeInstance>)typeof(Skyunion.ITable<ILRuntime.Runtime.Intepreter.ILTypeInstance>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.QueryRecords();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
