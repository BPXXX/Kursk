// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月29日
// Update Time         :    2019年12月29日
// Class Description   :    UI_Model_StandardButton_Blue_sure_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Model_StandardButton_Blue_sure_SubView : UI_Model_StandardButton_Blue_SubView
    {
        public void AddClickEvent(UnityAction call)
        {
            m_btn_languageButton_GameButton.onClick.AddListener(call);
        }
    }
}