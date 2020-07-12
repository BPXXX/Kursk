//
// Author:  Johance
//
using Client;
using GameFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[ExecuteInEditMode]
public class ArabLayoutCompment : MonoBehaviour{
    public enum ArabLayoutStyle
    {
        ArabLayoutStyle_None = 0,

        [ArabLayoutStyleEditor(typeof(CanvasScaler), "开启预览(只有CanvasScaler层的才可以修改)")]
        ArabLayoutStyle_Preview = 0x01,
        [ArabLayoutStyleEditor(typeof(RectTransform), "位置左右对调")]
        ArabLayoutStyle_PosX = 0x02,
        //[ArabLayoutStyleEditor(typeof(RectTransform), "锚点左右对调")]
        //ArabLayoutStyle_AnchorX = 0x04,
        [ArabLayoutStyleEditor(typeof(Text), "文本左右对齐")]
        ArabLayoutStyle_TextAlignX = 0x08,
        [ArabLayoutStyleEditor("图片左右翻转", typeof(Image))]
        ArabLayoutStyle_ImageFlipX = 0x10,
        [ArabLayoutStyleEditor(typeof(Slider), "进度条左右翻转")]
        ArabLayoutStyle_LoadingBarFlipX = 0x20,
        [ArabLayoutStyleEditor("左右镜像", typeof(RectTransform))]
        ArabLayoutStyle_NodeFlipX = 0x40,
        //[ArabLayoutStyleEditor(typeof(RectTransform), "节点旋转翻转")]
        //ArabLayoutStyle_RotateFlip = 0x80, //
        [ArabLayoutStyleEditor(typeof(ListView), "滑动列表左右翻转")]
        ArabLayoutStyle_ListViewFlipX = 0x100,
        [ArabLayoutStyleEditor(typeof(GridLayoutGroup), "起始位置翻转")]
        ArabLayoutStyle_GridLayoutGroupFlipX = 0x200,
    }
    private RectTransform gui;

    [HideInInspector]
    [SerializeField]
    protected int _arab_Layout = 0;
    public int ArabLayout
    {
        get
        {
            return _arab_Layout;
        }
        set
        {
            int oldValue = _arab_Layout;
            _arab_Layout = value;
            OnArabLayoutPropertyChanged(oldValue, value);
        }
    }
    public void SetStyle(ArabLayoutStyle style, bool bEnable)
    {
        // 属性如果一样就不刷新UI
        if (GetStyle(style) == bEnable)
            return;

        bool bUpdateLayout = GetStyle(ArabLayoutStyle.ArabLayoutStyle_Preview);
        if(style == ArabLayoutStyle.ArabLayoutStyle_Preview)
        {
            bUpdateLayout = bUpdateLayout != bEnable;
        }
        if (bUpdateLayout)
        {
            UpdateArabLayout(style);
        }
        if (bEnable)
            ArabLayout = ArabLayout | (int)style;
        else
            ArabLayout = ArabLayout & ~((int)style);
    }

    public bool GetStyle(ArabLayoutStyle style)
    {
        return (ArabLayout & (int)style) != 0;
    }
    
    internal virtual void OnArabLayoutPropertyChanged(int oldValue, int newValue)
    {
    }

