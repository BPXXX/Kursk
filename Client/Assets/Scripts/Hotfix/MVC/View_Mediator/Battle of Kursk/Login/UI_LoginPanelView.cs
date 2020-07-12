// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月17日
// Update Time         :    2020年5月17日
// Class Description   :    UI_ LoginPanelView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_LoginPanelView : GameView
    {
        public const string VIEW_NAME = "UI_LoginPanel";

        public UI_LoginPanelView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_ipt_AccountInputField_PolygonImage;
		[HideInInspector] public GameInput m_ipt_AccountInputField_GameInput;

		[HideInInspector] public PolygonImage m_ipt_PasswordsInputField_PolygonImage;
		[HideInInspector] public GameInput m_ipt_PasswordsInputField_GameInput;

		[HideInInspector] public UI_Button_Sure_SubView m_UI_Button_Sure;
		[HideInInspector] public UI_CancelButton_SubView m_UI_CancelButton;
		[HideInInspector] public UI_RegistrationButton_SubView m_UI_RegistrationButton;


        private void UIFinder()
        {
			m_ipt_AccountInputField_PolygonImage = FindUI<PolygonImage>(vb.transform ,"ipt_AccountInputField");
			m_ipt_AccountInputField_GameInput = FindUI<GameInput>(vb.transform ,"ipt_AccountInputField");

			m_ipt_PasswordsInputField_PolygonImage = FindUI<PolygonImage>(vb.transform ,"ipt_PasswordsInputField");
			m_ipt_PasswordsInputField_GameInput = FindUI<GameInput>(vb.transform ,"ipt_PasswordsInputField");

			m_UI_Button_Sure = new UI_Button_Sure_SubView(FindUI<RectTransform>(vb.transform ,"UI_Button_Sure"));
			m_UI_CancelButton = new UI_CancelButton_SubView(FindUI<RectTransform>(vb.transform ,"UI_CancelButton"));
			m_UI_RegistrationButton = new UI_RegistrationButton_SubView(FindUI<RectTransform>(vb.transform ,"UI_RegistrationButton"));

            UI_LoginPanelMediator mt = new UI_LoginPanelMediator(vb.gameObject);
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
