using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Patterns;
using System;
using LitJson;
using PureMVC.Patterns.Facade;
using Skyunion;
using PureMVC.Patterns.Mediator;
using PureMVC.Interfaces;
using Sproto;
using SprotoType;
using System.IO;

namespace Game
{
    public class AppFacade : Facade
    {
        public const string STARTUP = "AppFacade.StartUp";

        private Dictionary<ViewBinder, IMediator> m_viewMap = new Dictionary<ViewBinder, IMediator>();
        private Dictionary<string, IMediator> m_MediatorMap = new Dictionary<string, IMediator>();
        private Dictionary<string, Mediator> m_globalMap = new Dictionary<string, Mediator>();

        public static AppFacade GetInstance()
        {
            return GetInstance(() => new AppFacade()) as AppFacade;
        }

        protected override void InitializeController()
        {
            base.InitializeController();
            //注册所有的Command
            RegisterCommand(STARTUP, () => new StartupCommand());
            RegisterCommand(CmdConstant.ReloadGame, () => new ReloadGame());
            RegisterCommand(CmdConstant.ResetProxyAnMeida, () => new ResetProxyAnMeida());

            RegisterCommand(CmdConstant.LoginToServer, () => new LoginCommand());
            RegisterCommand(CmdConstant.AutoLogin, () => new LoginCommand());

            RegisterCommand(CmdConstant.LoginPanel, () => new ChangePanelCmd());
            RegisterCommand(CmdConstant.RegisterPanel, () => new ChangePanelCmd());
            RegisterCommand(CmdConstant.MainInterFacePanel, () => new ChangePanelCmd());
            RegisterCommand(CmdConstant.AboutPanel, () => new ChangePanelCmd());
            RegisterCommand(CmdConstant.GameHallPanel, () => new ChangePanelCmd());
            RegisterCommand(CmdConstant.JoinRoomPanel, () => new ChangePanelCmd());
            RegisterCommand(CmdConstant.CreateRoomPanel, () => new ChangePanelCmd());
            RegisterCommand(CmdConstant.GamePreparePanle, () => new ChangePanelCmd());
            RegisterCommand(CmdConstant.GamePrepareAdminPanle, () => new ChangePanelCmd());
            RegisterCommand(CmdConstant.SettingPanel, () => new ChangePanelCmd());
            RegisterCommand(CmdConstant.GameOningPanel, () => new ChangePanelCmd()); 
            RegisterCommand(CmdConstant.SucessPanel, () => new ChangePanelCmd());
            RegisterCommand(CmdConstant.FailurePanel, () => new ChangePanelCmd());

            RegisterCommand(CmdConstant.QueryAccountCmd, () => new QueryAccountCmd());
            RegisterCommand(CmdConstant.RegisterAccountCmd, () => new RegisterAccountCmd()); 
            RegisterCommand(CmdConstant.CreatRoomCmd, () => new CreatRoomCmd());
            RegisterCommand(CmdConstant.JoinRoomCmd, () => new JoinRoomCmd());
            RegisterCommand(CmdConstant.ShowUserInfoCmd, () => new ShowUserInfoCmd());
            RegisterCommand(CmdConstant.ChangeSeatCmd, () => new ChangeSeatCmd());
            RegisterCommand(CmdConstant.StartWarCmd, () => new StartWarCmd()); 
            RegisterCommand(CmdConstant.SingleName, () => new ShowSingleInfoCmd());
            RegisterCommand(CmdConstant.SingleScore, () => new ShowSingleInfoCmd()); 
            RegisterCommand(CmdConstant.TatalScores, () => new ShowSingleInfoCmd());
            //进入主场景
            RegisterCommand(CmdConstant.EnterScenceCmd, () => new EnterScenceCmd());
            RegisterCommand(CmdConstant.S2CcontrolTank, () => new S2CcontrolTankCmd());
            RegisterCommand(CmdConstant.S2CcontrolHp, () => new S2CcontrolHpCmd());
            RegisterCommand(CmdConstant.InitTank, () => new InitTankCmd());
           // RegisterCommand(CmdConstant.CreateOtherTank, () => new CreateOtherTankCmd());
            RegisterCommand(CmdConstant.CreateAllTank, () => new CreateAllTankCmd());
            RegisterCommand(CmdConstant.InitPosition, () => new InitPositionCmd());
            RegisterCommand(CmdConstant.CreateAmmo, () => new CreateAmmoCmd());
            RegisterCommand(CmdConstant.AddScores, () => new AddSocresCmd());
            

        }

        public void StartUp()
        {
            SendNotification(STARTUP);
        }

        public override void RegisterMediator(IMediator mediator)
        {
            ViewBinder vb = null;
            if (mediator.ViewComponent != null)
            {
                vb = ((GameObject) mediator.ViewComponent).GetComponent<ViewBinder>();
            }

            if (vb != null)
            {
                m_viewMap[vb] = mediator;
            }

            if (!m_MediatorMap.ContainsKey(mediator.MediatorName))
            {
                m_MediatorMap[mediator.MediatorName] = mediator;
            }

            base.RegisterMediator(mediator);
        }

        public override IMediator RetrieveMediator(string mediatorName)
        {
            if (m_MediatorMap.ContainsKey(mediatorName))
            {
                return m_MediatorMap[mediatorName];
            }

            return base.RetrieveMediator(mediatorName);
        }

        public override IMediator RemoveMediator(string mediatorName)
        {
            if (m_MediatorMap.ContainsKey(mediatorName))
            {
                m_MediatorMap.Remove(mediatorName);
            }

            return base.RemoveMediator(mediatorName);
        }

        public void RemoveView(ViewBinder view)
        {
            if (m_viewMap.ContainsKey(view))
            {
                IMediator md = m_viewMap[view];
                AppFacade.GetInstance().RemoveMediator(md.MediatorName);
            }
        }

        public void RegisterGlobalMediator(Mediator mediator)
        {
            m_globalMap.Add(mediator.MediatorName, mediator);
            base.RegisterMediator(mediator);
        }

        public void RemoveGlobalMediator(string mediatorName)
        {
            if (m_globalMap.ContainsKey(mediatorName))
            {
                Mediator md = m_globalMap[mediatorName];
                AppFacade.GetInstance().RemoveMediator(md.MediatorName);
                m_globalMap.Remove(mediatorName);
            }
        }

        public void SendPBMsg(UInt16 id, MemoryStream stream)
        {
            var net = RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
            net.SendPBMsg(id, stream);
        }
    }
}