using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using rapidjson;

using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.Utils;
using ILRuntime.Reflection;

namespace GameFramework {
    public class rcjson
    {


        private static Dictionary<Type, Dictionary<string, MemberInfo>> _typeMap =
            new Dictionary<Type, Dictionary<string, MemberInfo>>();

        private static Dictionary<Type, Type> _typeEm = new Dictionary<Type, Type>();


        private static Type getEMType(Type inst_type) {
            Type elem_type;
            _typeEm.TryGetValue(inst_type,out elem_type);
            if (elem_type != null)
            {
                return elem_type;
            }
            foreach (PropertyInfo p_info in inst_type.GetProperties())
            {
                if (p_info.Name != "Item")
                    continue;

                ParameterInfo[] parameters = p_info.GetIndexParameters();
                if (parameters.Length != 1)
                    continue;

                if (parameters[0].ParameterType == typeof(int))
                    elem_type = p_info.PropertyType;
            }
            _typeEm[inst_type] = elem_type;
            return elem_type;
        }
        
        public static object ToObject (string jsondata,Type value_type) {
            Document document = Document.Parse (jsondata);
            object obj = ReadValue (value_type, document.Root);
            document.Dispose();
            return obj;
        }
#if UNITY_IOS && !UNITY_EDITOR    
        public static object ToObject (IntPtr st,int index,Type value_type) {
            Document document = Document.Parse (st,index);

            if (document!=null)
            {
                object obj = ReadValue (value_type, document.Root);
                document.Dispose();
                return obj;
            }
           
            return null;
        }
#endif
     
        private static object ReadValue (Type inst_type, JsonValue vread) {
            
            
            var vType = inst_type is ILRuntimeWrapperType ? ((ILRuntimeWrapperType)inst_type).CLRType.TypeForCLR : inst_type;
//            UnityEngine.Debug.LogFormat("read value {0}   {1}",vType.ToString(),vread.IsNull ());
            if (vread.IsNull ()) {
                return null;
            }

            if (vread.IsNumber())
            {
                if (vType == typeof(int) || vType.IsEnum)
                {
                    if (vread.IsInt())
                    {
                        return vread.ToInt();
                    }
                    if (vread.IsLong())
                    {
                        return (int)vread.ToLong();
                    }
                }
                if (vread.IsLong() && vType == typeof(long))
                {
                    return vread.ToLong();
                }
                if (vType == typeof(float))
                {
                    if (vread.IsFloat())
                    {
                        return vread.ToFloat();
                    }
                    if (vread.IsInt())
                    {
                        return (float)vread.ToInt();
                    }
                }
                if (vType == typeof(double))
                {
                    if (vread.IsDouble())
                    {
                        return vread.ToDouble();
                    }
                    if (vread.IsInt())
                    {
                        return (float)vread.ToInt();
                    }
                }
                
                UnityEngine.Debug.LogFormat("isInt :{0}  isLong :{1}  isFloat:{2} isDouble:{3}   {4}",
                    vread.IsInt().ToString(),vread.IsLong().ToString(),vread.IsFloat().ToString()
                    ,vread.IsDouble().ToString(),inst_type.ToString());
                UnityEngine.Debug.LogError("ReadValue no gess type:"+inst_type.ToString());
            }
            
            
            if (vread.IsString())
            {
                return vread.ToString();
            }
            

