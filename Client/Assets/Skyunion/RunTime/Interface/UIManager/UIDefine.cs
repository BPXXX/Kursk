using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Skyunion
{
    /// <summary>
    /// 界面类型
    /// </summary>
    public enum UIViewType
    {
        hud,        //地图上的UI
        FullView,   //全屏界面
        Window,     //弹出窗口
        Fix         //固定位置
    }
    /// <summary>
    /// 界面显示的层级
    /// </summary>
    public enum UILayer
    {
        HUDLayer = 0,
        FullViewLayer = 1,
        FullViewMenuLayer = 2,
        ChatLayer = 3,
        StoryLayer = 4,
        WindowLayer = 5,
        WindowMenuLayer = 6,
        WindowPopLayer = 7,
        GuideLayer = 8,
        TipLayer = 9,
        SystemOpenLayer = 10,
        LoadingLayer = 11,
        BrowserLayer = 12
    }

    public enum UICloseMode
    {
        PopAll,         //弹出所有窗口界面
        PopWin,         //弹出窗口
        Hide,           //隐藏，缓存下次使用
        Move,           //移动到边界
        Recycle,        //频繁销毁的界面需要回收在用
    }

    public enum UIAddMode
    {
        Replace,        //替换界面
        Stack           //堆叠模式
    }

    /// <summary>
    /// 界面显示，图层，等自定义信息
    /// </summary>
    public class UIViewInfo
    {
        /// <summary>
        /// 窗口类型  全屏|窗口|固定
        /// </summary>
        public UIViewType viewType;
        /// <summary>
        /// 窗口层级
        /// </summary>
        public UILayer layer;
        /// <summary>
        /// 窗口模式
        /// </summary>
        public UIAddMode addMode;
        /// <summary>
        /// 出口关闭类型
        /// </summary>
        public UICloseMode closeMode;

        public UIViewInfo(UIViewType _viewType, UILayer _layer, UIAddMode _addMode, UICloseMode _closeMode)
        {
            viewType = _viewType;
            addMode = _addMode;
            closeMode = _closeMode;
            layer = _layer;
        }
    }
    /// <summary>
    /// 蒙版状态
    /// </summary>
    public enum EnumMaskStatus
    {
        kNone = 0, // 不创建蒙版
        kOnlyShow, // 只显示,点击不做响应
        kTouchClose, // 点击蒙版关闭界面
        kTouchCloseAlpha, // 点击透明蒙板关闭界面
        kNoMaskNoTouch,   //没有蒙板, 点击不做响应
    }

    /// <summary>
    /// 界面资源和显示信息
    /// </summary>
    public class UIInfo
    {
        public string assetName;
        public UIViewInfo info;
        public int uiId;

        public Type viewClass;

        public GameView View;
        public ViewBinder gameView;

        public GameObject uiObj;

        public bool autoPlayUISound = true;

        public string OpenSound = "Sound_Ui_CommonOpenUi";
        public string CloseSound = "Sound_Ui_CommonCloseUi";

        /// <summary>
        /// 窗口蒙版层
        /// </summary>
        public GameObject maskObj = null;

        public EnumMaskStatus maskStatus = EnumMaskStatus.kNone;

        /// <summary>
        /// 关联界面开启
        /// </summary>
        public UIInfo[] linkUI = { };
        public UIInfo ParentUI;

        public UIInfo(string assetName, Type viewClass, UIViewInfo info,
            EnumMaskStatus maskStatus = EnumMaskStatus.kNone, UIInfo[] uiInfos = null, int uiId = 0)
        {
            this.assetName = assetName;
            this.info = info;
            this.viewClass = viewClass;
            this.maskStatus = maskStatus;

            if (uiInfos != null)
            {
                this.linkUI = uiInfos;
            }
            this.uiId = uiId;
        }

        public UIInfo SetSoundAssetName(string openSound,string closeSound)
        {
            if(!string.IsNullOrEmpty(openSound))
            {
                this.OpenSound = openSound;
            }
            if (!string.IsNullOrEmpty(closeSound))
            {
                this.CloseSound = closeSound;
            }
            return this;
        }

        public UIInfo SetMute()
        {
            this.autoPlayUISound = false;
            return this;
        }
    }
}
