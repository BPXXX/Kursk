using Skyunion;
using Game;
using System.Collections.Generic;

public class UI
{

    #region ViewInfo
    public static UIViewInfo s_hudView = new UIViewInfo(UIViewType.hud, UILayer.HUDLayer, UIAddMode.Stack, UICloseMode.PopWin);

    //加载界面模型
    public static UIViewInfo s_loading = new UIViewInfo(UIViewType.FullView, UILayer.LoadingLayer, UIAddMode.Stack, UICloseMode.Hide);
    public static UIViewInfo s_battleLoading = new UIViewInfo(UIViewType.FullView, UILayer.LoadingLayer, UIAddMode.Stack, UICloseMode.PopWin);
    //战斗loading
    public static UIViewInfo s_story = new UIViewInfo(UIViewType.FullView, UILayer.StoryLayer, UIAddMode.Stack, UICloseMode.PopWin);
    //全屏菜单
    public static UIViewInfo s_fullViewMenu = new UIViewInfo(UIViewType.FullView, UILayer.FullViewMenuLayer, UIAddMode.Stack, UICloseMode.Hide);
    //全屏
    public static UIViewInfo s_windowMenu = new UIViewInfo(UIViewType.FullView, UILayer.WindowMenuLayer, UIAddMode.Stack, UICloseMode.PopWin);

    //菜单
    public static UIViewInfo s_fullViewMenuClose = new UIViewInfo(UIViewType.FullView, UILayer.FullViewMenuLayer, UIAddMode.Stack, UICloseMode.PopWin);


    //聊天窗口
    public static UIViewInfo s_chat = new UIViewInfo(UIViewType.Window, UILayer.ChatLayer, UIAddMode.Stack, UICloseMode.Hide);

    //全屏窗口模型
    public static UIViewInfo s_fullWindow = new UIViewInfo(UIViewType.FullView, UILayer.FullViewLayer, UIAddMode.Replace, UICloseMode.PopWin);

    //弹出窗口模型
    public static UIViewInfo s_popWin = new UIViewInfo(UIViewType.Window, UILayer.WindowLayer, UIAddMode.Stack, UICloseMode.PopWin);
    //弹出窗口的弹出窗口模型
    public static UIViewInfo s_popWinPop = new UIViewInfo(UIViewType.Window, UILayer.WindowPopLayer, UIAddMode.Stack, UICloseMode.PopWin);

    //清空所有窗口模型
    public static UIViewInfo s_popAllWin = new UIViewInfo(UIViewType.Window, UILayer.WindowLayer, UIAddMode.Stack, UICloseMode.PopAll);

    //弹出窗口模型,关闭是隐藏
    public static UIViewInfo s_popWinHide = new UIViewInfo(UIViewType.Window, UILayer.WindowLayer, UIAddMode.Stack, UICloseMode.Hide);
    //弹出窗口模型,关闭是隐藏
    public static UIViewInfo s_popWinPopHide = new UIViewInfo(UIViewType.Window, UILayer.WindowPopLayer, UIAddMode.Stack, UICloseMode.Hide);
    //功能开启
    public static UIViewInfo s_systemOpen = new UIViewInfo(UIViewType.Window, UILayer.SystemOpenLayer, UIAddMode.Stack, UICloseMode.PopWin);
    //引导
    public static UIViewInfo s_guide = new UIViewInfo(UIViewType.Window, UILayer.GuideLayer, UIAddMode.Stack, UICloseMode.Hide);
    //浏览器
    public static UIViewInfo s_browserViewInfo = new UIViewInfo(UIViewType.FullView,UILayer.BrowserLayer,UIAddMode.Replace,UICloseMode.PopWin);
    #endregion


    //-----------------------------------------Logic Info Start Here--------------------------------------------------------------------

    #region 通用菜单模型
    #endregion

    #region 公共模型
    #endregion
    #region 全屏模型
    public static UIInfo s_LoadingView = new UIInfo(UI_IF_LoadingView.VIEW_NAME, typeof(UI_IF_LoadingView), s_fullWindow, EnumMaskStatus.kNone);
    public static UIInfo UI_chooseroom = new UIInfo(UI_IF_LoadingView.VIEW_NAME, typeof(UI_IF_LoadingView), s_fullWindow, EnumMaskStatus.kNone);
    #endregion

