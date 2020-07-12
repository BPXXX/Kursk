// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月28日
// Update Time         :    2020年5月28日
// Class Description   :    UI_Win_LanguagenewView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_LanguagenewView : GameView
    {
        public const string VIEW_NAME = "UI_Win_Languagenew";

        public UI_Win_LanguagenewView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public UI_Model_Window_TypeMid_SubView m_UI_Model_Window_Type2;
		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;

		[HideInInspector] public UI_Model_StandardButton_Blue_big_SubView m_UI_Model_StandardButton_Blue_sure;


        private void UIFinder()
        {
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_UI_Model_Window_Type2 = new UI_Model_Window_TypeMid_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type2"));
			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/content/sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/content/sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(vb.transform ,"rect/content/sv_list_view");

			m_UI_Model_StandardButton_Blue_sure = new UI_Model_StandardButton_Blue_big_SubView(FindUI<RectTransform>(vb.transform ,"rect/content/UI_Model_StandardButton_Blue_sure"));

            UI_Win_LanguagenewMediator mt = new UI_Win_LanguagenewMediator(vb.gameObject);
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
