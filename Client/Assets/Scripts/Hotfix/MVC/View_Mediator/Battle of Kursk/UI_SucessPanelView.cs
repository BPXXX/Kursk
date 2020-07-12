// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月5日
// Update Time         :    2020年6月5日
// Class Description   :    UI_SucessPanelView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_SucessPanelView : GameView
    {
        public const string VIEW_NAME = "UI_SucessPanel";

        public UI_SucessPanelView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_ScoreName_LanguageText;

		[HideInInspector] public LanguageText m_lbl_SingleScores_LanguageText;

		[HideInInspector] public UI_ExitGameButton_SubView m_UI_ExitGameButton;


        private void UIFinder()
        {
			m_lbl_ScoreName_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_ScoreName");

			m_lbl_SingleScores_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_SingleScores");

			m_UI_ExitGameButton = new UI_ExitGameButton_SubView(FindUI<RectTransform>(vb.transform ,"UI_ExitGameButton"));

            UI_SucessPanelMediator mt = new UI_SucessPanelMediator(vb.gameObject);
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
