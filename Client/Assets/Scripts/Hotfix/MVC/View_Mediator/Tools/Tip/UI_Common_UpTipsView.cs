// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月3日
// Update Time         :    2020年1月3日
// Class Description   :    UI_Common_UpTipsView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Common_UpTipsView : GameView
    {
		public const string VIEW_NAME = "UI_Common_UpTips";

        public UI_Common_UpTipsView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public CanvasGroup m_img_bg_CanvasGroup;

		[HideInInspector] public LanguageText m_lbl_message_LanguageText;

		[HideInInspector] public UI_Tag_PopAnime_UpTips_SubView m_UI_Tag_PopAnime_UpTips;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");
			m_img_bg_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"img_bg");

			m_lbl_message_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/lbl_message");

			m_UI_Tag_PopAnime_UpTips = new UI_Tag_PopAnime_UpTips_SubView(FindUI<RectTransform>(vb.transform ,"img_bg/UI_Tag_PopAnime_UpTips"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}