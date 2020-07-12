//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      Unity资源接口
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using UnityEngine.SceneManagement;

namespace Skyunion
{
    public interface IAsset
    {
        string assetName();
        Scene scene();
        UnityEngine.Object asset();
        void Attack(UnityEngine.Object obj);
        void Release();
        void Retain();
        bool Update();
    }
}
