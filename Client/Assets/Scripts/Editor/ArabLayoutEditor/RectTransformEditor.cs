//
// Author:  Johance
//
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(RectTransform))]
[CanEditMultipleObjects]
public class ArabLayoutEditor : DecoratorEditor
{
    public ArabLayoutEditor() : base("RectTransformEditor") { }

    private bool m_bArabPreview = false;
    public void OnEnable()
    {
        var gui = target as RectTransform;
        CanvasScaler root = gui.GetComponentInParent<CanvasScaler>();
        if (root)
        {
            var rootLayout = root.GetComponent<ArabLayoutCompment>();
            if (rootLayout)
            {
                m_bArabPreview = rootLayout.GetStyle(ArabLayoutCompment.ArabLayoutStyle.ArabLayoutStyle_Preview);
            }
        }
    }

    private void ChangeStyle(ArabLayoutCompment.ArabLayoutStyle style, bool bEnble)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            RectTransform gui = targets[i] as RectTransform;
            ArabLayoutCompment layout = gui.GetComponent<ArabLayoutCompment>();
            if(layout == null)
            {
                layout = gui.gameObject.AddComponent<ArabLayoutCompment>();
                layout.SetStyle(ArabLayoutCompment.ArabLayoutStyle.ArabLayoutStyle_Preview, m_bArabPreview);
            }
            layout.SetStyle(style, bEnble);
            if (style == ArabLayoutCompment.ArabLayoutStyle.ArabLayoutStyle_Preview)
            {
                var childsCompment = gui.GetComponentsInChildren<ArabLayoutCompment>();
                foreach (var compment in childsCompment)
                {
                    compment.SetStyle(ArabLayoutCompment.ArabLayoutStyle.ArabLayoutStyle_Preview, bEnble);
                    EditorUtility.SetDirty(targets[i]);
                }
                break;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // 多选编辑
        if (targets.Length > 1)
        {
            foreach (ArabLayoutCompment.ArabLayoutStyle item in Enum.GetValues(typeof(ArabLayoutCompment.ArabLayoutStyle)))
            {
                if ((int)item > 0)
                {
                    FieldInfo field = item.GetType().GetField(item.ToString());
                    object[] objs = field.GetCustomAttributes(typeof(ArabLayoutStyleEditor), false);    //获取描述属性
                    if (objs != null && objs.Length > 0)
                    {
                        ArabLayoutStyleEditor styleAttribute = (ArabLayoutStyleEditor)objs[0];
                        bool bValue = true;
                        for (int i = 0; i < targets.Length; i++)
                        {
                            RectTransform gui = targets[i] as RectTransform;
                            ArabLayoutCompment layout = gui.GetComponent<ArabLayoutCompment>();
                            if (!styleAttribute.HasCompament(gui))
                            {
                                if (item == ArabLayoutCompment.ArabLayoutStyle.ArabLayoutStyle_Preview)
                                {
                                    GUILayout.Toggle(m_bArabPreview, styleAttribute.Name);
                                }
                                break;
                            }
                            bool value = layout != null ? layout.GetStyle(item) : false;
                            if(value == false)
                            {
                                bValue = false;
                            }
                            // 表示有一样的参数设置
                            if (i == targets.Length-1)
                            {
                                if (GUILayout.Toggle(bValue, styleAttribute.Name) != bValue)
                                {
                                    ChangeStyle(item, !bValue);
                                    return ;
                                }
                                if (item == ArabLayoutCompment.ArabLayoutStyle.ArabLayoutStyle_Preview)
                                {
                                    return ;
                                }
                            }
                        }
                    }
                }
            }

        }
        else
        {
            // 单选编辑
            RectTransform gui = target as RectTransform;
            // 调试代码
            //GUILayout.Label(string.Format("x:{0}y:{1}w:{2}h:{3}ax:{4}ay:{5} rect:{6}", gui.position.x, gui.position.y, gui.sizeDelta.x, gui.sizeDelta.y, gui.anchoredPosition.x, gui.anchoredPosition.y, gui.rect.ToString()));

            ArabLayoutCompment layout = gui.GetComponent<ArabLayoutCompment>();
            int arabLayout = 0;
            if (layout)
            {
                arabLayout = layout.ArabLayout;
            }
            bool bDirty = false;
            bool bIsRoot = false;
            foreach (ArabLayoutCompment.ArabLayoutStyle item in Enum.GetValues(typeof(ArabLayoutCompment.ArabLayoutStyle)))
            {
                if ((int)item > 0)
                {
                    FieldInfo field = item.GetType().GetField(item.ToString());
                    object[] objs = field.GetCustomAttributes(typeof(ArabLayoutStyleEditor), false);    //获取描述属性
                    if (objs != null && objs.Length > 0)
                    {
                        ArabLayoutStyleEditor styleAttribute = (ArabLayoutStyleEditor)objs[0];
                        if (styleAttribute.HasCompament(gui))
                        {
                            bool bValue = (arabLayout & (int)item) > 0;
                            if (GUILayout.Toggle(bValue, styleAttribute.Name) != bValue)
                            {
                                EditorUtility.SetDirty(target);
                                if (layout)
                                {
                                    layout.SetStyle(item, !bValue);
                                }
                                else
                                {
                                    if (bValue)
                                        arabLayout = arabLayout & ~(int)item;
                                    else
                                        arabLayout = arabLayout | (int)item;
                                    bDirty = true;
                                }

                                // 需要设置所有子空间的 阿语预览
                                if (item == ArabLayoutCompment.ArabLayoutStyle.ArabLayoutStyle_Preview)
                                {
                                    var childsCompment = gui.GetComponentsInChildren<ArabLayoutCompment>();
                                    foreach (var compment in childsCompment)
                                    {
                                        compment.SetStyle(ArabLayoutCompment.ArabLayoutStyle.ArabLayoutStyle_Preview, !bValue);
                                    }
                                }
#if UNITY_EDITOR
                                LayoutRebuilder.ForceRebuildLayoutImmediate(gui);
#endif
                            }
                            if (item == ArabLayoutCompment.ArabLayoutStyle.ArabLayoutStyle_Preview)
                            {
                                bIsRoot = true;
                                break;
                            }
                        }
                        else if (item == ArabLayoutCompment.ArabLayoutStyle.ArabLayoutStyle_Preview)
                        {
                            bool bValue = (arabLayout & (int)item) > 0;
                            GUILayout.Toggle(bValue, styleAttribute.Name);
                        }
                    }
                }
            }
            if (bDirty)
            {
                layout = gui.gameObject.AddComponent<ArabLayoutCompment>();
                layout.ArabLayout = arabLayout;
                if (!bIsRoot)
                {
                    CanvasScaler root = gui.GetComponentInParent<CanvasScaler>();
                    if (root)
                    {
                        var rootLayout = root.GetComponent<ArabLayoutCompment>();
                        if (rootLayout)
                        {
                            layout.SetStyle(ArabLayoutCompment.ArabLayoutStyle.ArabLayoutStyle_Preview, rootLayout.GetStyle(ArabLayoutCompment.ArabLayoutStyle.ArabLayoutStyle_Preview));
#if UNITY_EDITOR
                            LayoutRebuilder.ForceRebuildLayoutImmediate(gui);
#endif
                        }
                    }
                }
            }
        }
    }
}