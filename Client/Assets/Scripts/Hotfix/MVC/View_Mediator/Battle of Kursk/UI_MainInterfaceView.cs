// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月17日
// Update Time         :    2020年5月17日
// Class Description   :    UI_MainInterfaceView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_MainInterfaceView : GameView
    {
        public const string VIEW_NAME = "UI_MainInterface";

        public UI_MainInterfaceView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_Chinese_PolygonImage;

		[HideInInspector] public PolygonImage m_img_English_PolygonImage;

		[HideInInspector] public UI_StartGameButton_SubView m_UI_StartGameButton;
		[HideInInspector] public UI_ExitGameButton_SubView m_UI_ExitGameButton;
		[HideInInspector] public UI_SettingButton_SubView m_UI_SettingButton;
		[HideInInspector] public UI_AboutButton_SubView m_UI_AboutButton;


        private void UIFinder()
        {
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_img_Chinese_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_Chinese");

			m_img_English_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_English");

			m_UI_StartGameButton = new UI_StartGameButton_SubView(FindUI<RectTransform>(vb.transform ,"UI_StartGameButton"));
			m_UI_ExitGameButton = new UI_ExitGameButton_SubView(FindUI<RectTransform>(vb.transform ,"UI_ExitGameButton"));
			m_UI_SettingButton = new UI_SettingButton_SubView(FindUI<RectTransform>(vb.transform ,"UI_SettingButton"));
			m_UI_AboutButton = new UI_AboutButton_SubView(FindUI<RectTransform>(vb.transform ,"UI_AboutButton"));

            UI_MainInterfaceMediator mt = new UI_MainInterfaceMediator(vb.gameObject);
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
