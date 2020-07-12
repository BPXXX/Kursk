// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月7日
// Update Time         :    2020年2月7日
// Class Description   :    UI_LC_Language_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;

namespace Game {
    public partial class UI_LC_Language_SubView : UI_SubView
    {
        public void BindGroup(ToggleGroup toggleGroup)
        {
            m_UI_Item_LanguageSelect1.BindGroup(toggleGroup);
            m_UI_Item_LanguageSelect2.BindGroup(toggleGroup);
        }
        public void AddValueChange(Action<int> action)
        {
            m_UI_Item_LanguageSelect1.AddValueChange(action);
            m_UI_Item_LanguageSelect2.AddValueChange(action);
        }
        public void SetLanguageId(int left, int right)
        {
            m_UI_Item_LanguageSelect1.SetLanguage(left);
            m_UI_Item_LanguageSelect2.SetLanguage(right);
        }
        public void Selected(int id)
        {
            m_UI_Item_LanguageSelect1.Selected(id);
            m_UI_Item_LanguageSelect2.Selected(id);
        }
    }
}