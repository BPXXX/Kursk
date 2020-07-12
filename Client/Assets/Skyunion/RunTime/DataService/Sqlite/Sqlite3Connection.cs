using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Sqlite3DatabaseHandle = System.IntPtr;
using Sqlite3Statement = System.IntPtr;

using LitJson;
using ILRuntime.Reflection;
using GameFramework;
using UnityEngine;
using Random = System.Random;

namespace SqlCipher4Unity3D
{
    /// <summary>
    ///     Represents an open connection to a SQLite database.
    /// </summary>
    public class SQLiteConnection : IDisposable
    {
        internal static readonly Sqlite3DatabaseHandle NullHandle = default(Sqlite3DatabaseHandle);

        /// <summary>
        ///     Used to list some code that we want the MonoTouch linker
        ///     to see, but that we never want to actually execute.
        /// </summary>
        private static bool _preserveDuringLinkMagic;

        private readonly Random _rand = new Random();

        private TimeSpan _busyTimeout;
        private long _elapsedMilliseconds;
        private bool _open;

        private int _transactionDepth;

        private int _maxReadStringSize = 1024*1024;

        private int _readStringSize = 0;

        private bool _lastExeNoNull;
        
        private List<object> _bindObjects = new List<object>();


        private Dictionary<string, TableMapping> _mappings;

        static SQLiteConnection()
        {
            if (_preserveDuringLinkMagic)
            {
                ColumnInfo ti = new ColumnInfo();
                ti.Name = "magic";
            }
        }

        /// <summary>
        ///     Constructs a new SQLiteConnection and opens a SQLite database specified by databasePath.
        /// </summary>
        /// <param name="databasePath">
        ///     Specifies the path to the database file.
        /// </param>
        /// <param name="storeDateTimeAsTicks">
        ///     Specifies whether to store DateTime properties as ticks (true) or strings (false). You
        ///     absolutely do want to store them as Ticks in all new projects. The default of false is
        ///     only here for backwards compatibility. There is a *significant* speed advantage, with no
        ///     down sides, when setting storeDateTimeAsTicks = true.
        /// </param>
        public SQLiteConnection(string databasePath, string password = null, bool storeDateTimeAsTicks = false) : this(
            databasePath, password, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create, storeDateTimeAsTicks) { }

        /// <summary>
        ///     Constructs a new SQLiteConnection and opens a SQLite database specified by databasePath.
        /// </summary>
        /// <param name="databasePath">
        ///     Specifies the path to the database file.
        /// </param>
        /// <param name="storeDateTimeAsTicks">
        ///     Specifies whether to store DateTime properties as ticks (true) or strings (false). You
        ///     absolutely do want to store them as Ticks in all new projects. The default of false is
        ///     only here for backwards compatibility. There is a *significant* speed advantage, with no
        ///     down sides, when setting storeDateTimeAsTicks = true.
        /// </param>
        public SQLiteConnection(string databasePath, string password, SQLiteOpenFlags openFlags,
            bool storeDateTimeAsTicks = false)
        {
            if (string.IsNullOrEmpty(databasePath))
                throw new ArgumentException("Must be specified", "databasePath");

            this.DatabasePath = databasePath;

            Sqlite3DatabaseHandle handle;

            // open using the byte[]
            // in the case where the path may include Unicode
            // force open to using UTF-8 using sqlite3_open_v2
            byte[] databasePathAsBytes = GetNullTerminatedUtf8(this.DatabasePath);
            SQLite3.Result r = SQLite3.Open(databasePathAsBytes, out handle, (int) openFlags, IntPtr.Zero);

            this.Handle = handle;
            if (r != SQLite3.Result.OK)
                throw SQLiteException.New(r,
                    string.Format("Could not open database file: {0} ({1})", this.DatabasePath, r));

            if (!string.IsNullOrEmpty(password))
            {
                SQLite3.Result result = SQLite3.Key(handle, password, password.Length);
                if (result != SQLite3.Result.OK)
                    throw SQLiteException.New(r,
                        string.Format("Could not open database file: {0} ({1})", this.DatabasePath, r));
            }

            this._open = true;

            this.StoreDateTimeAsTicks = storeDateTimeAsTicks;

            this.BusyTimeout = TimeSpan.FromSeconds(0.1);
        }

