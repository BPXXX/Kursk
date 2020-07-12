/////////////////////////////////////////////////////////////////////////////////
// @desc 热更层的视图基类
// @copyright ©2018 iGG
// @release 2018年4月11日 星期三
// @author BobWong
// @mail 15959187562@qq.com
/////////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;
using ILRuntime.Other;

namespace Skyunion
{
    [NeedAdaptorAttribute]
    public class GameView
    {
        /// <summary>
        /// 自定义数据
        /// </summary>
        public object data;

        /// <summary>
        /// 视图绑定中介引用
        /// </summary>
        public ViewBinder vb;

        /// <summary>
        /// 绑定的gameObject引用
        /// </summary>
        public GameObject gameObject;

        public GameView(GameObject go)
        {
            vb = go.GetComponent<ViewBinder>();
            if (vb == null)
            {
                vb = go.AddComponent<ViewBinder>();
            }
        }

        public GameView(){}

        
        
        /// <summary>
        /// 加载界面
        /// </summary>
        /// <param name="action"></param>
        public virtual void LoadUI(Action action)
        {
            
        }

        /// <summary>
        /// 根据内定的Unity类型查找组件
        /// </summary>
        /// <param name="root">查找根节点</param>
        /// <param name="path">相对根节点的路径</param>
        /// <returns></returns>
        protected T FindUI<T>(Transform root,string path){
            Transform tran = root.Find(path);
            if (tran && tran.gameObject != null )
            {
                return tran.gameObject.GetComponent<T>();
            }
            Debug.LogFormat("can't not find {0}",path);
            return (T)((object)null);
        }

        /// <summary>
        /// 绑定UI
        /// </summary>
        public virtual void BindUI(){}

        public virtual void BindSingleUI(UnityEngine.GameObject obj){}

        public virtual void Start(){ }
        public virtual void OnDestroy() { }
    }
}
