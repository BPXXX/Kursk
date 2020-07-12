// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月28日
// Update Time         :    2020年5月28日
// Class Description   :    UI_GamePrepareInterface_adminView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_GamePrepareInterface_adminView : GameView
    {
        public const string VIEW_NAME = "UI_GamePrepareInterface_admin";

        public UI_GamePrepareInterface_adminView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bg_bottom_PolygonImage;

		[HideInInspector] public UI_TankChooseBtn_One_SubView m_UI_TankChooseBtn_One;
		[HideInInspector] public UI_TankChooseBtn_Two_SubView m_UI_TankChooseBtn_Two;
		[HideInInspector] public UI_StatWarBtn_SubView m_UI_StatWarBtn;
		[HideInInspector] public UI_Position_Blue_SubView m_UI_Position_Blue1;
		[HideInInspector] public UI_Position_Blue_SubView m_UI_Position_Blue2;
		[HideInInspector] public UI_Position_Blue_SubView m_UI_Position_Blue3;
		[HideInInspector] public UI_Position_Blue_SubView m_UI_Position_Blue4;
		[HideInInspector] public UI_Position_Blue_SubView m_UI_Position_Blue5;
		[HideInInspector] public UI_Position_Blue_SubView m_UI_Position_Blue6;
		[HideInInspector] public UI_Position_Red_SubView m_UI_Position_Red1;
		[HideInInspector] public UI_Position_Red_SubView m_UI_Position_Red2;
		[HideInInspector] public UI_Position_Red_SubView m_UI_Position_Red3;
		[HideInInspector] public UI_Position_Red_SubView m_UI_Position_Red4;
		[HideInInspector] public UI_Position_Red_SubView m_UI_Position_Red5;
		[HideInInspector] public UI_Position_Red_SubView m_UI_Position_Red6;
		[HideInInspector] public UI_ChangePosition_SubView m_UI_ChangePosition1;
		[HideInInspector] public UI_ChangePosition_SubView m_UI_ChangePosition2;
		[HideInInspector] public UI_ChangePosition_SubView m_UI_ChangePosition3;
		[HideInInspector] public UI_ChangePosition_SubView m_UI_ChangePosition4;
		[HideInInspector] public UI_ChangePosition_SubView m_UI_ChangePosition5;
		[HideInInspector] public UI_ChangePosition_SubView m_UI_ChangePosition6;
		[HideInInspector] public UI_ChangePosition_SubView m_UI_ChangePosition7;
		[HideInInspector] public UI_ChangePosition_SubView m_UI_ChangePosition8;
		[HideInInspector] public UI_ChangePosition_SubView m_UI_ChangePosition9;
		[HideInInspector] public UI_ChangePosition_SubView m_UI_ChangePosition10;
		[HideInInspector] public UI_ChangePosition_SubView m_UI_ChangePosition11;
		[HideInInspector] public UI_ChangePosition_SubView m_UI_ChangePosition12;


        private void UIFinder()
        {
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_img_bg_bottom_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg_bottom");

			m_UI_TankChooseBtn_One = new UI_TankChooseBtn_One_SubView(FindUI<RectTransform>(vb.transform ,"img_bg_bottom/UI_TankChooseBtn_One"));
			m_UI_TankChooseBtn_Two = new UI_TankChooseBtn_Two_SubView(FindUI<RectTransform>(vb.transform ,"img_bg_bottom/UI_TankChooseBtn_Two"));
			m_UI_StatWarBtn = new UI_StatWarBtn_SubView(FindUI<RectTransform>(vb.transform ,"UI_StatWarBtn"));
			m_UI_Position_Blue1 = new UI_Position_Blue_SubView(FindUI<RectTransform>(vb.transform ,"UI_Position_Blue1"));
			m_UI_Position_Blue2 = new UI_Position_Blue_SubView(FindUI<RectTransform>(vb.transform ,"UI_Position_Blue2"));
			m_UI_Position_Blue3 = new UI_Position_Blue_SubView(FindUI<RectTransform>(vb.transform ,"UI_Position_Blue3"));
			m_UI_Position_Blue4 = new UI_Position_Blue_SubView(FindUI<RectTransform>(vb.transform ,"UI_Position_Blue4"));
			m_UI_Position_Blue5 = new UI_Position_Blue_SubView(FindUI<RectTransform>(vb.transform ,"UI_Position_Blue5"));
			m_UI_Position_Blue6 = new UI_Position_Blue_SubView(FindUI<RectTransform>(vb.transform ,"UI_Position_Blue6"));
			m_UI_Position_Red1 = new UI_Position_Red_SubView(FindUI<RectTransform>(vb.transform ,"UI_Position_Red1"));
			m_UI_Position_Red2 = new UI_Position_Red_SubView(FindUI<RectTransform>(vb.transform ,"UI_Position_Red2"));
			m_UI_Position_Red3 = new UI_Position_Red_SubView(FindUI<RectTransform>(vb.transform ,"UI_Position_Red3"));
			m_UI_Position_Red4 = new UI_Position_Red_SubView(FindUI<RectTransform>(vb.transform ,"UI_Position_Red4"));
			m_UI_Position_Red5 = new UI_Position_Red_SubView(FindUI<RectTransform>(vb.transform ,"UI_Position_Red5"));
			m_UI_Position_Red6 = new UI_Position_Red_SubView(FindUI<RectTransform>(vb.transform ,"UI_Position_Red6"));
			m_UI_ChangePosition1 = new UI_ChangePosition_SubView(FindUI<RectTransform>(vb.transform ,"UI_ChangePosition1"));
			m_UI_ChangePosition2 = new UI_ChangePosition_SubView(FindUI<RectTransform>(vb.transform ,"UI_ChangePosition2"));
			m_UI_ChangePosition3 = new UI_ChangePosition_SubView(FindUI<RectTransform>(vb.transform ,"UI_ChangePosition3"));
			m_UI_ChangePosition4 = new UI_ChangePosition_SubView(FindUI<RectTransform>(vb.transform ,"UI_ChangePosition4"));
			m_UI_ChangePosition5 = new UI_ChangePosition_SubView(FindUI<RectTransform>(vb.transform ,"UI_ChangePosition5"));
			m_UI_ChangePosition6 = new UI_ChangePosition_SubView(FindUI<RectTransform>(vb.transform ,"UI_ChangePosition6"));
			m_UI_ChangePosition7 = new UI_ChangePosition_SubView(FindUI<RectTransform>(vb.transform ,"UI_ChangePosition7"));
			m_UI_ChangePosition8 = new UI_ChangePosition_SubView(FindUI<RectTransform>(vb.transform ,"UI_ChangePosition8"));
			m_UI_ChangePosition9 = new UI_ChangePosition_SubView(FindUI<RectTransform>(vb.transform ,"UI_ChangePosition9"));
			m_UI_ChangePosition10 = new UI_ChangePosition_SubView(FindUI<RectTransform>(vb.transform ,"UI_ChangePosition10"));
			m_UI_ChangePosition11 = new UI_ChangePosition_SubView(FindUI<RectTransform>(vb.transform ,"UI_ChangePosition11"));
			m_UI_ChangePosition12 = new UI_ChangePosition_SubView(FindUI<RectTransform>(vb.transform ,"UI_ChangePosition12"));

            UI_GamePrepareInterface_adminMediator mt = new UI_GamePrepareInterface_adminMediator(vb.gameObject);
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
