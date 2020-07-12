// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月28日
// Update Time         :    2020年5月28日
// Class Description   :    UI_GameHallInterfaceView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_GameHallInterfaceView : GameView
    {
        public const string VIEW_NAME = "UI_GameHallInterface";

        public UI_GameHallInterfaceView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_headline_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bg_tank_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bg_RoomInfo_PolygonImage;

		[HideInInspector] public PolygonImage m_img_RoomMap_PolygonImage;

		[HideInInspector] public PolygonImage m_img_RoomInfo_PolygonImage;

		[HideInInspector] public UI_BackMainMenuBtn_SubView m_UI_BackMainMenuBtn;
		[HideInInspector] public UI_CreatRoomBtn_SubView m_UI_CreatRoomBtn;
		[HideInInspector] public UI_SingleRoom_SubView m_UI_SingleRoom;


        private void UIFinder()
        {
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_img_headline_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_headline");

			m_img_bg_tank_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg_tank");

			m_img_bg_RoomInfo_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg_RoomInfo");

			m_img_RoomMap_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg_RoomInfo/img_RoomMap");

			m_img_RoomInfo_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg_RoomInfo/img_RoomInfo");

			m_UI_BackMainMenuBtn = new UI_BackMainMenuBtn_SubView(FindUI<RectTransform>(vb.transform ,"UI_BackMainMenuBtn"));
			m_UI_CreatRoomBtn = new UI_CreatRoomBtn_SubView(FindUI<RectTransform>(vb.transform ,"UI_CreatRoomBtn"));
			m_UI_SingleRoom = new UI_SingleRoom_SubView(FindUI<RectTransform>(vb.transform ,"UI_SingleRoom"));

            UI_GameHallInterfaceMediator mt = new UI_GameHallInterfaceMediator(vb.gameObject);
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
