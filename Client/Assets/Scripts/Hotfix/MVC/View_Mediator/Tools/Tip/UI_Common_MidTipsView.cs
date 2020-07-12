// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月4日
// Update Time         :    2020年2月4日
// Class Description   :    UI_Common_MidTipsView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Common_MidTipsView : GameView
    {
		public const string VIEW_NAME = "UI_Common_MidTips";

        public UI_Common_MidTipsView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_message_LanguageText;
		[HideInInspector] public Outline m_lbl_message_Outline;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_lbl_message_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/lbl_message");
			m_lbl_message_Outline = FindUI<Outline>(vb.transform ,"img_bg/lbl_message");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}