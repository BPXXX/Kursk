// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月23日
// Update Time         :    2020年5月23日
// Class Description   :    UI_CreatRoomPanelView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_CreatRoomPanelView : GameView
    {
        public const string VIEW_NAME = "UI_CreatRoomPanel";

        public UI_CreatRoomPanelView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_RoomName_PolygonImage;

		[HideInInspector] public PolygonImage m_ipt_RoomName_PolygonImage;
		[HideInInspector] public GameInput m_ipt_RoomName_GameInput;

		[HideInInspector] public UI_Button_Sure_SubView m_UI_Button_Sure;
		[HideInInspector] public UI_CancelButton_SubView m_UI_CancelButton;


        private void UIFinder()
        {
			m_img_bg_RoomName_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg_RoomName");

			m_ipt_RoomName_PolygonImage = FindUI<PolygonImage>(vb.transform ,"ipt_RoomName");
			m_ipt_RoomName_GameInput = FindUI<GameInput>(vb.transform ,"ipt_RoomName");

			m_UI_Button_Sure = new UI_Button_Sure_SubView(FindUI<RectTransform>(vb.transform ,"UI_Button_Sure"));
			m_UI_CancelButton = new UI_CancelButton_SubView(FindUI<RectTransform>(vb.transform ,"UI_CancelButton"));

            UI_CreatRoomPanelMediator mt = new UI_CreatRoomPanelMediator(vb.gameObject);
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
