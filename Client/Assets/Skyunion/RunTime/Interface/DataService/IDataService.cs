//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      数据对象 接口
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using ILRuntime.Other;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Skyunion
{
    public interface ITable<T>
    {
        T QueryRecord(int id);
        List<T> QueryRecords();
    }
    public enum DataMode
    {
        Binary,
        SQLite,
        Excel,
    }
    public interface IDataService : IModule
    {
        DataMode GetDataMode();
        T QueryRecord<T>(int id);
        List<T> QueryRecords<T>();
        ITable<T> QueryTable<T>();
        // 下面是给ILRuntime使用的
        T QueryRecord<T>(int id, Type type);
        List<T> QueryRecords<T>(Type type);
        ITable<T> QueryTable<T>(Type type);
    }
}