        public Sqlite3DatabaseHandle Handle { get; private set; }

        public string DatabasePath { get; private set; }

        public bool StoreDateTimeAsTicks { get; private set; }


        /// <summary>
        ///     Sets a busy handler to sleep the specified amount of time when a table is locked.
        ///     The handler will sleep multiple times until a total time of <see cref="BusyTimeout" /> has accumulated.
        /// </summary>
        public TimeSpan BusyTimeout
        {
            get { return this._busyTimeout; }
            set
            {
                this._busyTimeout = value;
                if (this.Handle != NullHandle)
                    SQLite3.BusyTimeout(this.Handle, (int) this._busyTimeout.TotalMilliseconds);
            }
        }

        /// <summary>
        /// 返回true 说明有值
        /// </summary>
        /// <returns></returns>
        public bool LastExeColNoNull()
        {
            return _lastExeNoNull;
        }


        public void SetLastExeColNoNull(bool v)
        {
            _lastExeNoNull = v;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void EnableLoadExtension(int onoff)
        {
            SQLite3.Result r = SQLite3.EnableLoadExtension(this.Handle, onoff);
            if (r != SQLite3.Result.OK)
            {
                string msg = SQLite3.GetErrmsg(this.Handle);
                throw SQLiteException.New(r, msg);
            }
        }

        private static byte[] GetNullTerminatedUtf8(string s)
        {
            int utf8Length = Encoding.UTF8.GetByteCount(s);
            byte[] bytes = new byte[utf8Length + 1];
            utf8Length = Encoding.UTF8.GetBytes(s, 0, s.Length, bytes, 0);
            return bytes;
        }


        public TableMapping GetMapping(Type type, CreateFlags createFlags = CreateFlags.None)
        {
            if (this._mappings == null) this._mappings = new Dictionary<string, TableMapping>();
            TableMapping map;
            if (!this._mappings.TryGetValue(type.FullName, out map))
            {
                map = new TableMapping(type, createFlags);
                this._mappings[type.FullName] = map;
            }

            return map;
        }


        public SQLiteORM ORM<T>() {
            var orm = new SQLiteORM(this);
            var type = typeof(T);
            type = type is ILRuntimeWrapperType ? ((ILRuntimeWrapperType)type).CLRType.TypeForCLR : type;
            orm.MapType(type);
            return orm;
        }


        public SQLiteORM ORM(Type type)
        {
            var orm = new SQLiteORM(this);
            type = type is ILRuntimeWrapperType ? ((ILRuntimeWrapperType)type).CLRType.TypeForCLR : type;
            orm.MapType(type);
            return orm;
        }



        private void Finalize(Sqlite3Statement stmt)
        {
            SQLite3.Finalize(stmt);
        }


      

        ~SQLiteConnection()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            Close();
        }

        public void Close()
        {
            if (this._open && this.Handle != NullHandle)
                try
                {
                    SQLite3.Result r = SQLite3.Close(this.Handle);
                    if (r != SQLite3.Result.OK)
                    {
                        string msg = SQLite3.GetErrmsg(this.Handle);
                        throw SQLiteException.New(r, msg);
                    }
                }
                finally
                {
                    this.Handle = NullHandle;
                    this._open = false;
                }
        }

        private struct IndexedColumn
        {
            public int Order;
            public string ColumnName;
        }

        private struct IndexInfo
        {
            public string IndexName;
            public string TableName;
            public bool Unique;
            public List<IndexedColumn> Columns;
        }

