
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UI_SubView
    {
        public RectTransform m_root_RectTransform;
        public GameObject gameObject;
        /// <summary>
        /// �����ڶ���Unity���Ͳ������
        /// </summary>
        /// <param name="root">���Ҹ��ڵ�</param>
        /// <param name="path">��Ը��ڵ��·��</param>
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