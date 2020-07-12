using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Skyunion
{
    // SQLite的到时候自己写个 DataService_SQLite的实现类
    class DataServiceBinary : DataService
    {
        protected override ITable<T> CreateTable<T>(Type type)
        {
            return new TableBinary<T>(type);
        }
        public override DataMode GetDataMode()
        {
            return DataMode.Binary;
        }
    }
    public class TableBinary<T> : TableBase<T>
    {
        public TableBinary(Type type) : base(type)
        {
        }
        public TableBinary() : base(typeof(T))
        {
        }
        protected override void LoadTable()
        {
            float startTime = Time.realtimeSinceStartup;
            var path = Path.Combine(Application.streamingAssetsPath, "Config/Bin/", $"{tableName}.bin");
            byte[] content;
#if UNITY_EDITOR
            if(!Application.isPlaying || CoreUtils.assetService == null)
            {
                content = File.ReadAllBytes(path);
            }
            else
#endif
            {
                content = CoreUtils.assetService.LoadFile(path);
            }
            var reader = new BinaryReader(new MemoryStream(content));

            int textNum = reader.ReadInt32();
            List<string> listText = new List<string>(textNum);

            for (int i = 0; i < textNum; i++)
            {
                int nSize = reader.ReadUInt16();
                var data = reader.ReadBytes(nSize);
                var value = Encoding.UTF8.GetString(data);
                listText.Add(value);
            }

            int nRow = reader.ReadInt32();
            Byte nCol = reader.ReadByte();
            rows = new List<List<string>>(nRow);
            listRecords = new List<T>(rows.Count);
            for (int row = 0; row < nRow; row++)
            {
                var cols = new List<string>(nCol);
                for (int col = 0; col < nCol; col++)
                {
                    int nIdx = reader.ReadInt32();
                    cols.Add(listText[nIdx]);
                }
                try
                {
                    int id = Convert.ToInt32(cols[0]);
                    mapIdx.Add(id, rows.Count);
                }
                catch (Exception e)
                {
                    mapIdx.Add(mapIdx.Count, rows.Count);
                }
                rows.Add(cols);
                listRecords.Add(default);
            }
        }
    }
}
