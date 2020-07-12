
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UI_SubView
    {
        public RectTransform m_root_RectTransform;
        public GameObject gameObject;
        /// <summary>
        /// 根据内定的Unity类型查找组件
        /// </summary>
        /// <param name="root">查找根节点</param>
        /// <param name="path">相对根节点的路径</param>
        /// <returns></returns>
        protected T FindUI<T>(Transform root, string path)
        {
            Transform tran = root.Find(path);
            if (tran && tran.gameObject != null)
            {
                return tran.gameObject.GetComponent<T>();
            }
            Debug.LogFormat("can't not find {0}", path);
            return (T)((object)null);
        }
        protected virtual void BindEvent() { }
    }
}