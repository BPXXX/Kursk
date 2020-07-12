using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Client;
public class UIAnimationCurveWindow : EditorWindow
{

    public AnimationClip m_animationClip;

    public AnimationCurve m_animationCurve = new AnimationCurve();

    public EasingEquation m_ease = EasingEquation.Linear;

    public bool m_useCustomize = false;
    bool[] bindSelect;
    bool allSelect;
    [MenuItem("Tools/UIAnimation/修改动画曲线")]
    private static void ShowWindow()
    {
        UIAnimationCurveWindow win = GetWindow<UIAnimationCurveWindow>(true, "修改动画曲线");
    }
    
    
    private void OnGUI()
    {
        GUILayout.Space(20);
        m_animationCurve = EditorGUILayout.CurveField(m_animationCurve);
        GUILayout.Space(15);
        m_animationClip = EditorGUILayout.ObjectField(m_animationClip, typeof(AnimationClip), true) as AnimationClip;
        GUILayout.Space(15);
        m_ease = (EasingEquation)EditorGUILayout.EnumPopup(m_ease);
        GUILayout.Space(20);
        if (m_animationClip != null)
        {
            EditorCurveBinding[] binds = AnimationUtility.GetCurveBindings(m_animationClip);
            if (bindSelect == null || bindSelect.Length < binds.Length)
            {
                bindSelect = new bool[binds.Length];
            }
            if (binds != null)
            {
                for (int i = 0; i < binds.Length; i++)
                {
                    //GUILayout.Label($"{binds[i].path}\\{binds[i].propertyName}");
                    bindSelect[i] = GUILayout.Toggle(bindSelect[i], $"{binds[i].path}\\{binds[i].propertyName}");
                }
            }
            
            GUILayout.Space(5);

            allSelect = EditorGUILayout.Toggle("全选", allSelect);
            if (allSelect)
            {
                for (int i = 0; i < bindSelect.Length; i++)
                {
                    bindSelect[i] = true;
                }
            }
            m_useCustomize = EditorGUILayout.Toggle("是否使用自定义曲线", m_useCustomize);
            GUILayout.Space(5);
            if (GUILayout.Button("生成速度缓动曲线"))
            {
                bool success = false;
                for (int i = 0; i < binds.Length; i++)
                {
                    if (!bindSelect[i])
                    {
                        return;
                    }
                    AnimationCurve tmpCurve = AnimationUtility.GetEditorCurve(m_animationClip, binds[i]);
                    for (int j = tmpCurve.keys.Length - 2; j > 0; j--)
                    {
                        tmpCurve.RemoveKey(j);
                    }
                    if (tmpCurve.keys.Length == 2)
                    {
                        Keyframe firstKey = tmpCurve.keys[0];
                        Keyframe lastKey = tmpCurve.keys[1];
                        float frameCount = (lastKey.time - firstKey.time) * m_animationClip.frameRate;

                        for (float j = 0; j < frameCount; j++)
                        {
                            float tmpTime = firstKey.time + j / m_animationClip.frameRate;
                            Keyframe key;
                            if (m_useCustomize)
                            {
                                if (m_animationCurve == null)
                                {
                                    Debug.LogError("自定义曲线为空!");
                                    return;
                                }
                                float range = m_animationCurve.Evaluate((j / m_animationClip.frameRate) / (lastKey.time - firstKey.time));
                                key = new Keyframe(tmpTime, range * (lastKey.value - firstKey.value) + firstKey.value);
                            }
                            else
                            {
                                float range = EasingManager.GetEaseProgress(m_ease, (j / m_animationClip.frameRate) / (lastKey.time - firstKey.time));
                                key = new Keyframe(tmpTime, range * (lastKey.value - firstKey.value) + firstKey.value);
                            }
                            tmpCurve.AddKey(key);
                            success = true;
                        }
                        for(int j = 0;j<tmpCurve.keys.Length;j++)
                        {
                            AnimationUtility.SetKeyLeftTangentMode(tmpCurve, j, AnimationUtility.TangentMode.ClampedAuto);
                            AnimationUtility.SetKeyRightTangentMode(tmpCurve, j, AnimationUtility.TangentMode.ClampedAuto);
                        }

                    }
                    AnimationUtility.SetEditorCurve(m_animationClip, binds[i], tmpCurve);
                }
                if (success)
                {
                    Debug.Log("设置成功！");
                }
                else
                {
                    Debug.LogError("设置失败！确保已经设置了正确的动画");
                }
            }

            if (GUILayout.Button("清除曲线设置"))
            {
                bool success = false;
                for (int i = 0; i < binds.Length; i++)
                {
                    AnimationCurve tmpCurve = AnimationUtility.GetEditorCurve(m_animationClip, binds[i]);
                    for (int j = tmpCurve.keys.Length - 2; j > 0; j--)
                    {
                        tmpCurve.RemoveKey(j);
                        success = true;
                    }
                    AnimationUtility.SetEditorCurve(m_animationClip, binds[i], tmpCurve);
                }
                if (success)
                {
                    Debug.Log("清除成功!");
                }
            }
        }
        GUILayout.Space(20);
    }


}

