using GameFramework;
using ILRuntime.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Skyunion
{
    // SQLite的到时候自己写个 DataService_SQLite的实现类
    public abstract class DataService : Module, IDataService
    {
        private Dictionary<Type, object> mTables = new Dictionary<Type, object>();
        protected abstract ITable<T> CreateTable<T>(Type type);

        public abstract DataMode GetDataMode();

        public T QueryRecord<T>(int id)
        {
            return QueryRecord<T>(id, typeof(T));
        }
        public List<T> QueryRecords<T>()
        {
            return QueryRecords<T>(typeof(T));
        }
        public ITable<T> QueryTable<T>()
        {
            return QueryTable<T>(typeof(T));
        }
        public T QueryRecord<T>(int id, Type type)
        {
            return QueryTable<T>(type).QueryRecord(id);
        }
        public List<T> QueryRecords<T>(Type type)
        {
            return QueryTable<T>(type).QueryRecords();
        }

        public virtual ITable<T> QueryTable<T>(Type type)
        {
            ITable<T> table;
            object tableObject;
            if (mTables.TryGetValue(type, out tableObject))
            {
                table = tableObject as ITable<T>;
            }
            else
            {
                var newTable = CreateTable<T>(type);
                mTables.Add(type, newTable);
                table = newTable;
            }
            return table;
        }
        private float mStartTime;
        public override void Init()
        {
            mStartTime = Time.realtimeSinceStartup;
            CoreUtils.assetService.WaitInitAsync(() =>
            {
                OnInitialized();
            });
        }
        public override void Shut()
        {
            mTables.Clear();
        }
    }

    public abstract class TableBase<T> : ITable<T>
    {
        protected abstract void LoadTable();

        private Type type;
        protected string tableName;
        private static TableBase<T> table;
        protected List<List<string>> rows;
        protected Dictionary<int, int> mapIdx = new Dictionary<int, int>();
        protected List<T> listRecords = new List<T>();
        protected bool fullRecords = false;
        protected FieldInfo[] fields;
        protected PropertyInfo[] props;
        public TableBase(Type type)
        {
            this.type = type;
            tableName = type.Name.Substring(0, type.Name.Length - 6);
            props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance |
                                                                 BindingFlags.SetProperty);
            fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
            LoadTable();
        }

        class Property
        {
        }

        public T QueryRecord(int id)
        {
            T value;
            int idx;
            if (mapIdx.TryGetValue(id, out idx))
            {
                value = listRecords[idx];
                if (value == null)
                {
                    value = listRecords[idx] = ConvertToObject(rows[idx]);
                }
            }
            else
            {
                value = default;
            }
            return value;
        }

        public List<T> QueryRecords()
        {
            if (fullRecords == false)
            {
                fullRecords = true;
                for (int i = 0; i < listRecords.Count; i++)
                {
                    if (listRecords[i] != null)
                        continue;
                    listRecords[i] = ConvertToObject(rows[i]);
                }
            }
            return listRecords;
        }

        protected object ReadValue(Type type, string content)
        {
            object value;
            if (typeof(string) == type)
            {
                if (content.Length == 0)
                {
                    value = string.Empty;
                }
                else
                {
                    value = string.Intern(content);
                }
            }
            else if (typeof(int) == type)
            {
                if (content.Length == 0)
                {
                    value = 0;
                }
                else
                {
                    value = 0;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    try
                    {
#endif
                        value = Convert.ToInt32(content);
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(type.Assembly.ToString()+ "error read 【"+content+"]");
                    }
#endif
                }
            }
            else if (typeof(bool) == type)
            {
                if (content.Length == 0)
                {
                    value = true;
                }
                else
                {
                    value = Convert.ToBoolean(content);
                }
            }
            else if (typeof(double) == type)
            {
                if (content.Length == 0)
                {
                    value = 0.0;
                }
                else
                {
                    value = Convert.ToDouble(content);
                }
            }
            else if (typeof(float) == type)
            {
                if (content.Length == 0)
                {
                    value = 0.0;
                }
                else
                {
                    value = Convert.ToSingle(content);
                }
            }
            else if (typeof(TimeSpan) == type)
            {
                if (content.Length == 0)
                {
                    value = TimeSpan.Zero;
                }
                else
                {
                    value = TimeSpan.Parse(content);
                }
            }
            else if (typeof(DateTime) == type)
            {
                if (content.Length == 0)
                {
                    value = DateTime.MinValue;
                }
                else
                {
                    value = Convert.ToDateTime(content);
                }
            }
            else if (typeof(DateTimeOffset) == type)
            {
                if (content.Length == 0)
                {
                    value = DateTimeOffset.MinValue;
                }
                else
                {
                    value = DateTimeOffset.Parse(content);
                }
            }
            else if (type.IsEnum)
            {
                if (content.Length == 0)
                {
                    value = 0;
                }
                else
                {
                    value = Convert.ToInt32(content);
                }
            }
            else if (typeof(long) == type)
            {
                if (content.Length == 0)
                {
                    value = TimeSpan.Zero;
                }
                else
                {
                    value = Convert.ToInt64(content);
                }
            }
            else if (typeof(uint) == type)
            {
                if (content.Length == 0)
                {
                    value = TimeSpan.Zero;
                }
                else
                {
                    value = Convert.ToUInt32(content);
                }
            }
            else if (typeof(decimal) == type)
            {
                if (content.Length == 0)
                {
                    value = TimeSpan.Zero;
                }
                else
                {
                    value = Convert.ToDecimal(content);
                }
            }
            else if (typeof(byte) == type)
            {
                if (content.Length == 0)
                {
                    value = TimeSpan.Zero;
                }
                else
                {
                    value = Convert.ToByte(content);
                }
            }
            else if (typeof(ushort) == type)
            {
                if (content.Length == 0)
                {
                    value = TimeSpan.Zero;
                }
                else
                {
                    value = Convert.ToUInt16(content);
                }
            }
            else if (typeof(short) == type)
            {
                if (content.Length == 0)
                {
                    value = TimeSpan.Zero;
                }
                else
                {
                    value = Convert.ToInt32(content);
                }
            }
            else if (typeof(sbyte) == type)
            {
                if (content.Length == 0)
                {
                    value = TimeSpan.Zero;
                }
                else
                {
                    value = Convert.ToSByte(content);
                }
            }
            else if (typeof(sbyte) == type)
            {
                if (content.Length == 0)
                {
                    value = TimeSpan.Zero;
                }
                else
                {
                    value = Convert.ToSByte(content);
                }
            }
            else if (typeof(byte[]) == type)
            {
                value = Encoding.UTF8.GetBytes(content);
            }
            else if (typeof(Guid) == type)
            {
                if (content.Length == 0)
                {
                    value = Guid.Empty;
                }
                else
                {
                    value = Guid.Parse(content);
                }
            }
            else if (type is ILRuntimeType && ((ILRuntimeType)type).ILType.IsEnum)
            {
                if (content.Length == 0)
                {
                    value = 0;
                }
                else
                {
                    value = Convert.ToInt32(content);
                }
            }
            else
            {
                try
                {
                    if (typeof(IList).IsAssignableFrom(type))
                    {
                        var list = (IList)Instantiate(type);
                        System.Type elem_type = null;
                        if (type is ILRuntime.Reflection.ILRuntimeWrapperType)
                        {
                            var wt = (ILRuntime.Reflection.ILRuntimeWrapperType)type;
                            if (type.IsArray)
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
                            elem_type = type.GenericTypeArguments[0];
                        }
                        if (content.Length > 0)
                        {
                            var elements = content.Split('|');
                            foreach (var element in elements)
                            {
                                list.Add(ReadValue(elem_type, element));
                            }
                        }
                        value = list;
                    }
                    else
                    {
                        if (content.Length == 0)
                        {
                            value = Instantiate(type);
                        }
                        else
                        {
                            value = rcjson.ToObject(content, type);
                        }
                    }
                }
                catch (Exception e)
                {
                    value = null;
                    Debug.LogErrorFormat($"{e.ToString()} \n {tableName}读取配置表错误 {type.ToString()}:{content}");
                }
            }

            return value;
        }

        private object Instantiate(Type type)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying || CoreUtils.hotService == null)
                return Activator.CreateInstance(type);
            else
                return CoreUtils.hotService.Instantiate(type);
#else
            return CoreUtils.hotService.Instantiate(type);
#endif
        }

        protected T ConvertToObject(List<string> cols)
        {
            var col = (T)Instantiate(type);

            if (cols.Count != fields.Length + props.Length)
            {
                Debug.LogError(type.FullName + " dataCols: "+ cols.Count +" != cs.fields:"+(fields.Length + props.Length)+ "id:" + cols[0]);
            }

            int nCol = 0;
            for (int i = 0; i < props.Length; i++, nCol++)
            {
                var property = props[i];
                var type = property.PropertyType;

                type = type is ILRuntimeWrapperType ? ((ILRuntimeWrapperType)type).RealType : type;

                if (nCol<cols.Count)
                {
                    property.SetValue(col, ReadValue(type, cols[nCol]));
                }
                
            }
            for (int i = 0; i < fields.Length; i++, nCol++)
            {
                var field = fields[i];
                var type = field.FieldType;

                type = type is ILRuntimeWrapperType ? ((ILRuntimeWrapperType)type).RealType : type;
                if (nCol < cols.Count)
                {
                    field.SetValue(col, ReadValue(type, cols[nCol]));
                }
            }
            return col;
        }
    }
}
