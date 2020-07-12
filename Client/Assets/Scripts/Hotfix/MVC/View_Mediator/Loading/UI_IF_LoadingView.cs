// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月7日
// Update Time         :    2020年2月7日
// Class Description   :    UI_IF_LoadingView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_IF_LoadingView : GameView
    {
        public const string VIEW_NAME = "UI_IF_Loading";

        public UI_IF_LoadingView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_version_LanguageText;

		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;

		[HideInInspector] public LanguageText m_lbl_doing_LanguageText;

		[HideInInspector] public LanguageText m_lbl_Tip_LanguageText;



        private void UIFinder()
        {
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_lbl_version_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_version");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(vb.transform ,"pb_rogressBar");

			m_lbl_doing_LanguageText = FindUI<LanguageText>(vb.transform ,"pb_rogressBar/lbl_doing");

			m_lbl_Tip_LanguageText = FindUI<LanguageText>(vb.transform ,"pb_rogressBar/lbl_Tip");


            UI_IF_LoadingMediator mt = new UI_IF_LoadingMediator(vb.gameObject);
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
