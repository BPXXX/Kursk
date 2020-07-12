// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_StandardButton_Blue_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_StandardButton_Blue_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_StandardButton_Blue";

        public UI_Model_StandardButton_Blue_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_StandardButton_Blue;
		[HideInInspector] public PolygonImage m_btn_languageButton_PolygonImage;
		[HideInInspector] public GameButton m_btn_languageButton_GameButton;

		[HideInInspector] public PolygonImage m_img_img_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_Text_LanguageText;
		[HideInInspector] public Shadow m_lbl_Text_Shadow;



        private void UIFinder()
        {       
			m_UI_Model_StandardButton_Blue = gameObject.GetComponent<RectTransform>();
			m_btn_languageButton_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_languageButton");
			m_btn_languageButton_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_languageButton");

			m_img_img_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_languageButton/img_img");

			m_lbl_Text_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_languageButton/lbl_Text");
			m_lbl_Text_Shadow = FindUI<Shadow>(gameObject.transform ,"btn_languageButton/lbl_Text");


			BindEvent();
        }

        #endregion
    }
}