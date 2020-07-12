/////////////////////////////////////////////////////////////////////////////////
// @desc Behaviour绑定中介,用于模拟Mono的部分生命周期
// @copyright ©2018 iGG
// @release 2018年3月30日 星期五
// @author BobWong
// @mail 15959187562@qq.com
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ILRuntime.Other;

namespace Skyunion {

	[NeedAdaptorAttribute]
	public class BehaviourBinder : MonoBehaviour {
		/// <summary>
		/// update事件
		/// </summary>
		public System.Action updateCallback;
		/// <summary>
		/// fixedUpdate事件
		/// </summary>
		public System.Action fixedUpdateCallback;
		/// <summary>
		/// lateUpdate事件
		/// </summary>
		public System.Action lateUpdateCallback;

		// Update is called once per frame
		protected virtual void Update()
		{
			if (updateCallback != null)
			{
				updateCallback();
			}

			int len = _hotfixComps.Count;
			for (int i = 0; i < len; i++)
			{
				var cp = _hotfixComps[i];
				if (cp.needUpdate)
				{
					cp.Update();
				}
			}
		}

		protected virtual void FixedUpdate()
		{
			if (fixedUpdateCallback != null)
			{
				fixedUpdateCallback();
			}
			int len = _hotfixComps.Count;
			for (int i = 0; i < len; i++)
			{
				var cp = _hotfixComps[i];
				if (cp.needUpdate)
				{
					cp.FixedUpdate();
				}
			}
		}
		
		protected virtual void LateUpdate()
		{
			if (lateUpdateCallback != null)
			{
				lateUpdateCallback();
			}
			int len = _hotfixComps.Count;
			for (int i = 0; i < len; i++)
			{
				var cp = _hotfixComps[i];
				if (cp.needUpdate)
				{
					cp.LateUpdate();
				}
			}
		}

		protected virtual void OnDestroy()
		{
			_hotfixComps.Clear();
		}

		/// <summary>
		/// 从不被渲染到被渲染的时候调用
		/// </summary>
		protected virtual void OnBecameVisible() 
		{

		}
		
		/// <summary>
		/// 从被渲染状态切换到不被渲染的时候调用
		/// </summary>
		protected virtual void OnBecameInvisible() 
		{

		}

		/// <summary>
		/// monoLikeEntity列表,用于管理热更工程中的MonoLikeEntity
		/// </summary>
		/// <returns></returns>
		protected List<MonoLikeEntity> _hotfixComps = new List<MonoLikeEntity>();

		/// <summary>
		/// 添加MonoLikeEntity组件
		/// </summary>
		/// <param name="tmp"></param>
		/// <returns></returns>
		public MonoLikeEntity AddHotFixComponent(MonoLikeEntity tmp) 
		{
			_hotfixComps.Add(tmp);
			tmp.OnEnable();
			tmp.Start();
			return tmp;
		}

		/// <summary>
		/// 获取MonoLikeEntity组件
		/// </summary>
		/// <param name="monoLikeType"></param>
		/// <returns></returns>
		public MonoLikeEntity GetHotFixComponent(Type monoLikeType)
		{
			string compRealName = string.Empty;
			int lastDotIndex = -1;
			var len = _hotfixComps.Count;
			for (int i = 0; i < len; i++)
			{
				var cp = _hotfixComps[i];
				compRealName = cp.ToString();
				lastDotIndex = compRealName.LastIndexOf('.');
				if (lastDotIndex > 0)
				{
					compRealName = compRealName.Substring(lastDotIndex + 1);
				}
				if (compRealName == monoLikeType.Name)
				{
					return cp as MonoLikeEntity;
				}
			}
			return null;
		}

		/// <summary>
		/// 移除MonoLikeEntity组件
		/// </summary>
		/// <param name="entity"></param>
		public void RemoveHotFixComponent(MonoLikeEntity entity)
		{
			if (_hotfixComps.Contains(entity))
			{
				_hotfixComps.Remove(entity);
				entity.OnDestroy();
			}
		}
	}
}
