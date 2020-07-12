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
    public unsafe class IDataServiceBinding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            MethodBase method;
            Type[] args;
            Type type = typeof(Skyunion.IDataService);
            Dictionary<string, List<MethodInfo>> genericMethods = new Dictionary<string, List<MethodInfo>>();
            List<MethodInfo> lst = null;                    
            foreach(var m in type.GetMethods())
            {
                if(m.IsGenericMethodDefinition)
                {
                    if (!genericMethods.TryGetValue(m.Name, out lst))
                    {
                        lst = new List<MethodInfo>();
                        genericMethods[m.Name] = lst;
                    }
                    lst.Add(m);
                }
            }
            args = new Type[]{typeof(ILRuntime.Runtime.Intepreter.ILTypeInstance)};
            if (genericMethods.TryGetValue("QueryRecord", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(ILRuntime.Runtime.Intepreter.ILTypeInstance), typeof(System.Int32)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, QueryRecord_0);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(ILRuntime.Runtime.Intepreter.ILTypeInstance)};
            if (genericMethods.TryGetValue("QueryRecords", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, QueryRecords_1);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(ILRuntime.Runtime.Intepreter.ILTypeInstance)};
            if (genericMethods.TryGetValue("QueryTable", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(Skyunion.ITable<ILRuntime.Runtime.Intepreter.ILTypeInstance>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, QueryTable_2);

                        break;
                    }
                }
            }


        }


        static StackObject* QueryRecord_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @id = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.IDataService instance_of_this_method = (Skyunion.IDataService)typeof(Skyunion.IDataService).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var type = __method.GenericArguments[0].ReflectionType;

            var result_of_this_method = instance_of_this_method.QueryRecord<ILRuntime.Runtime.Intepreter.ILTypeInstance>(@id, type);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* QueryRecords_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.IDataService instance_of_this_method = (Skyunion.IDataService)typeof(Skyunion.IDataService).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var type = __method.GenericArguments[0].ReflectionType;

            var result_of_this_method = instance_of_this_method.QueryRecords<ILRuntime.Runtime.Intepreter.ILTypeInstance>(type);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* QueryTable_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.IDataService instance_of_this_method = (Skyunion.IDataService)typeof(Skyunion.IDataService).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var type = __method.GenericArguments[0].ReflectionType;

            var result_of_this_method = instance_of_this_method.QueryTable<ILRuntime.Runtime.Intepreter.ILTypeInstance>(type);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
