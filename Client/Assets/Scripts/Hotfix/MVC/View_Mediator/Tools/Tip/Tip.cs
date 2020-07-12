using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using Skyunion;
using System;


namespace Game
{
    public class Tip
    {
        public enum TipStyle
        {
            Top,
            Middle,
        }

        //单次播放持续时间
        public static float ShowTime = 2f;
        //连续播放的持续时间
        public static float QuickShowTime = 0.5f;

        public string text;
        public TipStyle style;
        public GameObject go;

        public static Tip CreateTip(string text, TipStyle style = TipStyle.Top)
        {
            Tip tip = new Tip();
            tip.text = text;
            tip.style = style;
            return tip;
        }

        public static Tip CreateTip(int languageID, TipStyle style = TipStyle.Top)
        {
            Tip tip = new Tip();
            tip.text = LanguageUtils.getText(languageID);
            tip.style = style;
            return tip;
        }

        public static Tip CreateTip(int languageID, params object[] args)
        {
            Tip tip = new Tip();
            tip.text = string.Format(LanguageUtils.getText(languageID),args);
            return tip;
        }

        public Tip SetStyle(TipStyle style)
        {
            this.style = style;
            return this;
        }

        public Tip Show()
        {
            TipManager.Instance.Enqueue(this);
            return this;
        }

    }

    public class TipManager: Hotfix.TSingleton<TipManager>
    {
        private  Dictionary<string,GameObject> m_tipAssets = new Dictionary<string, GameObject>();
        private  Queue<Tip> m_topTips = new Queue<Tip>();
        private  bool isDequeueTop = false;
        private Queue<Tip> m_midTips = new Queue<Tip>();
        private bool isDequeueMid =false;
        private  Transform tipLayer;
        public void Enqueue(Tip tip)
        {
            switch(tip.style)
            {
                case Tip.TipStyle.Top:
                    m_topTips.Enqueue(tip);
                    InitAsset(RS.Tip_Up, OnTopAssetLoad);
                    break;
                case Tip.TipStyle.Middle:
                    m_midTips.Enqueue(tip);
                    InitAsset(RS.Tip_Mid, OnMidAssetLoad);
                    break;
                default:break;
            }
        }

        private void OnTopAssetLoad()
        {
            if(!isDequeueTop)
            {
                TopDequeue();
            }
        }

        private void TopDequeue()
        {
            if (m_topTips.Count>0)
            {
                isDequeueTop = true;
                Tip tip =  m_topTips.Peek();
                GameObject go = CoreUtils.assetService.Instantiate(m_tipAssets[RS.Tip_Up]);
                go.transform.SetParent(tipLayer);
                go.transform.localScale = Vector3.one;
                tip.go = go;
                UI_Common_UpTipsView tipView = MonoHelper.AddHotFixViewComponent<UI_Common_UpTipsView>(go);
                tipView.m_lbl_message_LanguageText.text = tip.text;
                tipView.m_img_bg_PolygonImage.rectTransform.sizeDelta = new Vector2(tipView.m_lbl_message_LanguageText.preferredWidth + 50, tipView.m_img_bg_PolygonImage.rectTransform.sizeDelta.y);
                Timer.Register(Tip.QuickShowTime,OnTopComplete);
            }
        }

        private void OnTopComplete()
        {
            if(m_topTips.Count > 0)
            {
                Tip tip = m_topTips.Dequeue();
                //播放完毕自动销毁
                if(tip.go!=null)
                {
                    AutoPlayAndDestroyTip component = tip.go.GetComponent<AutoPlayAndDestroyTip>();
                    if(component!=null)
                    {
                        component.PlayEndAni();
                    }
                    else
                    {
                        GameObject.DestroyImmediate(tip.go);
                    }
                }
                isDequeueTop = false;
                TopDequeue();
            }
        }

        private void OnMidAssetLoad()
        {
            if (!isDequeueMid)
            {
                MidDequeue();
            }
        }

        private void MidDequeue()
        {
            if (m_midTips.Count > 0)
            {
                isDequeueMid = true;
                Tip tip = m_midTips.Peek();
                GameObject go = CoreUtils.assetService.Instantiate(m_tipAssets[RS.Tip_Mid]);
                go.transform.SetParent(tipLayer);
                go.transform.localScale = Vector3.one;
                tip.go = go;
                UI_Common_MidTipsView tipView = MonoHelper.AddHotFixViewComponent<UI_Common_MidTipsView>(go);
                tipView.m_lbl_message_LanguageText.text = tip.text;
                tipView.m_img_bg_PolygonImage.rectTransform.sizeDelta = new Vector2(tipView.m_lbl_message_LanguageText.preferredWidth + 50, tipView.m_img_bg_PolygonImage.rectTransform.sizeDelta.y);
                Timer.Register(Tip.QuickShowTime, OnMidComplete);
            }
        }

        private void OnMidComplete()
        {
            if (m_midTips.Count > 0)
            {
                Tip tip = m_midTips.Dequeue();
                //播放完毕自动销毁
                if (tip.go != null)
                {
                    AutoPlayAndDestroyTip component = tip.go.GetComponent<AutoPlayAndDestroyTip>();
                    if (component != null)
                    {
                        component.PlayEndAni();
                    }
                    else
                    {
                        GameObject.DestroyImmediate(tip.go);
                    }
                }
                isDequeueMid = false;
                MidDequeue();
            }
        }


        private void InitAsset(string assetName,Action callBack)
        {
            if(tipLayer==null)
            {
                tipLayer = CoreUtils.uiManager.GetUILayer((int)UILayer.TipLayer).Find("pl_tip");
            }
            if(!m_tipAssets.ContainsKey(assetName))
            {
                CoreUtils.assetService.LoadAssetAsync<GameObject>(assetName, (asset) =>
                {
                    m_tipAssets[assetName] = asset.asset() as GameObject;
                    callBack();
                });
            }
            else
            {
                callBack();
            }
        }
    }
}