        public class ColumnInfo
        {
            [Column("name")]
            public string Name { get; set; }
            public int notnull { get; set; }
            public override string ToString()
            {
                return this.Name;
            }
        }

    }

    /// <summary>
    ///     Represents a parsed connection string.
    /// </summary>
    internal class SQLiteConnectionString
    {
        public string ConnectionString { get; private set; }
        public string DatabasePath { get; private set; }
        public bool StoreDateTimeAsTicks { get; private set; }

        public SQLiteConnectionString(string databasePath, bool storeDateTimeAsTicks)
        {
            this.ConnectionString = databasePath;
            this.StoreDateTimeAsTicks = storeDateTimeAsTicks;
            this.DatabasePath = databasePath;
        }
    }


    public class Column
    {
        private readonly MemberInfo _prop;

        private readonly FieldInfo _tfiled;

        private readonly PropertyInfo _tprop;

        public Column(MemberInfo prop, CreateFlags createFlags = CreateFlags.None)
        {
           

            this._prop = prop;
            if (this._prop is FieldInfo)
            {
                this.IsField = true;

                _tfiled = (FieldInfo)prop;
                this.ColumnType = _tfiled.FieldType;
            }
            else
            {
                _tprop = (PropertyInfo)prop;
                this.ColumnType = _tprop.PropertyType;
            }

            this.Name = prop.Name;

            this.IsPK = SQLiteORM.IsPK(prop);

        }

        public string Name { get; private set; }

        public string PropertyName
        {
            get { return this._prop.Name; }
        }

        public Type ColumnType { get; private set; }

        public string Collation { get; private set; }

        public bool IsAutoInc { get; private set; }
        public bool IsAutoGuid { get; private set; }

        public bool IsPK { get; private set; }

        public bool IsNullable { get; private set; }

        public int? MaxStringLength { get; private set; }

        public bool IsField { get; private set; }

        public void SetValue(object obj, object val)
        {
            if (val == null)
            {
                return;
            }
            if (IsField)
            {
                _tfiled.SetValue(obj, val);
            }
            else {
                if (_tprop.CanWrite)
                    _tprop.SetValue(obj, val,null);
            }

        }
    }

    public class TableMapping
    {
        private readonly Column _autoPk;

        private Dictionary<string, Column> _map = new Dictionary<string, Column>();


        public static void AddRange<T>(ref T[] array, T[] items)
        {
            int size = array.Length;
            System.Array.Resize(ref array, array.Length + items.Length);
            for (int i = 0; i < items.Length; i++)
                array[size + i] = items[i];
        }

        public TableMapping(Type type, CreateFlags createFlags = CreateFlags.None)
        {
            this.MappedType = type;
           
            this.TableName = this.MappedType.Name;

            MemberInfo[] props = this.MappedType.GetProperties(BindingFlags.Public | BindingFlags.Instance |
                                                                 BindingFlags.SetProperty);
            MemberInfo[] fields = this.MappedType.GetFields(BindingFlags.Public | BindingFlags.Instance |
                                                                 BindingFlags.SetProperty);

            TableMapping.AddRange(ref props,fields);


            List<Column> cols = new List<Column>();


            foreach (MemberInfo p in props)
            {
                bool ignore = p.IsDefined(typeof(IgnoreAttribute), true);
                if (!ignore) cols.Add(new Column(p, createFlags));
            }

            this.Columns = cols.ToArray();
            foreach (Column c in this.Columns)
            {
                if (c.IsAutoInc && c.IsPK) this._autoPk = c;
                if (c.IsPK) this.PK = c;

                _map.Add(c.Name, c);
            }

            this.HasAutoIncPK = this._autoPk != null;
          
        }

        public Type MappedType { get; private set; }
        public string TableName { get; private set; }
        public Column[] Columns { get; private set; }
        public Column PK { get; private set; }

        public bool HasAutoIncPK { get; private set; }
      
