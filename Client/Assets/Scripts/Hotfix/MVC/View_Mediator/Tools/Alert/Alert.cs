using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using Skyunion;
using UnityEngine.Events;
using DG.Tweening;
using System;
using UnityEngine.UI;
using System.Text;

namespace Game
{
    public class AlertButtonData
    {
        public string m_ButtonText;
        public int m_ButtonTimer = -1;
        public UnityAction m_ButtonEvent;
        public Text m_TextCom;
        public bool m_DestroySelf;
    }

    public class Alert
    {
        public string m_text;
        public string m_title;

        public GameObject m_gameObject;

        public AlertButtonData m_left;
        public AlertButtonData m_right;

        public int m_currentTimer = 0;

        public int m_sortingOrder = 0;

        public static Alert CreateAlert(string text,string title = null)
        {
            Alert alert = new Alert();
            alert.m_text = text;
            alert.m_title = title;
            return alert;
        }

        public static Alert CreateAlert(int languageID, string title = null)
        {
            Alert alert = new Alert();
            alert.m_text = LanguageUtils.getText(languageID);
            alert.m_title = title;
            return alert;
        }

        public Alert SetLeftButton(UnityAction buttonEvent = null, string buttonText = null, bool destroySelf = true,int countdown = -1)
        {
            if(m_left==null)
            {
                m_left = new AlertButtonData();
            }
            m_left.m_ButtonEvent = buttonEvent;
            m_left.m_ButtonText = buttonText;
            m_left.m_ButtonTimer = countdown;
            m_left.m_DestroySelf = destroySelf;
            return this;
        }

        public Alert SetRightButton(UnityAction buttonEvent = null, string buttonText = null, bool destroySelf = true,int countdown = -1)
        {
            if (m_right == null)
            {
                m_right = new AlertButtonData();
            }
            m_right.m_ButtonEvent = buttonEvent;
            m_right.m_ButtonText = buttonText;
            m_right.m_ButtonTimer = countdown;
            m_right.m_DestroySelf = destroySelf;
            return this;
        }

        public Alert SetSortingOrder(int sortingOrder)
        {
            this.m_sortingOrder = sortingOrder;
            return this;
        }

        public void DestroySelf()
        {
            if(m_gameObject!=null)
            {
                GameObject.DestroyImmediate(m_gameObject);
            }
        }

        public void CompleteTimer()
        {
            if((m_left==null||m_left.m_ButtonTimer==-1)&&(m_right==null||m_right.m_ButtonTimer==-1))
            {
                return;
            }
            m_currentTimer++;
            CountDownLeft();
            CountDownRight();
            m_gameObject.transform.DOScale(Vector3.one,1).OnComplete(CompleteTimer);
        }

        public void CountDownLeft()
        {
            if(m_left!=null&&m_left.m_ButtonTimer!=-1)
            {
                int countDown = m_left.m_ButtonTimer - m_currentTimer;
                if (countDown!=0)
                {
                    string leftText = string.Format($"{(m_left.m_ButtonText != null ? m_left.m_ButtonText : Alert.DefaultLeftText())}({countDown})");
                    m_left.m_TextCom.text = leftText;
                }
                else if (m_gameObject != null)
                {
                    m_left.m_ButtonEvent();
                }
            }
        }

        public void CountDownRight()
        {        
            if (m_right != null && m_right.m_ButtonTimer != -1)
            {
                int countDown = m_right.m_ButtonTimer - m_currentTimer;
                if (countDown != 0)
                {
                    string RightText = string.Format($"{(m_right.m_ButtonText != null ? m_right.m_ButtonText : Alert.DefaultRightText())}({countDown})");
                    m_right.m_TextCom.text = RightText;
                }
                else if(m_gameObject!=null)
                {
                    m_right.m_ButtonEvent();
                }
            }
        }

        public static string DefaultTitle()
        {
            return LanguageUtils.getText(100035);
        }

        public static string DefaultText()
        {
            return "错误：没有内容的Alert";
        }

        public static string DefaultRightText()
        {
            return LanguageUtils.getText(300012);
        }

        public static string DefaultLeftText()
        {
            return LanguageUtils.getText(300011);
        }


        public Alert Show()
        {
            AlertManager.Instance.ShowAlert(this);
            return this;
        }

    }

    public class AlertManager: Hotfix.TSingleton<AlertManager>
    {
        private Dictionary<string, GameObject> m_assets = new Dictionary<string, GameObject>();
        private Transform tipLayer;
        
        public void ShowAlert(Alert alert)
        {
            if(alert.m_right!=null && alert.m_left != null)
            {
                InitAssets(RS.Alert,alert, CreateAlert);
            }
            else
            {
                InitAssets(RS.AlertMin, alert, CreateAlertMin);
            }
        }

