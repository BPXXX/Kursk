// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月6日
// Update Time         :    2020年1月6日
// Class Description   :    UI_Common_AlertView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Common_AlertView : GameView
    {
		public const string VIEW_NAME = "UI_Common_Alert";

        public UI_Common_AlertView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_text_LanguageText;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_Blue_sure_SubView m_UI_Model_StandardButton_Blue_sure;
		[HideInInspector] public UI_Model_StandardButton_Red_SubView m_UI_Model_StandardButton_Red;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_lbl_text_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/lbl_text");

			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/lbl_title");

			m_UI_Model_StandardButton_Blue_sure = new UI_Model_StandardButton_Blue_sure_SubView(FindUI<RectTransform>(vb.transform ,"img_bg/UI_Model_StandardButton_Blue_sure"));
			m_UI_Model_StandardButton_Red = new UI_Model_StandardButton_Red_SubView(FindUI<RectTransform>(vb.transform ,"img_bg/UI_Model_StandardButton_Red"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}