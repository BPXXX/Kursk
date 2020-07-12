// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_BG_StandardButton_Big_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_BG_StandardButton_Big_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_BG_StandardButton_Big";

        public UI_BG_StandardButton_Big_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_BG_StandardButton_Big;
		[HideInInspector] public PolygonImage m_btn_bgButton_PolygonImage;
		[HideInInspector] public GameButton m_btn_bgButton_GameButton;

		[HideInInspector] public PolygonImage m_img_img_PolygonImage;



        private void UIFinder()
        {       
			m_UI_BG_StandardButton_Big = gameObject.GetComponent<RectTransform>();
			m_btn_bgButton_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_bgButton");
			m_btn_bgButton_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_bgButton");

			m_img_img_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_bgButton/img_img");


			BindEvent();
        }

        #endregion
    }
}