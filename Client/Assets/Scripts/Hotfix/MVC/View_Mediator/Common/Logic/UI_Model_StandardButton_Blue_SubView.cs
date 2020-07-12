// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月30日
// Update Time         :    2019年12月30日
// Class Description   :    UI_Model_StandardButton_Blue_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;
using System.IO;
using ProtoBuf;

namespace Game {
    public partial class UI_Model_StandardButton_Blue_SubView : UI_SubView
    {
       
        public void Enable(bool enabled)
        {
            m_btn_languageButton_GameButton.enabled = enabled;
        }
        public void AddClickEvent(UnityAction call)
        {
            m_btn_languageButton_GameButton.onClick.AddListener(call);
        }
    }
}