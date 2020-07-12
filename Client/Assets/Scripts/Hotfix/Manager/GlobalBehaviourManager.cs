/////////////////////////////////////////////////////////////////////////////////
// @desc 全局BehaviourBinder,适用于挂载HotFix工程中全局的Manager,事件系统还是走PureMVC
// @copyright ©2018 nbob
// @release 2018年4月13日 星期五
// @author BobWong
// @mail 15959187562@qq.com
/////////////////////////////////////////////////////////////////////////////////

/*
用法
GlobalBehaviourManger.Instance.AddGlobalMeditor<MapMediator>(true);
GlobalBehaviourManger.Instance.RemoveGlobalMediator(MapMediator.NameMediator);
*/

using System;
using UnityEngine;
using Game;
using System.Collections.Generic;
using GameFramework;
using Skyunion;

public class GlobalBehaviourManger : Hotfix.TSingleton<GlobalBehaviourManger>
{
    private BehaviourBinder m_behaviourBinder;

    private Dictionary<string,GameMediator> m_mediatorDic;

    public BehaviourBinder BehaviourBinder { get { return m_behaviourBinder; } set { m_behaviourBinder = value; } }

    private GlobalBehaviourManger()
    {
        GameObject go = new GameObject("GlobalBehaviour");
        m_behaviourBinder = go.AddComponent<BehaviourBinder>();
        m_mediatorDic = new Dictionary<string, GameMediator>();
        GameObject.DontDestroyOnLoad(go);
    }

    //protected override void Init()
    //{
    //    base.Init();
    //    GameObject go = new GameObject("GlobalBehaviour");
    //    m_behaviourBinder = go.AddComponent<BehaviourBinder>();
    //    m_mediatorDic = new Dictionary<string, GameMediator>();
    //    GameObject.DontDestroyOnLoad(go);
    //}

    /// <summary>
    /// 添加全局Mediator
    /// </summary>
    /// <returns>The global meditor.</returns>
    /// <param name="needUpdate">If set to <c>true</c> need update.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public T AddGlobalMeditor<T>(bool needUpdate) where T : GameMediator,new()
    {
        T tmp = new T();
        if (needUpdate)
        {
            m_behaviourBinder.updateCallback += tmp.Update;
            m_behaviourBinder.fixedUpdateCallback += tmp.FixedUpdate;
            m_behaviourBinder.lateUpdateCallback += tmp.LateUpdate;
        }
       
        string name = tmp.MediatorName;
        if (m_mediatorDic.ContainsKey(name))
        {
            Debug.LogErrorFormat("Can not add same meditor {0}",name);
        }
        
        m_mediatorDic.Add(name, tmp);
        AppFacade.GetInstance().RegisterMediator(tmp);
        return tmp;
    }

    /// <summary>
    /// 获取全局Mediator
    /// </summary>
    /// <returns>The global mediator.</returns>
    /// <param name="mediatorName">Mediator name.</param>
    public GameMediator GetGlobalMediator(string mediatorName)
    {
        if (m_mediatorDic.ContainsKey(mediatorName))
        {
            return m_mediatorDic[mediatorName];
        }
        return null;
    }

    /// <summary>
    /// 移除全局Mediator
    /// </summary>
    /// <param name="mediatorName">Mediator name.</param>
    public void RemoveGlobalMediator(string mediatorName)
    {
        if (m_mediatorDic.ContainsKey(mediatorName))
        {
            var tmp = m_mediatorDic[mediatorName];
            m_behaviourBinder.updateCallback -= tmp.Update;
            m_behaviourBinder.fixedUpdateCallback -= tmp.FixedUpdate;
            m_behaviourBinder.lateUpdateCallback -= tmp.LateUpdate;
            AppFacade.GetInstance().RemoveMediator(mediatorName);
            m_mediatorDic.Remove(mediatorName);

        }
    }

    public void AddUpdateListener(Action action)
    {
        m_behaviourBinder.updateCallback += action;
    }

    public void RemoveUpdateListener(Action action)
    {
        if (m_behaviourBinder.updateCallback != null) m_behaviourBinder.updateCallback -= action;
    }
}