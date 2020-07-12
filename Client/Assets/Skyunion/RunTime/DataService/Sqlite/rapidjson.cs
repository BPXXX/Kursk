// Copyright (c) 2017 Takehito Gondo
//
// UniRapidJson is released under the MIT License.
// http://opensource.org/licenses/mit-license.php

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace rapidjson
{
    
    internal class PoolJson<T> where T : new()
    {
        static public List<T> bufs = new List<T>();
        
        static public T Get()
        {
            if (bufs.Count == 0)
            {
                return new T();
            }

            T t = bufs[bufs.Count - 1];
            bufs.RemoveAt(bufs.Count - 1);

            return t;
        }

        static public void Free(T t)
        {
            bufs.Add(t);
        }

        static public void FreeList(List<T> list, System.Action<T> fun)
        {
            for (int i = 0; i < list.Count; ++i)
                fun(list[i]);

            bufs.AddRange(list);
            list.Clear();
        }
    }
    
    
    public class Document : IDisposable
    {
        
       
        
        IntPtr ptr;
//#pragma warning disable 0414
        public JsonValue Root;
//#pragma warning restore

        public static Document Parse(byte[] bytes)
        {
            IntPtr ptr;
            if (!DLL._rapidjson_new_document_from_memory_bytes(bytes, (uint)bytes.Length, out ptr))
            {
                throw new DocumentParseError();
            }
            return PoolJson<Document>.Get().SetPtr(ptr);
        }
        
  #if UNITY_IOS && !UNITY_EDITOR     
        public static Document Parse(IntPtr st,int index)
        {
            IntPtr ptr;
            if (!DLL._rapidjson_new_document_from_sqlite(st, index, out ptr))
            {
                throw new DocumentParseError();
            }
            return PoolJson<Document>.Get().SetPtr(ptr);
        }
   #endif    

        public static Document Parse(string text)
        {
            IntPtr ptr;
            if (!DLL._rapidjson_new_document_from_memory_string(text, out ptr))
            {
                throw new DocumentParseError();
            }
            // UnityEngine.Debug.LogFormat("create data {0}",ptr);

            return PoolJson<Document>.Get().SetPtr(ptr);
        }

        public static Document ParseFromFile(string filepath)
        {
            IntPtr ptr;
            if (!DLL._rapidjson_new_document_from_file(filepath, out ptr))
            {
                throw new DocumentParseError();
            }
            return PoolJson<Document>.Get().SetPtr(ptr);
        }

        public Document()
        {
            
        }

        public Document SetPtr(IntPtr ptr)
        {
            this.ptr = ptr;
            this.Root = new JsonValue(this, ptr);
            return this;
        }

        ~Document()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (ptr != IntPtr.Zero)
            {
                // UnityEngine.Debug.LogFormat("Dispose data {0}",ptr);
                DLL._rapidjson_delete_document(out ptr);
            }
            PoolJson<Document>.Free(this);
            
        }

        public void CheckDisposed()
        {
            if (ptr == IntPtr.Zero)
            {
                throw new AlreadyDisposedDocumentError();
            }
        }
    }

    public struct JsonValue
    {
        Document doc;
        IntPtr ptr;

        public JsonValue(Document doc, IntPtr ptr)
        {
            this.doc = doc;
            this.ptr = ptr;
        }

        public JsonObject GetObject()
        {
            return PoolJson<JsonObject>.Get().SetPtr(doc, ptr);
        }
#if !UNITY_STANDALONE_WIN || !UNITY_EDITOR_WIN  
        public JsonObject GetObject(Dictionary<string, MemberInfo> map)
        {
            return PoolJson<JsonObject>.Get().SetPtr(doc, ptr,map);
        }
#endif

        public bool IsObject()
        {
            doc.CheckDisposed();
            return DLL._rapidjson_is_object(ptr);
        }

        public bool HasMember(string name)
        {
            doc.CheckDisposed();
            var ptr = IntPtr.Zero;
            return DLL.TryGet(this.ptr, name, out ptr);
        }

        public JsonArray GetArray()
        {
            return PoolJson<JsonArray>.Get().SetPrt(doc, ptr);
        }

        public bool IsArray()
        {
            doc.CheckDisposed();
            return DLL._rapidjson_is_array(ptr);
        }

        public int Size()
        {
            doc.CheckDisposed();
            uint size;
            if (!DLL._rapidjson_get_array_size(ptr, out size))
            {
                //throw new InvalidCastException();
                return 0;
            }
            return (int)size;
        }

        public JsonValue this[string name]
        {
            get
            {
                doc.CheckDisposed();
                var p = DLL.Get(ptr, name);
                return new JsonValue(doc, p);
            }
        }

        public JsonValue this[int index]
        {
            get
            {
                if (index < 0)
                {
                    throw new IndexOutOfRangeException();
                }
                doc.CheckDisposed();
                var p = DLL.Get(ptr, (uint)index);
                return new JsonValue(doc, p);
            }
        }

        public JsonType GetValueType()
        {
            doc.CheckDisposed();
            int t;
            if (!DLL._rapidjson_get_type(ptr, out t))
            {
                throw new InvalidCastException();
            }
            return (JsonType)t;
        }

        public bool IsNumber()
        {
            doc.CheckDisposed();
            return DLL._rapidjson_is_number(ptr);
        }

        public static explicit operator int(JsonValue self)
        {
            return self.ToInt();
        }

        public int ToInt()
        {
            doc.CheckDisposed();
            int value;
            if (!DLL._rapidjson_get_int(ptr, out value))
            {
                throw new InvalidCastException();
            }
            return value;
        }

        public bool IsInt()
        {
            doc.CheckDisposed();
            return DLL._rapidjson_is_int(ptr);
        }

        public static explicit operator uint(JsonValue self)
        {
            return self.ToUInt();
        }

        public uint ToUInt()
        {
            doc.CheckDisposed();
            uint value;
            if (!DLL._rapidjson_get_uint(ptr, out value))
            {
                throw new InvalidCastException();
            }
            return value;
        }

        public bool IsUInt()
        {
            doc.CheckDisposed();
            return DLL._rapidjson_is_uint(ptr);
        }

        public static explicit operator long(JsonValue self)
        {
            return self.ToLong();
        }

        public long ToLong()
        {
            doc.CheckDisposed();
            long value;
            if (!DLL._rapidjson_get_int64(ptr, out value))
            {
                throw new InvalidCastException();
            }
            return value;
        }

        public bool IsLong()
        {
            doc.CheckDisposed();
            return DLL._rapidjson_is_int64(ptr);
        }

        public static explicit operator ulong(JsonValue self)
        {
            return self.ToULong();
        }

        public ulong ToULong()
        {
            doc.CheckDisposed();
            ulong value;
            if (!DLL._rapidjson_get_uint64(ptr, out value))
            {
                throw new InvalidCastException();
            }
            return value;
        }

        public bool IsULong()
        {
            doc.CheckDisposed();
            return DLL._rapidjson_is_uint64(ptr);
        }

        public static explicit operator float(JsonValue self)
        {
            return self.ToFloat();
        }

        public float ToFloat()
        {
            doc.CheckDisposed();
            float value;
            if (!DLL._rapidjson_get_float(ptr, out value))
            {
                throw new InvalidCastException();
            }
            return value;
        }

        public bool IsFloat()
        {
            doc.CheckDisposed();
            return DLL._rapidjson_is_float(ptr);
        }

        public static explicit operator double(JsonValue self)
        {
            return self.ToDouble();
        }

        public double ToDouble()
        {
            doc.CheckDisposed();
            double value;
            if (!DLL._rapidjson_get_double(ptr, out value))
            {
                throw new InvalidCastException();
            }
            return value;
        }

        public bool IsDouble()
        {
            doc.CheckDisposed();
            return DLL._rapidjson_is_double(ptr);
        }

        public static explicit operator bool(JsonValue self)
        {
            return self.ToBool();
        }

        public bool ToBool()
        {
            doc.CheckDisposed();
            bool value;
            if (!DLL._rapidjson_get_bool(ptr, out value))
            {
                throw new InvalidCastException();
            }
            return value;
        }

        public bool IsBool()
        {
            doc.CheckDisposed();
            return DLL._rapidjson_is_bool(ptr);
        }

        public static explicit operator string(JsonValue self)
        {
            return self.ToString();
        }

        public override string ToString()
        {
            doc.CheckDisposed();
            string value;
            if (!DLL._rapidjson_get_string(ptr, out value))
            {
                throw new InvalidCastException();
            }
            return value;
        }

        public bool IsString()
        {
            doc.CheckDisposed();
            return DLL._rapidjson_is_string(ptr);
        }

        public bool IsNull()
        {
            if (ptr == IntPtr.Zero)
            {
                return true;
            }
            doc.CheckDisposed();
            return DLL._rapidjson_is_null(ptr);
        }

        public bool TryGetValueByPointer(string pointer, out JsonValue jsonValue)
        {
            doc.CheckDisposed();
            var dst = IntPtr.Zero;
            var isValid = false;
            var ret = DLL._rapidjson_get_value_by_pointer(ptr, pointer, (uint)pointer.Length, out isValid, out dst);
            if (!isValid)
            {
                throw new InvalidPointerError(pointer);
            }
            jsonValue = new JsonValue(ret ? doc : null, dst);
            return ret;
        }

        public int GetValueByPointer(string pointer, int defaultValue)
        {
            JsonValue jsonValue;
            if (TryGetValueByPointer(pointer, out jsonValue) && jsonValue.IsInt())
            {
                return (int)jsonValue;
            }
            return defaultValue;
        }

        public uint GetValueByPointer(string pointer, uint defaultValue)
        {
            JsonValue jsonValue;
            if (TryGetValueByPointer(pointer, out jsonValue) && jsonValue.IsUInt())
            {
                return (uint)jsonValue;
            }
            return defaultValue;
        }

        public long GetValueByPointer(string pointer, long defaultValue)
        {
            JsonValue jsonValue;
            if (TryGetValueByPointer(pointer, out jsonValue) && jsonValue.IsLong())
            {
                return (long)jsonValue;
            }
            return defaultValue;
        }

        public ulong GetValueByPointer(string pointer, ulong defaultValue)
        {
            JsonValue jsonValue;
            if (TryGetValueByPointer(pointer, out jsonValue) && jsonValue.IsULong())
            {
                return (ulong)jsonValue;
            }
            return defaultValue;
        }

        public float GetValueByPointer(string pointer, float defaultValue)
        {
            JsonValue jsonValue;
            if (TryGetValueByPointer(pointer, out jsonValue) && jsonValue.IsFloat())
            {
                return (float)jsonValue;
            }
            return defaultValue;
        }

        public double GetValueByPointer(string pointer, double defaultValue)
        {
            JsonValue jsonValue;
            if (TryGetValueByPointer(pointer, out jsonValue) && jsonValue.IsDouble())
            {
                return (double)jsonValue;
            }
            return defaultValue;
        }

        public bool GetValueByPointer(string pointer, bool defaultValue)
        {
            JsonValue jsonValue;
            if (TryGetValueByPointer(pointer, out jsonValue) && jsonValue.IsBool())
            {
                return (bool)jsonValue;
            }
            return defaultValue;
        }

        public string GetValueByPointer(string pointer, string defaultValue)
        {
            JsonValue jsonValue;
            if (TryGetValueByPointer(pointer, out jsonValue) && jsonValue.IsString())
            {
                return (string)jsonValue;
            }
            return defaultValue;
        }
    }

    public class JsonArray : IEnumerable<JsonValue>, IEnumerable
    {
        Document doc;
        ulong begin;
        uint elementSize;
        uint size;

        public int Length
        {
            get
            {
                return (int)size;
            }
        }

        public JsonArray()
        {
            
        }

        public JsonArray SetPrt(Document doc, IntPtr ptr)
        {
            doc.CheckDisposed();
            IntPtr elementsPointer;
            if (!DLL._rapidjson_get_array_iterator(ptr, out elementsPointer, out size, out elementSize))
            {
                throw new InvalidOperationException("Not Array Type.");
            }

            this.doc = doc;
            this.begin = (ulong)elementsPointer;
            return this;
        }

        public IEnumerator<JsonValue> GetEnumerator()
        {
            var end = begin + (size * elementSize);
            for (var itr = begin; itr != end; itr += elementSize)
            {
                var p = (IntPtr)itr;
                yield return new JsonValue(doc, p);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public JsonValue this[int index]
        {
            get
            {
                if (index >= 0 && (uint)index < size)
                {
                    var p = (IntPtr)(begin + (elementSize * (uint)index));
                    return new JsonValue(doc, p);
                }
                throw new IndexOutOfRangeException();
            }
        }


        public void Dispose()
        {
            PoolJson<JsonArray>.Free(this);
        }
    }

    public class JsonObject : Dictionary<string,JsonValue>
    {
        IntPtr root;
        Document doc;
        uint size;

        public int MemberCount
        {
            get
            {
                return (int)size;
            }
        }

        public JsonObject()
        {
           
        }
        
        
        public JsonObject SetPtr(Document doc, IntPtr ptr)
        {
            doc.CheckDisposed();
            if (!DLL._rapidjson_get_object_member_count(ptr, out size))
            {
                throw new InvalidOperationException("Not Object Type.");
            }

            this.doc = doc;
            this.root = ptr;
            
            
            for (uint i = 0; i < size; ++i)
            {
                doc.CheckDisposed();
                string key;
                IntPtr value;
                if (!DLL._rapidjson_get_key_value_pair_by_object_index(root, i, out key, out value))
                {
                    //
                }
                Add(key, new JsonValue(doc, value));
            }
            return this;
        }
#if !UNITY_STANDALONE_WIN || !UNITY_EDITOR_WIN   
        public JsonObject SetPtr(Document doc, IntPtr ptr,Dictionary<string, MemberInfo> map)
        {
            doc.CheckDisposed();
            if (!DLL._rapidjson_get_object_member_count(ptr, out size))
            {
                throw new InvalidOperationException("Not Object Type.");
            }

            this.doc = doc;
            this.root = ptr;
            if (size>0)
            {
                doc.CheckDisposed();
                foreach (var kv in map)
                {
                    IntPtr value;
                    if (DLL._rapidjson_get_key_value_by_object(root,kv.Key,out value))
                    {
                        Add(kv.Key, new JsonValue(doc, value));
                    }
                }
            }
            return this;
        }

#endif
        public bool TryGetValue(string name, out JsonValue jsonValue)
        {
            doc.CheckDisposed();
            var ptr = IntPtr.Zero;
            var ret = DLL.TryGet(root, name, out ptr);
            jsonValue = new JsonValue(ret ? doc : null, ptr);
            return ret;
        }

        public bool HasMember(string name)
        {
            doc.CheckDisposed();
            var ptr = IntPtr.Zero;
            return DLL.TryGet(root, name, out ptr);
        }
        
        
        public void Dispose()
        {
            this.Clear();
            PoolJson<JsonObject>.Free(this);
        }
    }

    public class DocumentParseError : Exception { }
    public class AlreadyDisposedDocumentError : Exception { }
    public class InvalidPointerError : Exception
    {
        public InvalidPointerError(string pointer) : base(pointer) { }
    }

    public enum JsonType
    {
        Null = 0,      
        False = 1,    
        True = 2,    
        Object = 3, 
        Array = 4,  
        String = 5, 
        Number = 6  
    }

    static class DLL
    {
        public static bool TryGet(IntPtr src, string name, out IntPtr dst)
        {
            bool isObject;
            if (!_rapidjson_get_value_by_object(src, name, out isObject, out dst))
            {
                dst = IntPtr.Zero;
                if (!isObject)
                {
                    throw new InvalidOperationException("Not Object Type.");
                }
                return false;
            }
            return true;
        }

        public static IntPtr Get(IntPtr src, string name)
        {
            IntPtr dst;
            if (!TryGet(src, name, out dst))
            {
                throw new KeyNotFoundException();
            }
            return dst;
        }

        public static IntPtr Get(IntPtr src, uint index)
        {
            bool isArray;
            IntPtr dst;
            if (!_rapidjson_get_value_by_array(src, index, out isArray, out dst))
            {
                if (!isArray)
                {
                    throw new InvalidOperationException("Not Array Type.");
                }
                throw new IndexOutOfRangeException();
            }
            return dst;
        }

#if !UNITY_EDITOR && UNITY_IOS
        const string LIBNAME = "__Internal";
#else
        const string LIBNAME = "rapidjson";
#endif
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_new_document_from_memory_bytes(byte[] bytes, uint length, out IntPtr document);
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_new_document_from_memory_string(string json, out IntPtr document);
        
#if  UNITY_IOS
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_new_document_from_sqlite(IntPtr src,int index, out IntPtr document);
#endif       
        
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_new_document_from_file([MarshalAs(UnmanagedType.LPStr)] string filepath, out IntPtr document);
        [DllImport(LIBNAME)]
        public static extern void _rapidjson_delete_document(out IntPtr document);

        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_get_value_by_object(IntPtr src, string name, [MarshalAs(UnmanagedType.I1)] out bool isObject, out IntPtr dst);
        
#if !UNITY_STANDALONE_WIN || !UNITY_EDITOR_WIN
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_get_key_value_by_object(IntPtr src, string name, out IntPtr dst);
#endif
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_get_object_member_count(IntPtr src, out uint size);
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_get_key_value_pair_by_object_index(IntPtr src, uint index, [MarshalAs(UnmanagedType.LPStr)] out string key, out IntPtr value);

        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_get_value_by_array(IntPtr src, uint index, [MarshalAs(UnmanagedType.I1)] out bool isArray, out IntPtr dst);
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_get_array_iterator(IntPtr src, out IntPtr elementsPointer, out uint size, out uint elementSize);
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_get_array_size(IntPtr src, out uint dst);

        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_get_int(IntPtr src, out int dst);
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_get_uint(IntPtr src, out uint dst);
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_get_int64(IntPtr src, out long dst);
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_get_uint64(IntPtr src, out ulong dst);
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_get_float(IntPtr src, out float dst);
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_get_double(IntPtr src, out double dst);

        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_get_string(IntPtr src, [MarshalAs(UnmanagedType.LPStr)] out string dst);
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_get_bool(IntPtr src, [MarshalAs(UnmanagedType.I1)] out bool dst);
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_get_type(IntPtr src, out int dst);

        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_is_array(IntPtr src);
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_is_object(IntPtr src);

        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_is_number(IntPtr src);
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_is_int(IntPtr src);
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_is_uint(IntPtr src);
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_is_int64(IntPtr src);
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_is_uint64(IntPtr src);
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_is_float(IntPtr src);
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_is_double(IntPtr src);

        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_is_string(IntPtr src);
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_is_bool(IntPtr src);
        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_is_null(IntPtr src);

        [DllImport(LIBNAME)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool _rapidjson_get_value_by_pointer(IntPtr src, [MarshalAs(UnmanagedType.LPStr)] string pointer, uint pointerLength, [MarshalAs(UnmanagedType.I1)] out bool isValid, out IntPtr dst);
    }
}
