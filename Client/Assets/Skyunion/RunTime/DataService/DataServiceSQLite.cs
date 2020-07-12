using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using SqlCipher4Unity3D;
using UnityEngine;


namespace Skyunion
{
    public class DataServiceSQLite : DataService
    {
        private SqlCipher4Unity3D.SQLiteConnection dbconConnection;
        
        private string dbPath = String.Empty;

        protected override ITable<T> CreateTable<T>(Type type)
        {
            return new TableSqlite<T>(dbconConnection.ORM(type));
        }
        public override DataMode GetDataMode()
        {
            return DataMode.SQLite;
        }

        public override void Init()
        {
            dbPath = Path.Combine(Application.streamingAssetsPath,"Config.bytes");
            
#if UNITY_EDITOR
            var memory = dbPath.Replace(".bytes", ".memory");
            File.Copy(dbPath,memory,true);
            dbPath = memory;
#endif

            if (!File.Exists(dbPath))
            {
                Debug.LogError("数据库文件不存在");
            }
            
            dbconConnection = new SQLiteConnection(dbPath,"TON2019decc97a2e3019233f77f");
            OnInitialized();
        }

        public override void Shut()
        {
            base.Shut();
            dbconConnection.Close();
#if UNITY_EDITOR
            File.Delete(dbPath);
#endif
            dbconConnection = null;
        }
    }
    public class TableSqlite<T> : ITable<T>
    {
        private SQLiteORM sQLiteORM;
        private List<T> records;
        public TableSqlite(SQLiteORM sQLiteORM)
        {
            this.sQLiteORM = sQLiteORM;
        }

        public T QueryRecord(int id)
        {
            return sQLiteORM.ExecuteRow<T>(id);
        }

        public List<T> QueryRecords()
        {
            return records ?? (records = sQLiteORM.ExecuteList<T>());
        }
    }
}