        public void SetAutoIncPK(object obj, long id)
        {
            if (this._autoPk != null) this._autoPk.SetValue(obj, Convert.ChangeType(id, this._autoPk.ColumnType, null));
        }

        public Column FindColumnWithPropertyName(string propertyName)
        {
            Column exact = this.Columns.FirstOrDefault(c => c.PropertyName == propertyName);
            return exact;
        }

        public Column FindColumn(string columnName)
        {
            Column exact = null;
            _map.TryGetValue(columnName, out exact);
            if (exact == null)
            {
                Debug.LogWarningFormat("can't find the sqlite column {0}",columnName);
            }
            return exact;
        }

        protected internal void Dispose()
        {

        }

       
    }

    public class SQLiteORM {

        

        internal static IntPtr NegativePointer = new IntPtr(-1);
        private List<object> _bindings;
        private readonly SQLiteConnection _conn;
        //private StringBuilder _cmdBuilder;

        private List<object> m_cmdList;

        private TableMapping _mapType;

        private int _state = 0;

        private string CommandText;



        internal SQLiteORM(SQLiteConnection conn)
        {
            this._conn = conn;
            m_cmdList = new List<object>(8);
        }

        /// <summary>
        /// 必须先绑定数据类型
        /// </summary>
        /// <returns>The type.</returns>
        /// <param name="type">Type.</param>
        public SQLiteORM MapType(Type type) {
            if (this._conn!=null)
            {
                _mapType = this._conn.GetMapping(type);
            }
            return this;
        }


        /// <summary>
        /// 构建最终的语句
        /// </summary>
        protected void Build(string sql = "",params object[] args) {
            CommandText = sql;
            if (args != null && args.Length>0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    Bind(args[i]);
                }
            }
  
        }          
 

        /// <summary>
        /// 执行不查询的操作
        /// </summary>
        /// <returns>The non query.</returns>
        public int ExecuteNonQuery()
        {
            Build();
            SQLite3.Result r = SQLite3.Result.OK;
            IntPtr stmt = Prepare();
            r = SQLite3.Step(stmt);
            Finalize(stmt);
           
            if (r == SQLite3.Result.Done)
            {
                int rowsAffected = SQLite3.Changes(this._conn.Handle);
                return rowsAffected;
            }

            if (r == SQLite3.Result.Error)
            {
                string msg = SQLite3.GetErrmsg(this._conn.Handle);
                throw SQLiteException.New(r, msg);
            }

            if (r == SQLite3.Result.Constraint)
                if (SQLite3.ExtendedErrCode(this._conn.Handle) == SQLite3.ExtendedResult.ConstraintNotNull)
                    throw NotNullConstraintViolationException.New(r, SQLite3.GetErrmsg(this._conn.Handle));
            throw SQLiteException.New(r, r.ToString());
        }

        /// <summary>
        /// 返回一个表的多行
        /// </summary>
        /// <returns>The table.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public IEnumerable<T> ExecuteTable<T>(string sql = "",params object[] args) {
            Build(sql,args);
            IntPtr stmt = Prepare();

