// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月7日
// Update Time         :    2020年2月7日
// Class Description   :    UI_Item_LanguageSelect_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;

namespace Game {
    public partial class UI_Item_LanguageSelect_SubView : UI_SubView
    {
        private int m_id;
        public void BindGroup(ToggleGroup toggleGroup)
        {
            m_ck_language_GameToggle.isOn = false;
            m_ck_language_GameToggle.group = toggleGroup;
        }
        public void AddValueChange(Action<int> action)
        {
            m_ck_language_GameToggle.onValueChanged.AddListener((isOn)=>
            {
                if(isOn)
                {
                    action(m_id);
                }
            });
        }
        public void SetLanguage(int id)
        {
            this.m_id = id;
            if(m_id == -1)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
                var tid = CoreUtils.dataService.QueryRecord<Data.LanguageSetDefine>(m_id).l_languageID;
                m_lbl_Language_LanguageText.text = LanguageUtils.getText(tid);
            }
        }
        public void Selected(int id)
        {
            m_ck_language_GameToggle.isOn = (this.m_id == id);
        }
    }
}