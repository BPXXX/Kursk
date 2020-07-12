/////////////////////////////////////////////////////////////////////////////////
// @desc 视图绑定中介
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
	public class ViewBinder : BehaviourBinder {

		/// <summary>
		/// 动画播放完成回调
		/// </summary>
		public System.Action openAniEndCallback;
		/// <summary>
		/// 窗口关闭回调,有时候需要在下次打开的时候恢复现场
		/// </summary>
		public System.Action onWinCloseCallback;
		/// <summary>
		/// 窗口取得焦点回调
		/// </summary>
		public System.Action onWinFocusCallback;
		/// <summary>
		/// 预加载完成你回调
		/// </summary>
		public System.Action onPrewarmCallback;	

		private bool m_hasSortingOrder = false;
	    public bool HasSortingOrder
	    {
	        get { return m_hasSortingOrder; }
	    }
		private int m_sortingOrder = 0;
	    public int SortingOrder
	    {
	        get
	        {
	            return m_sortingOrder;
	        }
	    }

		private UILayer m_layer;

		public void SetSortingArg(UILayer layer,bool hasSortingOrder,int sortingOrder) {
			m_hasSortingOrder = hasSortingOrder;
			m_sortingOrder = sortingOrder;
			m_layer = layer;
		}

        public void Start()
        {
        }

        /// <summary>
        /// 窗口被关闭
        /// </summary>
        public void OnWinClose()
		{
			if (onWinCloseCallback != null)
			{
				onWinCloseCallback();
			}
		}

		/// <summary>
		/// 窗口得到焦点
		/// </summary>
		public void OnWinFocus()
		{
			if (onWinFocusCallback != null)
			{
				onWinFocusCallback();
			}
		}

		/// <summary>
		/// 窗口被预加载
		/// </summary>
		public void OnWinPrewarm()
		{
			if (onPrewarmCallback != null)
			{
				onPrewarmCallback();
			}
		}

		/// <summary>
		/// 被销毁
		/// </summary>
		protected override void OnDestroy()
		{
			base.OnDestroy();
			for (int i = 0; i < m_hotfixViewComps.Count; i++)
			{
				if (m_hotfixViewComps[i] != null && m_hotfixViewComps[i].gameObject != null)
				m_hotfixViewComps[i].gameObject = null;
			}
			m_hotfixViewComps.Clear();
		}

		/// <summary>
		/// 创建具体Go实例
		/// </summary>
		/// <param name="abName">bundle名字</param>
		/// <param name="realName">asset名字</param>
		/// <returns></returns>
		public static void Create(string assetName,GameView view,Action cbAction)
		{
            IAssetService assetService = PluginManager.Instance().FindModule<IAssetService>();
            assetService.LoadAssetAsync<GameObject>(assetName, (IAsset asset) =>{
				GameObject prefab = asset.asset() as GameObject;
				if (prefab == null)
				{
					CoreUtils.logService.Error("not find asset name: "+assetName);
					return;
				}
				GameObject go = CoreUtils.assetService.Instantiate(prefab) as GameObject;
                asset.Attack(go);
				var vb = go.GetComponent<ViewBinder>();
				if (vb == null)
				{
					vb = go.AddComponent<ViewBinder>();
				}
				vb.enabled = true;

				view.vb = vb;
				view.gameObject = go;

				if (cbAction!=null)
				{
					cbAction();
				}
			});
		}

		public static ViewBinder Create(GameObject go)
		{
			var vb = go.GetComponent<ViewBinder>();
			if (vb == null)
			{
				vb = go.AddComponent<ViewBinder>();
			}
			vb.enabled = true;
			return vb;
		}


		/// <summary>
		/// 执行默认动画
		/// </summary>
		public void RunDefaultAni()
		{
            //RectTransform rt = transform.GetComponent<RectTransform>();
            //rt.localScale = Vector3.zero;
            //Sequence seq = DOTween.Sequence();
            //seq.Append(rt.DOScale(new Vector3(1.01f,1.01f,1.01f),0.3f))
            //.Append(rt.DOScale(new Vector3(0.995f,0.995f,0.995f),0.07f))
            //.Append(rt.DOScale(Vector3.one,0.03f))
            //.AppendCallback(()=>{
            //	if (openAniEndCallback != null)
            //	{
            //		openAniEndCallback();
            //	}
            //});
            if (openAniEndCallback != null)
            {
                openAniEndCallback();
            }
        }

        /// <summary>
        /// 热更View组件
        /// </summary>
        /// <returns></returns>
        private List<GameView> m_hotfixViewComps = new List<GameView>();

		/// <summary>
		/// 添加热更View组件
		/// </summary>
		/// <param name="gameView"></param>
		/// <returns></returns>
		public GameView AddHotFixViewComponent(GameView gameView)
		{
			m_hotfixViewComps.Add(gameView);
			return gameView;
		}

		/// <summary>
		/// 移除热更View组件
		/// </summary>
		/// <param name="entity"></param>
		public void RemoveHotFixViewComponent(GameView entity)
		{
			if (m_hotfixViewComps.Contains(entity))
			{
				m_hotfixViewComps.Remove(entity);
			}
		}

		/// <summary>
		/// 获取热更View组件
		/// </summary>
		/// <param name="gameView"></param>
		/// <returns></returns>
		public GameView GetHotFixViewComponent(Type gameView)
		{
			string compRealName = string.Empty;
			int lastDotIndex = -1;
			for (int i = 0; i < m_hotfixViewComps.Count; i++)
			{
				compRealName = m_hotfixViewComps[i].ToString();
				lastDotIndex = compRealName.LastIndexOf('.');
				if (lastDotIndex > 0)
				{
					compRealName = compRealName.Substring(lastDotIndex + 1);
				}
				if (compRealName == gameView.Name)
				{
					return m_hotfixViewComps[i] as GameView;
				}
			}
			return null;
		}
	}
}
