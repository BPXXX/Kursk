//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      ģ��ӿ�
// Author:
//      �⽭�� <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using System;

namespace Skyunion
{
    public interface IModule
    {
        //------------- �ӿ� -------------------//
        // ����Ƿ��ʼ�����
        bool Initialized();
        // ��ʼ��ǰ ׼��һЩ������ʼ��
        void BeforeInit();
        // ����ģ��ĳ�ʼ������֮���
        void Init();
        // �ȴ���ʼ�����
        void WaitInitAsync(Action complete);
        // ȫ����ʼ������в���
        void AfterInit();
        // ÿ֡����
        void Update();
        // ÿ֡�ͺ����
        void LateUpdate();
        // �ر�ǰ
        void BeforeShut();
        // �رպ�
        void Shut();
    };
}