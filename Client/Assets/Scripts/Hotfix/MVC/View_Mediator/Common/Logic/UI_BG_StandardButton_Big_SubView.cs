// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月17日
// Update Time         :    2020年5月17日
// Class Description   :    UI_BG_StandardButton_Big_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_BG_StandardButton_Big_SubView : UI_SubView
    {
        public void AddClickEvent(UnityAction call)
        {
            m_btn_bgButton_GameButton.onClick.AddListener(call);
        }
    }
}