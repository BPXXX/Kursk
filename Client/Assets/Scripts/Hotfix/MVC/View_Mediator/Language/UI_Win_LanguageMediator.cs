// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月7日
// Update Time         :    2020年2月7日
// Class Description   :    UI_Win_LanguageMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using SprotoType;
using UnityEngine.UI;

namespace Game {
    public class UI_Win_LanguageMediator : GameMediator
    {
        public enum SureType
        {
            None = 0,
            CloseAll = 1,
            OpenLoginView = 2,
            DonotClose = 4,
            AlwaysSave = 8,
            ResetScene = 16,
        }
        #region Member
        public static string NameMediator = "UI_Win_LanguageMediator";

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private List<Data.LanguageSetDefine> m_languages = new List<Data.LanguageSetDefine>();
        private int m_curLanguage;
        private int m_sureType = 0;

        #endregion

        //IMediatorPlug needs
        public UI_Win_LanguageMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_LanguageView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                default:
                    break;
            }
        }

       

        #region UI template method

        public override void OpenAniEnd(){

        }

        public override void WinFocus(){
            
        }

        public override void WinClose(){
            
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_sureType = (int)view.data;
            m_curLanguage = (int)LanguageUtils.GetLanguage();
            ClientUtils.PreLoadRes(view.gameObject, view.m_sv_list_view_ListView.ItemPrefabDataList, LoadFinish);
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {
            view.m_UI_Model_StandardButton_Blue_sure.AddClickEvent(OnSureClick);
            view.m_UI_Model_Window_Type2.setCloseHandle(OnCloseClick);
            view.m_UI_Model_Window_Type2.setBackHandle(OnCloseClick);
        }

        #endregion
        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }

            InitUI();
        }

        private ToggleGroup m_group;
        private void InitUI()
        {
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = OnItemEnter;
            view.m_sv_list_view_ListView.SetInitData(m_assetDic, functab);

            var langages = CoreUtils.dataService.QueryRecords<Data.LanguageSetDefine>();
            for (int i = 0; i < langages.Count; i++)
            {
                var lanConfig = langages[i];
                if (lanConfig.enumSwitch == 0)
                    continue;
                m_languages.Add(lanConfig);
            }
            m_group = view.m_sv_list_view_ListView.gameObject.AddComponent<ToggleGroup>();
            view.m_sv_list_view_ListView.FillContent((m_languages.Count + 1) / 2);
        }

        private void OnValueChange(int id)
        {
            if (m_curLanguage == id)
                return;
            m_curLanguage = id;

            //LanguageUtils.SetLanguage((SystemLanguage)m_curLanguage);
        }
        private void OnItemEnter(ListView.ListItem listItem)
        {
            UI_LC_Language_SubView subView;
            if (listItem.isInit == false)
            {
                subView = new UI_LC_Language_SubView(listItem.go.GetComponent<RectTransform>());
                subView.BindGroup(m_group);
                listItem.data = subView;
                listItem.isInit = true;
                subView.AddValueChange(OnValueChange);
            }
            else
            {
                subView = (UI_LC_Language_SubView)listItem.data;
            }
            int left = m_languages[listItem.index * 2].ID;
            int right = -1;
            if (listItem.index * 2 + 1 < m_languages.Count)
            {
                right = m_languages[listItem.index * 2 + 1].ID;
            }
            subView.SetLanguageId(left, right);
            subView.Selected(m_curLanguage);
        }
        private void OnSureClick()
        {
            if ((int)LanguageUtils.GetLanguage() != m_curLanguage)
            {
                LanguageUtils.SetLanguage((SystemLanguage)m_curLanguage);
                LanguageUtils.SaveCache();
            }
            else
            {
                if ((m_sureType & (int)SureType.AlwaysSave) != 0)
                {
                    LanguageUtils.SetLanguage((SystemLanguage)m_curLanguage);
                    LanguageUtils.SaveCache();
                }
                else
                {
                    OnCloseClick();
                    return;
                }
            }

            if ((m_sureType & (int)SureType.CloseAll) != 0)
            {
                CoreUtils.uiManager.CloseAll();
            }
            else
            {
                CoreUtils.uiManager.CloseUI(UI.s_Pop_Language);
            }

            //if ((m_sureType & (int)SureType.ResetScene) != 0)
            //{
            //    AppFacade.GetInstance().SendNotification(CmdConstant.ResetSceneCmd, null);
            //}
            if ((m_sureType & (int)SureType.OpenLoginView) != 0)
            {
                CoreUtils.uiManager.ShowUI(UI.s_LoadingView);
            }
        }
        private void OnCloseClick()
        {
            if ((m_sureType & (int)SureType.DonotClose) != 0)
            {
                return;
            }
            else
            {
                CoreUtils.uiManager.CloseUI(UI.s_Pop_Language);
            }
        }
    }
}