    #region 弹出窗口模型
    public static UIInfo s_Pop_Language = new UIInfo(UI_Win_LanguageView.VIEW_NAME, typeof(UI_Win_LanguageView), s_popWin, EnumMaskStatus.kOnlyShow);
    public static UIInfo s_gameTool = new UIInfo("UI_Win_GameTool", typeof(GameToolView), s_popWinPop, EnumMaskStatus.kNone);

    public static UIInfo s_test = new UIInfo(HelpTipView.VIEW_NAME, typeof(sendmessageView), s_popWinPop, EnumMaskStatus.kOnlyShow);

    //提示界面
    public static UIInfo s_helpTip = new UIInfo(HelpTipView.VIEW_NAME, typeof(HelpTipView), s_popWinPop, EnumMaskStatus.kTouchCloseAlpha);


    //lyh
    public static UIInfo s_MainInterface = new UIInfo(UI_MainInterfaceView.VIEW_NAME, typeof(UI_MainInterfaceView), s_fullWindow, EnumMaskStatus.kNone);
    public static UIInfo s_LoginInterface = new UIInfo(UI_LoginPanelView.VIEW_NAME, typeof(UI_LoginPanelView), s_popWin, EnumMaskStatus.kTouchCloseAlpha);
    public static UIInfo s_RegisterInterface = new UIInfo(UI_RegisterPanelView.VIEW_NAME, typeof(UI_RegisterPanelView), s_popWinPop, EnumMaskStatus.kTouchCloseAlpha);
    public static UIInfo s_AboutInterface = new UIInfo(UI_AboutPanelView.VIEW_NAME, typeof(UI_AboutPanelView), s_popWinPop, EnumMaskStatus.kTouchCloseAlpha);
    public static UIInfo s_SettingInterface = new UIInfo(UI_SettingPanelView.VIEW_NAME, typeof(UI_SettingPanelView), s_popWinPop, EnumMaskStatus.kTouchCloseAlpha);
    public static UIInfo s_GameHallInterface = new UIInfo(UI_GameHallInterfaceView.VIEW_NAME, typeof(UI_GameHallInterfaceView), s_fullWindow, EnumMaskStatus.kNone);
    public static UIInfo s_CreateRoomInterface = new UIInfo(UI_CreatRoomPanelView.VIEW_NAME, typeof(UI_CreatRoomPanelView), s_popWinPop, EnumMaskStatus.kTouchCloseAlpha);
    public static UIInfo s_JoinRoomInterface = new UIInfo(UI_JoinRoomPanelView.VIEW_NAME, typeof(UI_JoinRoomPanelView), s_popWinPop, EnumMaskStatus.kTouchCloseAlpha);
    public static UIInfo s_GamePrepareInterface = new UIInfo(UI_GamePrepareInterfaceView.VIEW_NAME, typeof(UI_GamePrepareInterfaceView), s_fullWindow, EnumMaskStatus.kNone);
    public static UIInfo s_GamePrepareAdminInterface = new UIInfo(UI_GamePrepareInterface_adminView.VIEW_NAME, typeof(UI_GamePrepareInterface_adminView), s_fullWindow, EnumMaskStatus.kNone);
    public static UIInfo s_GameOningInterface= new UIInfo(UI_GameOningPanelView.VIEW_NAME, typeof(UI_GameOningPanelView), s_fullWindow, EnumMaskStatus.kNone);
    public static UIInfo s_SucessInterface = new UIInfo(UI_SucessPanelView.VIEW_NAME, typeof(UI_SucessPanelView), s_popWinPop, EnumMaskStatus.kNone);
    public static UIInfo s_FailureInterface = new UIInfo(UI_FailurePanelView.VIEW_NAME, typeof(UI_FailurePanelView), s_popWinPop, EnumMaskStatus.kNone);
    #endregion

    //-----------------------------------------Logic Info End Here--------------------------------------------------------------------
}
