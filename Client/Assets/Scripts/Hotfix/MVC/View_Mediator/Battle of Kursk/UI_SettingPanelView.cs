// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月29日
// Update Time         :    2020年5月29日
// Class Description   :    UI_SettingPanelView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_SettingPanelView : GameView
    {
        public const string VIEW_NAME = "UI_SettingPanel";

        public UI_SettingPanelView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;

		[HideInInspector] public UI_Button_Sure_SubView m_UI_Button_Sure;
		[HideInInspector] public UI_CancelButton_SubView m_UI_CancelButton;


        private void UIFinder()
        {
			m_lbl_languageText_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_languageText");

			m_UI_Button_Sure = new UI_Button_Sure_SubView(FindUI<RectTransform>(vb.transform ,"UI_Button_Sure"));
			m_UI_CancelButton = new UI_CancelButton_SubView(FindUI<RectTransform>(vb.transform ,"UI_CancelButton"));

            UI_SettingPanelMediator mt = new UI_SettingPanelMediator(vb.gameObject);
            mt.view = this;
            AppFacade.GetInstance().RegisterMediator(mt);
			if(mt.IsOpenUpdate)
			{
                vb.fixedUpdateCallback = mt.FixedUpdate;
                vb.lateUpdateCallback = mt.LateUpdate;
				vb.updateCallback = mt.Update;
			}
            vb.openAniEndCallback = mt.OpenAniEnd;
            vb.onWinFocusCallback = mt.WinFocus;
            vb.onWinCloseCallback = mt.WinClose;
            vb.onPrewarmCallback = mt.PrewarmComplete;
        }

        #endregion

        public override void Start () {
            UIFinder();
    	}
        public override void OnDestroy()
        {
            AppFacade.GetInstance().RemoveView(vb);
        }

    }
}
