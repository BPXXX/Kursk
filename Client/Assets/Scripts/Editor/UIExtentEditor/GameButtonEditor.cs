using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameButton))]
public class GameButtonEditor : Editor
{
    private GameButton gameButton;

    SerializedProperty AutoPlaySound;
    SerializedProperty SoundAssetName;
    SerializedProperty AutoPlayRewardSound;
    SerializedProperty RewardSounedAssetName;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(gameButton!=null)
        {
            AutoPlaySound = serializedObject.FindProperty("AutoPlaySound");
            SoundAssetName = serializedObject.FindProperty("SoundAssetName");
            AutoPlayRewardSound = serializedObject.FindProperty("AutoPlayRewardSound");
            RewardSounedAssetName = serializedObject.FindProperty("RewardSounedAssetName");
            serializedObject.Update();

            AutoPlaySound.boolValue =  GUILayout.Toggle(gameButton.AutoPlaySound,"自动播放通用按钮音效");
            SoundAssetName.stringValue = EditorGUILayout.TextField("通用按钮音效资源",gameButton.SoundAssetName);

            AutoPlayRewardSound.boolValue = GUILayout.Toggle(gameButton.AutoPlayRewardSound, "自动播放通用领取奖励音效");
            RewardSounedAssetName.stringValue = EditorGUILayout.TextField("通用领取奖励音效资源",gameButton.RewardSounedAssetName);
            serializedObject.ApplyModifiedProperties();
        }

    }

    private void OnEnable()
    {
        gameButton = (GameButton)target;
    }
}
