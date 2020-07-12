// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_LanguageSelect_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_LanguageSelect_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_LanguageSelect";

        public UI_Item_LanguageSelect_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_LanguageSelect;
		[HideInInspector] public GameToggle m_ck_language_GameToggle;

		[HideInInspector] public LanguageText m_lbl_Language_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_LanguageSelect = gameObject.GetComponent<RectTransform>();
			m_ck_language_GameToggle = FindUI<GameToggle>(gameObject.transform ,"ck_language");

			m_lbl_Language_LanguageText = FindUI<LanguageText>(gameObject.transform ,"ck_language/lbl_Language");


			BindEvent();
        }

        #endregion
    }
}