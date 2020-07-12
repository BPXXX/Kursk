// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_Window_TypeMid_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_Window_TypeMid_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_Window_TypeMid";

        public UI_Model_Window_TypeMid_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_Window_TypeMid;
		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public Shadow m_lbl_title_Shadow;

		[HideInInspector] public PolygonImage m_btn_close_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_GameButton;

		[HideInInspector] public PolygonImage m_btn_back_PolygonImage;
		[HideInInspector] public GameButton m_btn_back_GameButton;



        private void UIFinder()
        {       
			m_UI_Model_Window_TypeMid = gameObject.GetComponent<RectTransform>();
			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"bg/lbl_title");
			m_lbl_title_Shadow = FindUI<Shadow>(gameObject.transform ,"bg/lbl_title");

			m_btn_close_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"bg/btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(gameObject.transform ,"bg/btn_close");

			m_btn_back_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"bg/btn_back");
			m_btn_back_GameButton = FindUI<GameButton>(gameObject.transform ,"bg/btn_back");


			BindEvent();
        }

        #endregion
    }
}