    protected virtual bool UpdateArabLayout(ArabLayoutStyle style)
    {
        if (gui == null)
        {
            gui = GetComponent<RectTransform>();
        }
        if (gui == null)
        {
            return false;
        }
        switch (style)
        {
            case ArabLayoutStyle.ArabLayoutStyle_Preview:
                {
                    foreach (ArabLayoutStyle item in Enum.GetValues(typeof(ArabLayoutStyle)))
                    {
                        if((int)item > 1 && GetStyle(item))
                        {
                            UpdateArabLayout(item);
                        }
                    }
                }
                return false;
            case ArabLayoutStyle.ArabLayoutStyle_PosX:
                {
                    //Vector2 anchorMax = gui.anchorMax;
                    //Vector2 anchorMin = gui.anchorMin;
                    //anchorMin.x = 1 - gui.anchorMax.x;
                    //anchorMax.x = 1 - gui.anchorMin.x;
                    //gui.anchorMax = anchorMax;
                    //gui.anchorMin = anchorMin;

                    //Vector2 pivot = gui.pivot;
                    //pivot.x = 1 - pivot.x;
                    //gui.pivot = pivot;
                    //Vector2 pos = gui.anchoredPosition;
                    //pos.x = -pos.x;
                    //gui.anchoredPosition = pos;
                    if (transform.name.Contains("lbl_property_residue"))
                    {
                        Debug.Log("lbl_property_residue");
                    }
                    gui.anchoredPosition = CalculateArabLayoutStyle_PosX(gui);
                }
                return true;
            case ArabLayoutStyle.ArabLayoutStyle_TextAlignX:
                {
                    var compment = GetComponent<Text>();
                    if (compment == null)
                        return false;
                    switch (compment.alignment)
                    {
                        case TextAnchor.LowerLeft:
                            {
                                compment.alignment = TextAnchor.LowerRight;
                            }
                            break;
                        case TextAnchor.MiddleLeft:
                            {
                                compment.alignment = TextAnchor.MiddleRight;
                            }
                            break;
                        case TextAnchor.UpperLeft:
                            {
                                compment.alignment = TextAnchor.UpperRight;
                            }
                            break;
                        case TextAnchor.LowerRight:
                            {
                                compment.alignment = TextAnchor.LowerLeft;
                            }
                            break;
                        case TextAnchor.MiddleRight:
                            {
                                compment.alignment = TextAnchor.MiddleLeft;
                            }
                            break;
                        case TextAnchor.UpperRight:
                            {
                                compment.alignment = TextAnchor.UpperLeft;
                            }
                            break;
                    }
                }
                return true;
            case ArabLayoutStyle.ArabLayoutStyle_ImageFlipX:
                {
                    var compment = GetComponent<Image>();
                    if (compment == null)
                        return false;

                    var scale = compment.transform.localScale;
                    scale.x = -scale.x;
                    compment.transform.localScale = scale;
                }
                return true;
            case ArabLayoutStyle.ArabLayoutStyle_LoadingBarFlipX:
                {
                    var compment = GetComponent<Slider>();
                    if (compment == null)
                        return false;
                    switch (compment.direction)
                    {
                        case Slider.Direction.LeftToRight:
                            {
                                compment.direction = Slider.Direction.RightToLeft;
                            }
                            break;
                        case Slider.Direction.RightToLeft:
                            {
                                compment.direction = Slider.Direction.LeftToRight;
                            }
                            break;
                    }
                }
                return true;
            case ArabLayoutStyle.ArabLayoutStyle_NodeFlipX:
                {
                    var compment = GetComponent<RectTransform>();
                    if (compment == null)
                        return false;

                    var scale = compment.transform.localScale;
                    scale.x = -scale.x;
                    compment.transform.localScale = scale;
                }
                return true;
            case ArabLayoutStyle.ArabLayoutStyle_ListViewFlipX:
                {
                    var compment = GetComponent<ListView>();
                    if (compment == null)
                        return false;
                    switch (compment.layoutType)
                    {
                        case ListView.ListViewLayoutType.LeftToRight:
                            {
                                compment.layoutType = ListView.ListViewLayoutType.RightToLeft;
                            }
                            break;
                        case ListView.ListViewLayoutType.RightToLeft:
                            {
                                compment.layoutType = ListView.ListViewLayoutType.LeftToRight;
                            }
                            break;
                    }
                    compment.ResetPosition();
                }
                return true;
            case ArabLayoutStyle.ArabLayoutStyle_GridLayoutGroupFlipX:
                {
                    var compment = GetComponent<GridLayoutGroup>();
                    if (compment == null)
                        return false;
                    switch (compment.startCorner)
                    {
                        case GridLayoutGroup.Corner.LowerLeft:
                            {
                                compment.startCorner = GridLayoutGroup.Corner.LowerRight;
                            }
                            break;
                        case GridLayoutGroup.Corner.LowerRight:
                            {
                                compment.startCorner = GridLayoutGroup.Corner.LowerLeft;
                            }
                            break;
                        case GridLayoutGroup.Corner.UpperLeft:
                            {
                                compment.startCorner = GridLayoutGroup.Corner.UpperRight;
                            }
                            break;
                        case GridLayoutGroup.Corner.UpperRight:
                            {
                                compment.startCorner = GridLayoutGroup.Corner.UpperLeft;
                            }
                            break;
                    }
                    switch (compment.childAlignment)
                    {
                        case TextAnchor.LowerLeft:
                            {
                                compment.childAlignment = TextAnchor.LowerRight;
                            }
                            break;
                        case TextAnchor.LowerRight:
                            {
                                compment.childAlignment = TextAnchor.LowerLeft;
                            }
                            break;
                        case TextAnchor.MiddleLeft:
                            {
                                compment.childAlignment = TextAnchor.MiddleRight;
                            }
                            break;
                        case TextAnchor.MiddleRight:
                            {
                                compment.childAlignment = TextAnchor.MiddleLeft;
                            }
                            break;
                        case TextAnchor.UpperLeft:
                            {
                                compment.childAlignment = TextAnchor.UpperRight;
                            }
                            break;
                        case TextAnchor.UpperRight:
                            {
                                compment.childAlignment = TextAnchor.UpperLeft;
                            }
                            break;
                    }
                }
                return true;
        }
        return false;
    }

