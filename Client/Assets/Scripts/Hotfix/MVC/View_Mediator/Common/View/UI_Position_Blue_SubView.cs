// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Position_Blue_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Position_Blue_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Position_Blue";

        public UI_Position_Blue_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Position_Blue;
		[HideInInspector] public PolygonImage m_img_Head_PolygonImage;

		[HideInInspector] public PolygonImage m_img_username_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_usernameText_LanguageText;

		[HideInInspector] public PolygonImage m_img_tankname_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_tanknameText_LanguageText;



        private void UIFinder()
        {       
			m_UI_Position_Blue = gameObject.GetComponent<RectTransform>();
			m_img_Head_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_Head");

			m_img_username_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_username");

			m_lbl_usernameText_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_username/lbl_usernameText");

			m_img_tankname_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_tankname");

			m_lbl_tanknameText_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_tankname/lbl_tanknameText");


			BindEvent();
        }

        #endregion
    }
}