            try
            {
                Column[] cols = new Column[SQLite3.ColumnCount(stmt)];

                for (int i = 0; i < cols.Length; i++)
                {
                    string name = SQLite3.ColumnName16(stmt, i);
                    cols[i] = _mapType.FindColumn(name);
                }

                while (SQLite3.Step(stmt) == SQLite3.Result.Row)
                {
                    object obj = null;
                    var value_type = _mapType.MappedType;
                    if (value_type is ILRuntimeType)
                        obj = ((ILRuntimeType)value_type).ILType.Instantiate();
                    else
                    {
                        if (value_type is ILRuntimeWrapperType)
                            value_type = ((ILRuntimeWrapperType)value_type).RealType;
                        obj = Activator.CreateInstance(value_type);
                    }
                    for (int i = 0; i < cols.Length; i++)
                    {
                        if (cols[i] == null)
                            continue;
                        SQLite3.ColType colType = SQLite3.ColumnType(stmt, i);
                        object val = ReadCol(stmt, i, colType, cols[i].ColumnType);
                        if (val !=null)
                        {
                            cols[i].SetValue(obj, val);
                        }
                    }
                    yield return (T)obj;
                }
            }
            finally
            {
                Finalize(stmt);
            }
        }

        /// <summary>
        /// 返回一个表的多行
        /// </summary>
        /// <returns>The list.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public List<T> ExecuteList<T>() {
            
            Build(string.Format("SELECT * FROM \"{0}\"",_mapType.TableName),null);
            IntPtr stmt = Prepare();
            List<T> list = new List<T>();
            try
            {
                Column[] cols = new Column[SQLite3.ColumnCount(stmt)];

                for (int i = 0; i < cols.Length; i++)
                {
                    string name = SQLite3.ColumnName16(stmt, i);
                    cols[i] = _mapType.FindColumn(name);
                }

                while (SQLite3.Step(stmt) == SQLite3.Result.Row)
                {
                    object obj = null;
                    var value_type = _mapType.MappedType;
                    if (value_type is ILRuntimeType)
                        obj = ((ILRuntimeType)value_type).ILType.Instantiate();
                    else
                    {
                        if (value_type is ILRuntimeWrapperType)
                            value_type = ((ILRuntimeWrapperType)value_type).RealType;
                        obj = Activator.CreateInstance(value_type);
                    }
                    for (int i = 0; i < cols.Length; i++)
                    {
                        if (cols[i] == null)
                            continue;
                        SQLite3.ColType colType = SQLite3.ColumnType(stmt, i);
                        object val = ReadCol(stmt, i, colType, cols[i].ColumnType);
                        if (val != null)
                        {
                            cols[i].SetValue(obj, val);
                        }
                    }
                    list.Add((T)obj);
                }
            }
            finally
            {
                Finalize(stmt);
            }
            return list;
        }

        /// <summary>
        /// 返回单行对象
        /// </summary>
        /// <returns>The row.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T ExecuteRow<T>(params object[] args) {
            if (_mapType.PK!=null)
            {
                Build(string.Format("SELECT * FROM \"{0}\" WHERE {1}=?",_mapType.TableName,_mapType.PK.Name),args);
            }
            else
            {
                Build(string.Format("SELECT * FROM \"{0}\"",_mapType.TableName),null);
            }

            object obj = null;
            IntPtr stmt = Prepare();
            try
            {
                if (SQLite3.Step(stmt) == SQLite3.Result.Row)
                {
                    Column[] cols = new Column[SQLite3.ColumnCount(stmt)];

                    for (int i = 0; i < cols.Length; i++)
                    {
                        string name = SQLite3.ColumnName16(stmt, i);
                        cols[i] = _mapType.FindColumn(name);
                    }
                    var value_type = _mapType.MappedType;
                    if (value_type is ILRuntimeType)
                        obj = ((ILRuntimeType)value_type).ILType.Instantiate();
                    else
                    {
                        if (value_type is ILRuntimeWrapperType)
                            value_type = ((ILRuntimeWrapperType)value_type).RealType;
                        obj = Activator.CreateInstance(value_type);
                    }
                    for (int i = 0; i < cols.Length; i++)
                    {
                        if (cols[i] == null)
                            continue;
                        SQLite3.ColType colType = SQLite3.ColumnType(stmt, i);
                        object val = ReadCol(stmt, i, colType, cols[i].ColumnType);
                        if (val != null)
                        {
                            cols[i].SetValue(obj, val);
                        }
                        
                    }
                }
            }
            finally
            {
                Finalize(stmt);
            }
            return (T)obj;
        }


        public int ExecuteCount()
        {
            Build();
            int val = 0;
            IntPtr stmt = Prepare();

            try
            {
                SQLite3.Result r = SQLite3.Step(stmt);

                if (r == SQLite3.Result.Row)
                {
                    SQLite3.ColType colType = SQLite3.ColumnType(stmt, 0);

                    object obj = ReadCol(stmt, 0, colType, typeof(int));
                    if (obj != null)
                    {
                        val = (int)obj;
                    }
                }
                else if (r == SQLite3.Result.Done) { }
                else
                {
                    throw SQLiteException.New(r, SQLite3.GetErrmsg(this._conn.Handle));
                }
            }
            finally
            {
                Finalize(stmt);
            }

            return val;
        }


        /// <summary>
        /// 返回一列的一个值
        /// </summary>
        /// <returns>The scalar.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T ExecuteCol<T>(string sql = "",params object[] args) {
            
            Build(sql,args);

            T val = default(T);

            IntPtr stmt = Prepare();

            try
            {
                SQLite3.Result r = SQLite3.Step(stmt);

                if (r == SQLite3.Result.Row)
                {
                    SQLite3.ColType colType = SQLite3.ColumnType(stmt, 0);

                    string name = SQLite3.ColumnName16(stmt, 0);
                    Column col = _mapType.FindColumn(name);
                    
                    
                    
                    object obj = ReadCol(stmt, 0, colType, col.ColumnType);
                    
                    this._conn.SetLastExeColNoNull(obj!=null);
                    if (obj!=null)
                    {
                        val = (T)obj;
                    }
                    
                }
                else if (r == SQLite3.Result.Done)
                {
                    this._conn.SetLastExeColNoNull(false);
                }
                else
                {
                    throw SQLiteException.New(r, SQLite3.GetErrmsg(this._conn.Handle));
                }
            }
            finally
            {
                Finalize(stmt);
            }

            return val;
        }

        /// <summary>
        /// 返回一列中的多个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> ExecuteColList<T>(string sql = "",params object[] args) {
      
            Build(sql,args);
            IntPtr stmt = Prepare();
            List<T> list = new List<T>();
            try
            {
                // Step会迭代一次，所以用do...while
                SQLite3.Result r = SQLite3.Step(stmt);
                do
                {
                    T val = default(T);
                    if (r == SQLite3.Result.Row)
                    {
                        SQLite3.ColType colType = SQLite3.ColumnType(stmt, 0);
                        string name = SQLite3.ColumnName16(stmt, 0);
                        Column col = _mapType.FindColumn(name);
                        val = (T)ReadCol(stmt, 0, colType, col.ColumnType);
                    }
                    else if (r == SQLite3.Result.Done) { }
                    else
                    {
                        throw SQLiteException.New(r, SQLite3.GetErrmsg(this._conn.Handle));
                    }
                    list.Add(val);
                } while (SQLite3.Step(stmt) == SQLite3.Result.Row);
            }
            finally
            {
                Finalize(stmt);
            }
            return list;
        }

        /// <summary>
        /// 读取返回的值
        /// </summary>
        /// <returns>The col.</returns>
        /// <param name="stmt">Statement.</param>
        /// <param name="index">Index.</param>
        /// <param name="type">Type.</param>
        /// <param name="refType">Clr type.</param>
        private object ReadCol(Sqlite3Statement stmt, int index, SQLite3.ColType type, Type refType)
        {
           
            var clrType = refType is ILRuntimeWrapperType ? ((ILRuntimeWrapperType)refType).RealType : refType;

            //GameFramework.Log.InfoFormat("ReadCol:{0} ColType:{1}  ILType:{2}  refType:{3}", index, type.ToString(), clrType.ToString(), refType.ToString());


            if (type == SQLite3.ColType.Null)
            {
                return null;
            }
                


            if (refType is ILRuntimeType && ((ILRuntimeType)refType).ILType.IsEnum)
            {
                var ttt = ((ILRuntimeType)refType);
                var t = ttt.ILType.ReflectionType;
                return SQLite3.ColumnInt(stmt, index);
            }

            if (clrType == typeof(string)) return  string.Intern(SQLite3.ColumnString(stmt, index));

            if (clrType == typeof(int)) return SQLite3.ColumnInt(stmt, index);

            if (clrType == typeof(bool)) return SQLite3.ColumnInt(stmt, index) == 1;

            if (clrType == typeof(double)) return SQLite3.ColumnDouble(stmt, index);

            if (clrType == typeof(float)) return (float)SQLite3.ColumnDouble(stmt, index);

            if (clrType == typeof(TimeSpan)) return new TimeSpan(SQLite3.ColumnInt64(stmt, index));

            if (clrType == typeof(DateTime))
            {
                if (this._conn.StoreDateTimeAsTicks) return new DateTime(SQLite3.ColumnInt64(stmt, index));

                string text = SQLite3.ColumnString(stmt, index);
                return DateTime.Parse(text);
            }

            if (clrType == typeof(DateTimeOffset))
            {
                return new DateTimeOffset(SQLite3.ColumnInt64(stmt, index), TimeSpan.Zero);
            }

            if (clrType.IsEnum)
            {
                return SQLite3.ColumnInt(stmt, index);
            }

            if (clrType == typeof(long)) return SQLite3.ColumnInt64(stmt, index);

            if (clrType == typeof(uint)) return (uint)SQLite3.ColumnInt64(stmt, index);

            if (clrType == typeof(decimal)) return (decimal)SQLite3.ColumnDouble(stmt, index);

            if (clrType == typeof(byte)) return (byte)SQLite3.ColumnInt(stmt, index);

            if (clrType == typeof(ushort)) return (ushort)SQLite3.ColumnInt(stmt, index);

            if (clrType == typeof(short)) return (short)SQLite3.ColumnInt(stmt, index);

            if (clrType == typeof(sbyte)) return (sbyte)SQLite3.ColumnInt(stmt, index);

            if (clrType == typeof(byte[])) return SQLite3.ColumnByteArray(stmt, index);

            if (clrType == typeof(Guid))
            {
                string text = SQLite3.ColumnString(stmt, index);
                return new Guid(text);
            }


#if UNITY_IOS && !UNITY_EDITOR    
            try
            {
                object obj = rcjson.ToObject(stmt, index, refType);
                if (obj != null)
                {
                    return obj;
                }
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("{0}读取配置表错误:{1}",this.CommandText,e.Message);
                return null;
            }
            
       
#else
            string json_data =SQLite3.ColumnString(stmt, index);
            if (json_data == null || string.IsNullOrEmpty(json_data))
            {
                return null;
            }
            
            try
            {
                object obj = rcjson.ToObject(json_data,refType);            
                if (obj != null)
                {
                    return obj;
                }
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("{0}读取配置表错误:{1}",this.CommandText,json_data+e.StackTrace);
                return null;
            }
#endif

           


            
            throw new NotSupportedException("Don't know how to read " + clrType);
        }

        private Sqlite3Statement Prepare()
        {

            IntPtr stmt = SQLite3.Prepare2(_conn.Handle, CommandText);
            BindAll(stmt);
            return stmt;
        }

        private void Finalize(Sqlite3Statement stmt)
        {
            SQLite3.Finalize(stmt);
            
            if (_bindings!=null)
            {
                _bindings.Clear();
            }
        }


        #region 参数绑定

        public SQLiteORM Bind(object val)
        {
            if (_bindings == null)
            {
                _bindings = new List<object>(1);
            }

            if (val == null)
            {
                Debug.LogErrorFormat("绑定参数错误");
            }
            
           
            
            _bindings.Add(val);
            return this;
        }

        
        
        private void BindAll(Sqlite3Statement stmt)
        {
            if (_bindings!=null)
            {
                int nextIdx = 1;
                foreach (var b in _bindings)
                {
                    BindParameter(stmt, nextIdx, b, _conn.StoreDateTimeAsTicks);
                    nextIdx++;
                }
            }
#if UNITY_EDITOR || UNITY_STANDALONE_WIN      
            if (!CommandText.Contains("LanguageDefine") && CommandText.Contains("?") && _bindings.Count>0)
            {
                //Debug.LogFormat("{0} [{1}]",CommandText,_bindings[0]);
            }
#endif
        }

        
        internal static void BindParameter (Sqlite3Statement stmt, int index, object value, bool storeDateTimeAsTicks)
        {
            if (value == null) {
                SQLite3.BindNull (stmt, index);
            } else {
                if (value is Int32) {
                    SQLite3.BindInt (stmt, index, (int)value);
                } else if (value is String) {
                    SQLite3.BindText (stmt, index, (string)value, -1, NegativePointer);
                } else if (value is Byte || value is UInt16 || value is SByte || value is Int16) {
                    SQLite3.BindInt (stmt, index, Convert.ToInt32 (value));
                } else if (value is Boolean) {
                    SQLite3.BindInt (stmt, index, (bool)value ? 1 : 0);
                } else if (value is UInt32 || value is Int64) {
                    SQLite3.BindInt64 (stmt, index, Convert.ToInt64 (value));
                } else if (value is Single || value is Double || value is Decimal) {
                    SQLite3.BindDouble (stmt, index, Convert.ToDouble (value));
                } else if (value is TimeSpan) {
                    SQLite3.BindInt64(stmt, index, ((TimeSpan)value).Ticks);
                } else if (value is DateTime) {
                    if (storeDateTimeAsTicks) {
                        SQLite3.BindInt64 (stmt, index, ((DateTime)value).Ticks);
                    }
                    else {
                        SQLite3.BindText (stmt, index, ((DateTime)value).ToString ("yyyy-MM-dd HH:mm:ss"), -1, NegativePointer);
                    }
                } else if (value is DateTimeOffset) {
                    SQLite3.BindInt64 (stmt, index, ((DateTimeOffset)value).UtcTicks);
                } else if (value.GetType().IsEnum) {
                    SQLite3.BindInt (stmt, index, Convert.ToInt32 (value));
                } else if (value is byte[]){
                    SQLite3.BindBlob(stmt, index, (byte[]) value, ((byte[]) value).Length, NegativePointer);
                } else if (value is Guid) {
                    SQLite3.BindText(stmt, index, ((Guid)value).ToString(), 72, NegativePointer);
                } else {
                    throw new NotSupportedException("Cannot store type: " + value.GetType());
                }
            }
        }



        #endregion

      

        #region 主键约束判断

        public const int DefaultMaxStringLength = 140;

        public static bool IsPK(MemberInfo p)
        {
             return p.IsDefined(typeof(PrimaryKeyAttribute), true);
        }

        public static string Collation(MemberInfo p)
        {
            object[] attrs = p.GetCustomAttributes(typeof(CollationAttribute), true);
            if (attrs.Length > 0)
                return ((CollationAttribute)attrs[0]).Value;
            return string.Empty;
        }

        public static bool IsAutoInc(MemberInfo p)
        {
            object[] attrs = p.GetCustomAttributes(typeof(AutoIncrementAttribute), true);
            return attrs.Length > 0;
        }

        public static int? MaxStringLength(MemberInfo p)
        {
            object[] attrs = p.GetCustomAttributes(typeof(MaxLengthAttribute), true);
            if (attrs.Length > 0)
                return ((MaxLengthAttribute)attrs[0]).Value;
            return null;
        }

        public static bool IsMarkedNotNull(MemberInfo p)
        {
            object[] attrs = p.GetCustomAttributes(typeof(NotNullAttribute), true);
            return attrs.Length > 0;
        }


        #endregion

    }

}