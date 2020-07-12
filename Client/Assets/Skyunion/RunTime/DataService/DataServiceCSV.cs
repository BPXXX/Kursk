using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Skyunion
{
    // SQLite的到时候自己写个 DataService_SQLite的实现类
    class DataServiceCSV : DataService
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

}
