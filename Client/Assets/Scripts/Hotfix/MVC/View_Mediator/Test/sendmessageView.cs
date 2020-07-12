// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月15日
// Update Time         :    2020年5月15日
// Class Description   :    sendmessageView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class sendmessageView : GameView
    {
        public const string VIEW_NAME = "sendmessage";

        public sendmessageView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_polygonImage_PolygonImage;

		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_send_enterroom_button;
		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_send_changestate_button;
		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_send_expel_button;


        private void UIFinder()
        {
			m_img_polygonImage_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_polygonImage");

			m_send_enterroom_button = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"send_enterroom_button"));
			m_send_changestate_button = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"send_changestate_button"));
			m_send_expel_button = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"send_expel_button"));

            sendmessageMediator mt = new sendmessageMediator(vb.gameObject);
            mt.view = this;
            AppFacade.GetInstance().RegisterMediator(mt);
			if(mt.IsOpenUpdate)
			{
                vb.fixedUpdateCallback = mt.FixedUpdate;
                vb.lateUpdateCallback = mt.LateUpdate;
				vb.updateCallback = mt.Update;
			}
            vb.openAniEndCallback = mt.OpenAniEnd;
            vb.onWinFocusCallback = mt.WinFocus;
            vb.onWinCloseCallback = mt.WinClose;
            vb.onPrewarmCallback = mt.PrewarmComplete;
        }

        #endregion

        public override void Start () {
            UIFinder();
    	}
        public override void OnDestroy()
        {
            AppFacade.GetInstance().RemoveView(vb);
        }

    }
}
