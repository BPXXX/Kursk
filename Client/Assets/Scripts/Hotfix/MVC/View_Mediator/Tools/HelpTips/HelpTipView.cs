// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月5日
// Update Time         :    2020年2月5日
// Class Description   :    HelpTipView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class HelpTipView : GameView
    {
        public const string VIEW_NAME = "UI_Pop_TextTip";

        public HelpTipView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_pos;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_text_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_text_ContentSizeFitter;

		[HideInInspector] public RectTransform m_pl_bgSize;
		[HideInInspector] public UI_Tag_PopAnime_SkillTip_SubView m_UI_Tag_PopAnime_SkillTip;


        private void UIFinder()
        {
			m_pl_pos = FindUI<RectTransform>(vb.transform ,"pl_pos");
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideL");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideButtom");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideTop");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideR");

			m_lbl_text_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_pos/lbl_text");
			m_lbl_text_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_pos/lbl_text");

			m_pl_bgSize = FindUI<RectTransform>(vb.transform ,"pl_pos/lbl_text/pl_bgSize");
			m_UI_Tag_PopAnime_SkillTip = new UI_Tag_PopAnime_SkillTip_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/UI_Tag_PopAnime_SkillTip"));

            HelpTipMediator mt = new HelpTipMediator(vb.gameObject);
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
