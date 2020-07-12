//
// GameApp.cs
// Create:
//      2020-2-12
// Description:
//      广告服务接口
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using UnityEngine;

namespace Skyunion
{
    public interface IADService : IModule
    {
        void OnFetchUserID(string userId);
    }
}
