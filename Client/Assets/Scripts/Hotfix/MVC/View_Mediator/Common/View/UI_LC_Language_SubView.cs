// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_LC_Language_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_LC_Language_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_LC_Language";

        public UI_LC_Language_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public HorizontalLayoutGroup m_UI_LC_Language_HorizontalLayoutGroup;

		[HideInInspector] public UI_Item_LanguageSelect_SubView m_UI_Item_LanguageSelect1;
		[HideInInspector] public UI_Item_LanguageSelect_SubView m_UI_Item_LanguageSelect2;


        private void UIFinder()
        {       
			m_UI_LC_Language_HorizontalLayoutGroup = gameObject.GetComponent<HorizontalLayoutGroup>();

			m_UI_Item_LanguageSelect1 = new UI_Item_LanguageSelect_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_LanguageSelect1"));
			m_UI_Item_LanguageSelect2 = new UI_Item_LanguageSelect_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_LanguageSelect2"));

			BindEvent();
        }

        #endregion
    }
}