        private void CreateAlert(Alert alert)
        {
            GameObject go = CoreUtils.assetService.Instantiate(m_assets[RS.Alert]);
            go.transform.SetParent(tipLayer);
            go.transform.localScale = Vector3.one;
            RectTransform rectTran = go.GetComponent<RectTransform>();
            rectTran.anchorMin = Vector2.zero;
            rectTran.anchorMax = Vector2.one;
            rectTran.sizeDelta = Vector2.zero;
            alert.m_gameObject = go;
            if(alert.m_sortingOrder!=0)
            {
                alert.m_gameObject.AddComponent<Canvas>().sortingOrder = alert.m_sortingOrder;
            }
            UI_Common_AlertView alertView = MonoHelper.AddHotFixViewComponent<UI_Common_AlertView>(go);
            alertView.m_lbl_title_LanguageText.text = alert.m_title != null ? alert.m_title : Alert.DefaultTitle();
            alertView.m_lbl_text_LanguageText.text = alert.m_text != null ? alert.m_text : Alert.DefaultText();

            alertView.m_UI_Model_StandardButton_Blue_sure.m_lbl_Text_LanguageText.text = alert.m_right.m_ButtonText != null ? alert.m_right.m_ButtonText : Alert.DefaultRightText();
            if(alert.m_right.m_ButtonEvent != null)
            {
                alertView.m_UI_Model_StandardButton_Blue_sure.m_btn_languageButton_GameButton.onClick.AddListener(alert.m_right.m_ButtonEvent);
            }
            if (alert.m_right.m_DestroySelf)
            {
                alertView.m_UI_Model_StandardButton_Blue_sure.m_btn_languageButton_GameButton.onClick.AddListener(alert.DestroySelf);
            }
            alert.m_left.m_TextCom = alertView.m_UI_Model_StandardButton_Blue_sure.m_lbl_Text_LanguageText;

            alertView.m_UI_Model_StandardButton_Red.m_lbl_Text_LanguageText.text = alert.m_left.m_ButtonText != null ? alert.m_left.m_ButtonText : Alert.DefaultLeftText();
            if (alert.m_left.m_ButtonEvent != null)
            {
                alertView.m_UI_Model_StandardButton_Red.m_btn_languageButton_GameButton.onClick.AddListener(alert.m_left.m_ButtonEvent);
            }
            if (alert.m_left.m_DestroySelf)
            {
                alertView.m_UI_Model_StandardButton_Red.m_btn_languageButton_GameButton.onClick.AddListener(alert.DestroySelf);
            }
            alert.m_right.m_TextCom = alertView.m_UI_Model_StandardButton_Red.m_lbl_Text_LanguageText;

            alert.CompleteTimer();
        }

        private void CreateAlertMin(Alert alert)
        {
            GameObject go = CoreUtils.assetService.Instantiate(m_assets[RS.AlertMin]);
            go.transform.SetParent(tipLayer);
            go.transform.localScale = Vector3.one;
            RectTransform rectTran = go.GetComponent<RectTransform>();
            rectTran.anchorMin = Vector2.zero;
            rectTran.anchorMax = Vector2.one;
            rectTran.sizeDelta = Vector2.zero;
            alert.m_gameObject = go;
            if (alert.m_sortingOrder != 0)
            {
                alert.m_gameObject.AddComponent<Canvas>().sortingOrder = alert.m_sortingOrder;
            }
            UI_Common_AlertMinView alertView = MonoHelper.AddHotFixViewComponent<UI_Common_AlertMinView>(go);
            alertView.m_lbl_title_LanguageText.text = alert.m_title != null ? alert.m_title : Alert.DefaultTitle();
            alertView.m_lbl_text_LanguageText.text = alert.m_text != null ? alert.m_text : Alert.DefaultText();
            AlertButtonData data = alert.m_left != null ? alert.m_left : alert.m_right;
            alertView.m_UI_Model_StandardButton_Blue_sure.m_lbl_Text_LanguageText.text = data.m_ButtonText != null ? data.m_ButtonText : Alert.DefaultRightText();
            alertView.m_UI_Model_StandardButton_Blue_sure.m_btn_languageButton_GameButton.onClick.AddListener(data.m_ButtonEvent != null ? data.m_ButtonEvent : alert.DestroySelf);
            data.m_TextCom = alertView.m_UI_Model_StandardButton_Blue_sure.m_lbl_Text_LanguageText;
            alert.CompleteTimer();
        }

        private void InitAssets(string assetName,Alert alert, Action<Alert> callBack)
        {
            if (tipLayer == null)
            {
                tipLayer = CoreUtils.uiManager.GetUILayer((int)UILayer.TipLayer).Find("pl_dialog");
            }
            if(!m_assets.ContainsKey(assetName))
            {
                CoreUtils.assetService.LoadAssetAsync<GameObject>(assetName, (asset) =>
                {
                    if (asset.asset() == null)
                    {
                        Debug.LogErrorFormat("load prefab fail:{0}", asset.assetName());
                    }
                    m_assets[assetName] = asset.asset() as GameObject;
                    callBack(alert);
                });
            }
            else
            {
                callBack(alert);
            }
        }
    }
}

