using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Skyunion
{
    internal class UIManager : Module, IUIManager
    {
        private IAssetService mAssetService;

        #region 实现 IModule
        public override void BeforeInit()
        {
            mAssetService = mPluginManager.FindModule<IAssetService>();
        }

        public override void Init()
        {
            InitUIRoot();
            mIsInitialized = true;
        }
        #endregion 

        #region 实现 IUIManager
        float screenScale = 1.0f;
        GameObject uiRoot;
        Canvas canvas;
        Transform trans;
        Camera uicamera;
        // 当前窗口的所有层级根节点
        Dictionary<int, Transform> layers = new Dictionary<int, Transform>();
        // 保存需要堆叠的界面 方便后续一个一个关闭过去
        List<UIInfo> uiStack = new List<UIInfo>();
        Dictionary<Type, UIInfo> uiCache = new Dictionary<Type, UIInfo>();
        Dictionary<int, List<UIInfo>> layerStacks = new Dictionary<int, List<UIInfo>>();

        Dictionary<int, UIInfo> uiIdDic = new Dictionary<int, UIInfo>();

        UIInfo lastClosedUI;//最后关闭的UI界面

        public UIInfo currentLoadingUI = null;

        List<UIInfo> infoList = new List<UIInfo>();

        List<UIInfo> m_tempStackUIList = new List<UIInfo>();

        public event OnShowUI m_onShowUIEvent;

        public event OnCloseUI m_onCloseUIEvent;

        public bool HasPopView;

        public void AddShowUIListener(OnShowUI param)
        {
            m_onShowUIEvent += param;
        }

        public void RemoveShowUIListener(OnShowUI param)
        {
            m_onShowUIEvent -= param;
        }

        public void AddCloseUIListener(OnCloseUI param)
        {
            m_onCloseUIEvent += param;
        }

        public void RemoveCloseUIListener(OnCloseUI param)
        {
            m_onCloseUIEvent -= param;
        }

        private void SetPopViewStatus(UIInfo uiInfo)
        {
            if ((int)uiInfo.info.layer > (int)UILayer.FullViewMenuLayer)
            {
                HasPopView = true;
            }
            else
            {
                if (uiInfo.assetName == "UI_Pop_ScoutSelect")
                {
                    HasPopView = true;
                }
            }
            //Debug.LogFormat("HasPopView:{0}", HasPopView);
        }

        private void UpdatePopViewStatus()
        {
            bool isHasPop = false;
            for (int i = 0; i < uiStack.Count; ++i)
            {
                if (uiStack[i].uiObj != null && uiStack[i].uiObj.activeSelf)
                {
                    if ((int)uiStack[i].info.layer > (int)UILayer.FullViewMenuLayer)
                    {
                        isHasPop = true;
                        break;
                    }
                }
            }
            HasPopView = isHasPop;
            //Debug.LogFormat("HasPopView:{0}", HasPopView);
        }

        public bool IsHasPopView()
        {
            return HasPopView;
        }

        /// <summary>
        /// 展示UI
        /// </summary>
        /// <param name="uiInfo"></param>
        /// <param name="callback"></param>
        /// <param name="data"></param>
        /// <param name="needOrder">需要重新排序</param>
        public void ShowUI(UIInfo uiInfo, Action callback = null, object data = null)
        {
            Type pageName = uiInfo.viewClass;
            Debug.LogFormat("ShowUI:{0} {1}", pageName, uiInfo.assetName);
            if(uiInfo.autoPlayUISound)
            {
                CoreUtils.audioService.PlayOneShot(uiInfo.OpenSound);
            }
            //关闭界面一层层退回去处理
            if (CheckNeedStackUI(uiInfo))
            {
                bool find = false;
                for (int i = 0; i < uiStack.Count; ++i)
                {
                    if (uiStack[i] == uiInfo)
                    {
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                    uiStack.Add(uiInfo);
                }
            }
            uiIdDic[uiInfo.uiId] = uiInfo;

            UIInfo oldUiInfo;
            if (uiCache.TryGetValue(pageName, out oldUiInfo))
            {
                if (oldUiInfo.uiObj != null)
                {
                    bool isRet = oldUiInfo.uiObj.activeSelf;
                    oldUiInfo.uiObj.SetActive(true);
                    SetPopViewStatus(oldUiInfo);
                    bool isExcuteFocus = false;
                    if (!isRet)
                    {
                        AddMask(oldUiInfo);
                        oldUiInfo.View.data = data;
                        oldUiInfo.gameView.OnWinFocus();
                        isExcuteFocus = true;
                    }
                    if (oldUiInfo.ParentUI != null)
                    {
                        RectTransform parent = GetUILayer((int)oldUiInfo.ParentUI.info.layer);
                        if (oldUiInfo.ParentUI.maskObj)
                        {
                            oldUiInfo.gameView.transform.SetParent(oldUiInfo.ParentUI.maskObj.transform);
                        }
                        else
                        {
                            oldUiInfo.gameView.transform.SetParent(parent);
                        }
                        RectTransform rt = uiInfo.gameView.transform.GetComponent<RectTransform>();
                        rt.offsetMin = Vector2.zero;
                        rt.offsetMax = Vector2.zero;
                        oldUiInfo.gameView.transform.transform.localScale = Vector3.one;
                        //oldUiInfo.gameView.transform.transform.SetSiblingIndex(oldUiInfo.ParentUI.gameView.transform.childCount-1);
                        oldUiInfo.gameView.OnWinFocus();
                        return;
                    }
                    else
                    {
                        if (!isExcuteFocus)
                        {
                            if (oldUiInfo.assetName == "UI_Win_Guide")
                            {
                                AddMask(oldUiInfo);
                                oldUiInfo.View.data = data;
                                oldUiInfo.gameView.OnWinFocus();
                            }
                        }
                    }
                    LinkUIHandle(oldUiInfo, () => {
                        //子模块加载完成后再回调
                        callback?.Invoke();
                    });
                }
            }
            else
            {
                currentLoadingUI = uiInfo;
                //异步加载
                LoadUIAsync(uiInfo.assetName, uiInfo, pageName, callback, data);
            }
        }

        public void HideUI(UIInfo uiInfo)
        {
            UIInfo[] linkUI = uiInfo.linkUI;
            foreach (var item in linkUI)
            {
                HideUI(item);
            }
            if (uiInfo.uiObj != null)
            {
                uiInfo.uiObj.SetActive(false);
            }
            if (uiInfo.maskObj != null)
            {
                uiInfo.maskObj.SetActive(false);
            }
            UpdatePopViewStatus();
        }
        /// <summary>
        /// 关闭UI
        /// </summary>
        /// <param name="uiInfo"></param>
        /// <param name="IsManual">true手动关闭,手动点击关闭按钮;false自动关闭</param>
        /// <param name="IsForceClose">true强制移除，false非强制移除</param>
        public void CloseUI(UIInfo uiInfo, bool IsManual = true, bool IsForceClose = false, bool returnToLogin = false)
        {
            int count = 0;
            int countMax = uiInfo.linkUI.Length;
            if (uiInfo.autoPlayUISound)
            {
                CoreUtils.audioService.PlayOneShot(uiInfo.CloseSound) ;
            }
            if (countMax > 0 && !returnToLogin)
            {
                for (int i = 0; i < countMax; ++i)
                {
                    if (uiInfo.linkUI[i].uiObj != null)
                    {
                        count++;
                    }
                }
                if (count == countMax)
                {
                    OnCloseUI(uiInfo, IsManual, IsForceClose);
                }
            }
            else
            {
                OnCloseUI(uiInfo, IsManual, IsForceClose);
            }
        }

        public void CloseLayerUI(UILayer uiLayer, bool isForceClose = false, bool returnToLogin = false)
        {
            infoList.Clear();
            foreach (var item in uiStack)
            {
                if (item.info.layer == uiLayer)
                {
                    infoList.Add(item);
                }
            }
            foreach (var item in layerStacks)
            {
                foreach (var info in item.Value)
                {
                    if (!infoList.Contains(info))
                    {
                        if (info.info.layer == uiLayer)
                        {
                            infoList.Add(info);
                        }
                    }
                }
            }
            for (int i = 0; i < infoList.Count; i++)
            {
                CloseUI(infoList[i], true, isForceClose, returnToLogin);
                if (uiStack.Contains(infoList[i]))
                {
                    uiStack.Remove(infoList[i]);
                }
            }
        }
        /// <summary>
        /// 关闭所有的UI
        /// </summary>
        /// <param name="IsForceClose">true强制移除，false非强制移除</param>
        /// <param name="returnToLogin">返回登陆界面隐藏界面不用判断linkui，不然关闭不了</param>
        public void CloseAll(bool isForceClose = false, bool returnToLogin = false)
        {
            infoList.Clear();
            foreach (var item in uiStack)
            {
                infoList.Add(item);
            }
            foreach (var item in layerStacks)
            {
                foreach (var info in item.Value)
                {
                    if (!infoList.Contains(info))
                        infoList.Add(info);
                }
            }
            for (int i = 0; i < infoList.Count; i++)
            {
                CloseUI(infoList[i], true, isForceClose, returnToLogin);
                if (uiStack.Contains(infoList[i]))
                {
                    uiStack.Remove(infoList[i]);
                }
            }
        }

        public bool ExistUI(UIInfo uiInfo)
        {
            return uiCache.Values.Any(i => i == uiInfo);
        }

        public bool ExistUI(int uiId)
        {
            if (uiIdDic.ContainsKey(uiId))
            {
                return true;
            }
            return false;
        }

        public UIInfo GetUI(int uiId)
        {
            if (uiIdDic.ContainsKey(uiId))
            {
                return uiIdDic[uiId];
            }
            return null;
        }

        public Canvas GetCanvas()
        {
            return canvas;
        }

        public Camera GetUICamera()
        {
            return uicamera;
        }

        public RectTransform GetUILayer(int layerIndex)
        {
            if(layers.ContainsKey(layerIndex))
            {
                return layers[layerIndex] as RectTransform;
            }
            return uiRoot.transform as RectTransform;
        }

        #endregion


        #region 私有方法
        private void InitUIRoot()
        {
            Debug.Log("UIManager InitUIRoot");
            uiRoot = GameObject.Find("UIRoot");
            if(uiRoot == null)
            {
                uiRoot = CoreUtils.assetService.Instantiate(Resources.Load<GameObject>("UIRoot"));
                Debug.LogWarning("你必须要把UIManager/Resources的预制体放到场景或者参考此预制体在场景创建一份！");
            }

            trans = uiRoot.transform;
            canvas = uiRoot.GetComponent<Canvas>();
            uicamera = uiRoot.GetComponentInChildren<Camera>();
            SetPerfectResolution();
            Array layer = Enum.GetValues(typeof(UILayer));
            Transform subRoot = null;
            for (int i = 0; i < layer.Length; i++)
            {
                layerStacks[i] = new List<UIInfo>();
                string strName = Enum.GetName(typeof(UILayer), i); //获取名称
                subRoot = FindLayer(strName);
                if (subRoot)
                {
                    layers.Add(i, subRoot.transform);
                }
                else
                {
                    Debug.Log("error layer " + strName);
                }
            }
            //设置高dpi下的点击和拖动的零界值
            int threshold = (int)Mathf.Ceil(UnityEngine.Screen.dpi / 10.0f);
            UnityEngine.EventSystems.EventSystem.current.pixelDragThreshold = threshold;

            mIsInitialized = true;
        }

        // 通过index获取到layer
        private Transform GetLayerByIndex(int index)
        {
            if (!layers.ContainsKey(index))
            {
                Debug.LogError("Error layer index " + index);
                return null;
            }

            return layers[index];
        }

        // 通过layer名称获取layer
        private Transform FindLayer(string layerName)
        {
            Transform tmpTrans = trans.Find("Container/" + layerName);
            if (tmpTrans != null)
            {
                return tmpTrans;
            }
            else
            {
                Debug.LogError("Can not find layer transform " + layerName);
                return null;
            }
        }

        //设置游戏分辨率为 UI的分辨率，这样可以提高渲染效率
        private void SetPerfectResolution()
        {
            screenScale = 1.0f;
            if (Application.platform == RuntimePlatform.WindowsEditor
                || Application.platform == RuntimePlatform.WindowsPlayer
                || Application.platform == RuntimePlatform.OSXPlayer
                || Application.platform == RuntimePlatform.OSXEditor
            )
            {
            }
            else
            {
                if (canvas != null)
                {
                    var size = canvas.GetComponent<RectTransform>().sizeDelta;
                    screenScale = size.x / Screen.width;
                    Screen.SetResolution((int)size.x, (int)size.y, true);
                }
            }
        }

        // 获取某个层最后添加的视图
        private UIInfo GetLastView(int index)
        {
            List<UIInfo> stack;
            if (layerStacks.TryGetValue(index, out stack))
            {
                if (stack.Count > 0)
                {
                    return stack[stack.Count - 1];
                }
            }
            return null;
        }

        // 添加界面到指定层
        protected void AddToLayer(UIInfo uiInfo)
        {
            int layerIndex = (int)uiInfo.info.layer;
            if (uiInfo.info.addMode == UIAddMode.Replace)
            {
                UIInfo lastView = GetLastView(layerIndex);
                if (lastView != null)
                {
                    if (lastView != null && lastView.uiObj != null)
                    {
                        var vb = lastView.uiObj.GetComponent<ViewBinder>();
                        vb.OnWinClose();
                        RemoveViewFromLayer(lastView);
                    }
                }
            }
            Transform layer = GetLayerByIndex(layerIndex);
            uiInfo.uiObj.transform.SetParent(layer);
            layerStacks[layerIndex].Add(uiInfo);
        }

        // 移除指定图层的界面
        private void RemoveViewFromLayer(UIInfo uiInfo)
        {
            if (uiInfo != null)
            {
                Type key = uiInfo.viewClass;
                if (uiCache.ContainsKey(key))
                {
                    uiCache.Remove(key);
                }
                if (uiIdDic.ContainsKey(uiInfo.uiId))
                {
                    uiIdDic.Remove(uiInfo.uiId);
                }
                if (layerStacks[(int)uiInfo.info.layer] != null && layerStacks[(int)uiInfo.info.layer].Count > 0)
                {
                    for (int i = 0; i < layerStacks[(int)uiInfo.info.layer].Count; ++i)
                    {
                        if (layerStacks[(int)uiInfo.info.layer][i] == uiInfo)
                        {
                            layerStacks[(int)uiInfo.info.layer].RemoveAt(i);
                            break;
                        }
                    }
                }
                else
                {
                    Debug.LogFormat("{0}移除界面错误{1}  {2}", key, uiInfo.info.layer, layerStacks[(int)uiInfo.info.layer].Count);
                }
                //移除当前界面的Mediator监听
                if (uiInfo.gameView != null && uiInfo.View != null)
                {
                    uiInfo.View.OnDestroy();
                    Debug.Log("===>>>>移除界面: " + uiInfo.gameView.name);
                }
                else
                {
                    Debug.Log("uiInfo.gameView:" + uiInfo.gameView);
                }
                if (uiInfo.maskObj != null)
                {
                    GameObject.DestroyImmediate(uiInfo.maskObj);
                }
                if (uiInfo.uiObj != null)
                {
                    GameObject.DestroyImmediate(uiInfo.uiObj);
                    Debug.Log("销毁界面:" + uiInfo.assetName);
                }
                uiInfo.ParentUI = null;
                uiInfo.gameView = null;
                uiInfo.uiObj = null;
                UpdatePopViewStatus();
            }
        }

        // 添加蒙版
        private void AddMask(UIInfo uiInfo)
        {
            if (uiInfo.maskStatus == EnumMaskStatus.kNone)
            {
                return;
            }
            if (uiInfo.maskObj != null)
            {
                uiInfo.maskObj.SetActive(true);
                return;
            }
            GameObject maskObj = new GameObject(string.Format("{0}Mask", uiInfo.viewClass.Name),typeof(RectTransform));
            if (maskObj != null)
            {
                maskObj.layer = LayerMask.NameToLayer("UI");
                int layerIndex = (int)uiInfo.info.layer;
                Transform layer = GetLayerByIndex(layerIndex);
                maskObj.transform.SetParent(layer);
                uiInfo.maskObj = maskObj;
                maskObj.transform.SetSiblingIndex(uiInfo.gameView.transform.GetSiblingIndex());
                maskObj.transform.localScale = Vector3.one;
                RectTransform rectTran = maskObj.GetComponent<RectTransform>();
                if(rectTran)
                {
                    rectTran.anchorMax = Vector3.one;
                    rectTran.anchorMin = Vector3.zero;
                    rectTran.sizeDelta = Vector3.zero;
                }

                if (uiInfo.maskStatus == EnumMaskStatus.kTouchCloseAlpha || uiInfo.maskStatus == EnumMaskStatus.kNoMaskNoTouch)
                {
                    Empty4Raycast maskableGraphic = maskObj.AddComponent<Empty4Raycast>();
                }
                else
                {
                    Image maskImg = maskObj.AddComponent<Image>();
                    maskImg.sprite = null;
                    maskImg.color = new Color(0, 0, 0, 0.7f);
                }
                if (uiInfo.maskStatus == EnumMaskStatus.kTouchClose || uiInfo.maskStatus == EnumMaskStatus.kTouchCloseAlpha)
                {
                    maskObj.AddComponent<Button>().onClick.AddListener(() =>
                    {
                        CloseUI(uiInfo);
                    });
                }
            }
        }

        // 添加界面到指定层，设置默认位置
        private void AnchorUIGameObject(UIInfo uiInfo)
        {
            Vector3 anchorPos = Vector3.zero;
            Vector2 sizeDel = Vector2.zero;
            Vector3 scale = Vector3.one;
            RectTransform uiRect = uiInfo.uiObj.GetComponent<RectTransform>();

            if (uiRect != null)
            {
                anchorPos = uiRect.anchoredPosition;
                sizeDel = uiRect.sizeDelta;
                scale = uiRect.localScale;
            }
            else
            {
                anchorPos = uiInfo.uiObj.transform.localPosition;
                scale = uiInfo.uiObj.transform.localScale;
            }
            // 添加到指定层级
            AddToLayer(uiInfo);

            if (uiRect != null)
            {
                uiRect.anchoredPosition = anchorPos;
                uiRect.sizeDelta = sizeDel;
                uiRect.localScale = scale;
            }
            else
            {
                uiInfo.uiObj.transform.localPosition = anchorPos;
                uiInfo.uiObj.transform.localScale = scale;
            }
        }

        private bool CheckNeedStackUI(UIInfo uiInfo)
        {
            if (uiInfo.info.addMode != UIAddMode.Stack)
            {
                return false;
            }
            return true;
        }

        private void LinkUIHandle(UIInfo uiInfo, Action callBack)
        {
            UIInfo[] linkUI = uiInfo.linkUI;
            int count = 0;
            int countMax = linkUI.Length;
            if (count == countMax)
            {
                callBack?.Invoke();
                return;
            }
            foreach (var item in linkUI)
            {
                if (item.ParentUI != uiInfo)
                {
                    item.ParentUI = uiInfo;
                }
                ShowUI(item, () =>
                {
                    count++;
                    if (count == countMax)
                    {
                        callBack?.Invoke();
                    }
                }, null);
            }
        }

        // 异步加载UI
        private void LoadUIAsync(string assetName, UIInfo uiInfo, Type pageName, Action callback, object data)
        {
            if (!uiCache.ContainsKey(pageName))
            {
                uiCache.Add(pageName, uiInfo);
            }
            else
            {
                Debug.LogWarning("已经加载了界面了" + pageName);
                return;
            }

            // 这里到时候需要区分一下是不是热更新里面的。
            GameView tmp = CoreUtils.hotService.Instantiate<GameView>(uiInfo.viewClass);

            tmp.LoadUI(() =>
            {
                GameObject uiObj = tmp.vb.gameObject;
                uiInfo.uiObj = uiObj;
                SetPopViewStatus(uiInfo);
                if (uiObj.name == "ErrorPrefab(Clone)")
                {
                    Debug.LogErrorFormat("Load Prefab Fail: {0}", uiInfo.assetName);
                }

                if (uiObj.activeSelf == false)
                {
                    Debug.LogWarning(string.Format("asset {0} obj is not visable", assetName));
                }

                if (uiInfo.ParentUI != null)
                {
                    uiInfo.View = tmp;
                    uiInfo.gameView = tmp.vb;
                    tmp.data = data;
                    tmp.Start();

                    RectTransform parent = GetUILayer((int)uiInfo.ParentUI.info.layer);
                    if(uiInfo.ParentUI.maskObj)
                    {
                        uiInfo.gameView.transform.SetParent(uiInfo.ParentUI.maskObj.transform);
                    }
                    else
                    {
                        uiInfo.gameView.transform.SetParent(parent);
                    }
                    RectTransform rt = uiInfo.gameView.transform.GetComponent<RectTransform>();
                    rt.offsetMin = Vector2.zero;
                    rt.offsetMax = Vector2.zero;
                    uiInfo.gameView.transform.transform.localScale = Vector3.one;
                    //uiInfo.gameView.transform.transform.SetSiblingIndex(uiInfo.ParentUI.gameView.transform.childCount-1);

                    tmp.vb.OnWinFocus();
                    callback?.Invoke();
                    uiInfo.ParentUI.gameView.onWinCloseCallback += uiInfo.gameView.OnWinClose;
                    m_onShowUIEvent?.Invoke(uiInfo);
                    return;
                }

                uiInfo.gameView = tmp.vb;
                // 添加到指定层
                AnchorUIGameObject(uiInfo);
                uiInfo.View = tmp;
                tmp.data = data;
                tmp.Start();
                // 窗口挂载关闭
                AddCloseButtonEvent(uiInfo);

                tmp.vb.OnWinFocus();
                AddMask(uiInfo);
                LinkUIHandle(uiInfo, () => {
                    //子模块加载完成后再回调
                    callback?.Invoke();
                });
                currentLoadingUI = null;
                m_onShowUIEvent?.Invoke(uiInfo);
            });
        }

        //窗口挂载关闭事件
        private void AddCloseButtonEvent(UIInfo uiInfo)
        {
            if (uiInfo.uiObj == null)
            {
                Debug.LogErrorFormat("assetName:{0}", uiInfo.assetName);
                return;
            }
            Transform but_closeTran = uiInfo.uiObj.transform.Find("btn_close");
            if (but_closeTran != null)
            {
                Button but_close = but_closeTran.gameObject.GetComponent<Button>();
                if (but_close)
                {
                    Debug.Log("find close button");
                    but_close.onClick.AddListener(delegate
                    {
                        CloseUI(uiInfo);
                    });
                }
                return;
            }

            but_closeTran = uiInfo.uiObj.transform.Find("pl_mes/btn_close");

            if (but_closeTran != null)
            {
                Button but_close = but_closeTran.gameObject.GetComponent<Button>();
                if (but_close)
                {
                    Debug.Log("find close button");
                    but_close.onClick.AddListener(delegate
                    {
                        CloseUI(uiInfo);
                    });
                }
            }
        }

        // 窗口关闭
        private void OnCloseUI(UIInfo uiInfo, bool IsManual = false,bool IsForceClose = false)
        {
            if (uiInfo != null && uiInfo.uiObj != null)
            {
                UIInfo[] linkUI = uiInfo.linkUI;
                if (linkUI.Length > 0)
                {
                    if (linkUI[0].ParentUI == uiInfo)
                    {
                        foreach (var item in linkUI)
                        {
                            RemoveViewFromLayer(item);
                        }
                    }
                }

                var vb = uiInfo.uiObj.GetComponent<ViewBinder>();
                vb.OnWinClose();
                if (uiInfo.info.closeMode == UICloseMode.PopWin || uiInfo.info.closeMode == UICloseMode.PopAll)
                {
                    RemoveViewFromLayer(uiInfo);
                }
                else
                {
                    if (uiInfo.info.closeMode == UICloseMode.Hide)
                    {
                        if (IsForceClose)
                        {
                            RemoveViewFromLayer(uiInfo);
                        }
                        else
                        {
                            uiInfo.uiObj.SetActive(false);
                            if (uiInfo.maskObj != null)
                            {
                                uiInfo.maskObj.SetActive(false);
                            }
                            UpdatePopViewStatus();
                        }
                    }
                }


                if (CheckNeedStackUI(uiInfo))
                {
                    bool isTop = false;
                    for (int i = 0; i < uiStack.Count; ++i)
                    {
                        if (uiStack[i] == uiInfo)
                        {
                            isTop = (uiStack.Count - 1 == i);
                            uiStack.RemoveAt(i);
                            break;
                        }
                    }
                    if (uiStack.Count > 0)
                    {
                        if (isTop)
                        {
                            //打开下层全屏窗口，如果有不是全屏的，也要打开
                            m_tempStackUIList.Clear();
                            for (int i = uiStack.Count - 1; i >= 0; --i)
                            {
                                UIInfo uIInfoTemp = uiStack[i];
                                m_tempStackUIList.Add(uIInfoTemp);
                            }
                            for (int i = m_tempStackUIList.Count - 1; i >= 0; --i)
                            {
                                if (m_tempStackUIList[i].assetName != "UI_Win_Guide")
                                {
                                    ShowUI(m_tempStackUIList[i], null, null);
                                }
                            }
                        }
                    }
                    else
                    {
                    }
                }
                m_onCloseUIEvent?.Invoke(uiInfo);
            }
        }

        #endregion
    }
}