            object instance = null;
            if (vread.IsArray ()) {
                IList list;
                System.Type elem_type = null;
                int size = vread.Size ();

                //获取数组类型
                if (!inst_type.IsArray) {
                    list = (IList) Activator.CreateInstance (inst_type, size);
                    //get item value

                    if (inst_type is ILRuntime.Reflection.ILRuntimeWrapperType)
                    {
                        var wt = (ILRuntime.Reflection.ILRuntimeWrapperType)inst_type;
                        if (inst_type.IsArray)
                        {
                            elem_type = wt.CLRType.ElementType.ReflectionType; 
                        }
                        else
                        {
                            elem_type = wt.CLRType.GenericArguments[0].Value.ReflectionType;
                        }
                    }
                    else
                    {
                        elem_type = getEMType(inst_type);
                    }
                    

                } else {
                    list = new ArrayList (size);
                    elem_type = inst_type.GetElementType ();
                }
//                UnityEngine.Debug.Log("read array size: "+size+"  "+ elem_type.ToString());
                //填充数组
                if (size > 0) {
                    var array = vread.GetArray ();
                    for (int i = 0; i < size; i++)
                    {
                        object item = ReadValue (elem_type, array[i]);
                        
//                        if (item == null)
//                        {
//                            continue;
//                        }
//                        
//                        var rt = elem_type is ILRuntime.Reflection.ILRuntimeWrapperType ? ((ILRuntime.Reflection.ILRuntimeWrapperType)elem_type).RealType : elem_type;
//
//                        if (!rt.IsEnum)
//                        {
//                            item = rt.CheckCLRTypes(item);
//                        }
//
//                        if (list ==null || item == null)
//                        {
//                            continue;
//                        }
                        
                        
                        list.Add (item);
                    }
                    
                    array.Dispose();
              

                    if (inst_type.IsArray) {
                        int n = list.Count;
                        instance = System.Array.CreateInstance (elem_type, n);

                        for (int i = 0; i < n; i++)
                            ((System.Array) instance).SetValue (list[i], i);
                    } else {
                        instance = list;
                    }
                }
            } else if (vread.IsObject ()) {               
                if (inst_type is ILRuntime.Reflection.ILRuntimeType)
                    instance = ((ILRuntime.Reflection.ILRuntimeType)inst_type).ILType.Instantiate();
                else
                {
                    if (inst_type is ILRuntime.Reflection.ILRuntimeWrapperType)
                        inst_type = ((ILRuntime.Reflection.ILRuntimeWrapperType)inst_type).RealType;
                    instance = Activator.CreateInstance(inst_type);
                }
                

                Dictionary<string, MemberInfo> map = null;

                _typeMap.TryGetValue(inst_type, out map);

                if (map==null)
                {
                    MemberInfo[] fields = inst_type.GetFields (BindingFlags.Public | BindingFlags.Instance |
                                                               BindingFlags.SetProperty);
                    map = new Dictionary<string, MemberInfo> (fields.Length);
                    foreach (var field in fields)
                    {
                        map.Add(field.Name,field);
                    }
                    _typeMap.Add(inst_type,map);
                }
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                 var obj = vread.GetObject ();
#else
                var obj = vread.GetObject (map);
#endif

                foreach (var member in obj) {
                    MemberInfo prop_data = null;
                    map.TryGetValue (member.Key, out prop_data);
                    // UnityEngine.Debug.Log("key:"+member.Key+"  "+prop_data.ReflectedType+" is Null"+prop_data);
                    if (prop_data != null) {
                        if (prop_data is FieldInfo) {
                            FieldInfo p_info =
                                (FieldInfo) prop_data;

                            p_info.SetValue (
                                instance, ReadValue (p_info.FieldType, member.Value));
                        } else {
                            PropertyInfo p_info =
                                (PropertyInfo) prop_data;

                            if (p_info.CanWrite)
                                p_info.SetValue (
                                    instance,
                                    ReadValue (p_info.PropertyType, member.Value),
                                    null);
                        }
                    }
                }
                
                obj.Dispose();
            }
            return instance;
        }
        
        
        
        public unsafe static void RegisterILRuntimeCLRRedirection(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
            foreach(var i in typeof(rcjson).GetMethods())
            {
                if(i.Name == "ToObject" && i.IsGenericMethodDefinition)
                {
                    var param = i.GetParameters();
                    if(param[0].ParameterType == typeof(string))
                    {
                        appdomain.RegisterCLRMethodRedirection(i, JsonToObject);
                    }
                   
                }
            }
        }

        public unsafe static StackObject* JsonToObject(ILIntepreter intp, StackObject* esp, IList<object> mStack, CLRMethod method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(esp, 1);
            ptr_of_this_method = ILIntepreter.Minus(esp, 1);
            System.String json = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, mStack));
            intp.Free(ptr_of_this_method);
            var type = method.GenericArguments[0].ReflectionType;
            var result_of_this_method = ToObject(json,type);

            return ILIntepreter.PushObject(__ret, mStack, result_of_this_method);
        }
    }
}