// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_SingleRoom_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_SingleRoom_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_SingleRoom";

        public UI_SingleRoom_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_UI_SingleRoom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_Map_PolygonImage;

		[HideInInspector] public PolygonImage m_img_RoomName_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_RoomNameText_LanguageText;

		[HideInInspector] public UI_JoinRoomBtn_SubView m_UI_JoinRoomBtn;


        private void UIFinder()
        {       
			m_UI_SingleRoom_PolygonImage = gameObject.GetComponent<PolygonImage>();

			m_img_Map_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_Map");

			m_img_RoomName_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_RoomName");

			m_lbl_RoomNameText_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_RoomNameText");

			m_UI_JoinRoomBtn = new UI_JoinRoomBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_JoinRoomBtn"));

			BindEvent();
        }

        #endregion
    }
}