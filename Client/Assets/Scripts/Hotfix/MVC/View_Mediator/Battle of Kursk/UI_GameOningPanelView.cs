// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月3日
// Update Time         :    2020年6月3日
// Class Description   :    UI_GameOningPanelView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_GameOningPanelView : GameView
    {
        public const string VIEW_NAME = "UI_GameOningPanel";

        public UI_GameOningPanelView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_polygonImage_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;

		[HideInInspector] public LanguageText m_lbl_RedScores_LanguageText;

		[HideInInspector] public LanguageText m_lbl_BlueScores_LanguageText;

		[HideInInspector] public LanguageText m_lbl_SingleScoreStr_LanguageText;

		[HideInInspector] public LanguageText m_lbl_SingleScore_LanguageText;



        private void UIFinder()
        {
			m_img_polygonImage_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_polygonImage");

			m_lbl_languageText_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_languageText");

			m_lbl_RedScores_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_RedScores");

			m_lbl_BlueScores_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_BlueScores");

			m_lbl_SingleScoreStr_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_SingleScoreStr");

			m_lbl_SingleScore_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_SingleScore");


            UI_GameOningPanelMediator mt = new UI_GameOningPanelMediator(vb.gameObject);
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
