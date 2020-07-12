
using PureMVC.Interfaces;
using Skyunion;
using UnityEngine;
namespace Game
{
    public class ChangePanelCmd : GameCmd
    {
        public override void Execute(INotification notification)
        {
           switch(notification.Name)
            {
                case CmdConstant.MainInterFacePanel:
                    {
                        CoreUtils.uiManager.CloseAll();
                        CoreUtils.uiManager.ShowUI(UI.s_MainInterface, null);
                    }
                    break;
                case CmdConstant.LoginPanel:
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_LoginInterface, null);
                    }
                    break;
                case CmdConstant.RegisterPanel:
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_RegisterInterface, null);
                    }
                    break;
                case CmdConstant.AboutPanel:
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_AboutInterface, null);
                    }
                    break;
                case CmdConstant.SettingPanel:
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_SettingInterface, null);
                    }
                    break;
                case CmdConstant.GameHallPanel:
                    {
                        CoreUtils.uiManager.CloseAll();
                        CoreUtils.uiManager.ShowUI(UI.s_GameHallInterface, null);
                    }
                    break;
                case CmdConstant.CreateRoomPanel:
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_CreateRoomInterface, null);
                    }
                    break; 
                case CmdConstant.JoinRoomPanel:
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_JoinRoomInterface, null);
                    }
                    break;
                case CmdConstant.GamePrepareAdminPanle:
                    {
                        CoreUtils.uiManager.CloseAll();
                        CoreUtils.uiManager.ShowUI(UI.s_GamePrepareAdminInterface, () =>
                        {
                            int xmg = UI_CreatRoomPanelMediator.CreateRoomid;
                            SendNotification(CmdConstant.ShowUserInfoCmd, xmg);
                        });  
                    }
                    break;
                case CmdConstant.GamePreparePanle:
                    {
                        CoreUtils.uiManager.CloseAll();
                        CoreUtils.uiManager.ShowUI(UI.s_GamePrepareInterface, () =>
                        {
                            int xmg = UI_JoinRoomPanelMediator.JoinRoomId;
                            SendNotification(CmdConstant.ShowUserInfoCmd, xmg);
                        });
                    }
                    break;
                case CmdConstant.GameOningPanel:
                    {
                        CoreUtils.uiManager.CloseAll();
                        CoreUtils.uiManager.ShowUI(UI.s_GameOningInterface, () =>
                        {
                            SendNotification(CmdConstant.SingleName);
                        });
                    }
                    break;
                case CmdConstant.SucessPanel:
                    {
                        
                        CoreUtils.uiManager.ShowUI(UI.s_SucessInterface, () =>
                        {
                            Time.timeScale = 0;
                        });
                    }
                    break;
                case CmdConstant.FailurePanel:
                    {
                        
                        CoreUtils.uiManager.ShowUI(UI.s_FailureInterface, () =>
                        {
                            SendNotification(CmdConstant.SingleScore);
                            Time.timeScale = 0;
                        });
                    }
                    break;
            }
        }
    }
}