    public Vector3 CalculateArabLayoutStyle_PosX(RectTransform gui)
    {
        Vector2 anchorMax = gui.anchorMax;
        Vector2 anchorMin = gui.anchorMin;
        anchorMin.x = 1 - gui.anchorMax.x;
        anchorMax.x = 1 - gui.anchorMin.x;
        gui.anchorMax = anchorMax;
        gui.anchorMin = anchorMin;

        Vector2 pivot = gui.pivot;
        pivot.x = 1 - pivot.x;
        gui.pivot = pivot;
        Vector2 pos = gui.anchoredPosition;
        pos.x = -pos.x;
        return pos;
    }

    // Use this for initialization
    void Awake () {
        gui = GetComponent<RectTransform>();
        if (Application.isPlaying)
        {
            bool isArabUI = GetStyle(ArabLayoutStyle.ArabLayoutStyle_Preview);
            if (isArabUI == Skyunion.LanguageUtils.IsArabic())
            {
                SetStyle(ArabLayoutStyle.ArabLayoutStyle_Preview, !isArabUI);
            }
        }
    }

#if UNITY_EDITOR
    private CanvasScaler m_RootCanvasScaler = null;
    // Update is called once per frame
    void Update () {
        if (Application.isPlaying)
        {
            return ;
        }
        if(m_RootCanvasScaler == null && gui != null)
        {
            m_RootCanvasScaler = gui.GetComponentInParent<CanvasScaler>();
            if(m_RootCanvasScaler)
            {
                var arabCom = m_RootCanvasScaler.GetComponent<ArabLayoutCompment>();
                bool bRootIsArabic = false;
                if (arabCom)
                {
                    bRootIsArabic = arabCom.GetStyle(ArabLayoutStyle.ArabLayoutStyle_Preview);
                }
                bool isArabUI = GetStyle(ArabLayoutStyle.ArabLayoutStyle_Preview);
                if (isArabUI != bRootIsArabic)
                {
                    SetStyle(ArabLayoutStyle.ArabLayoutStyle_Preview, !isArabUI);
                    Debug.Log("ArabLayout Compment Start 1");
                }
            }
        }
    }
#endif
}
