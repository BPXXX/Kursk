using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Skyunion;

public class GameButton : Button {
    [HideInInspector]
    [SerializeField]
    public bool AutoPlaySound = true;
    [HideInInspector]
    [SerializeField]
    public string SoundAssetName = "Sound_Ui_CommonClickButton";
    [HideInInspector]
    [SerializeField]
    public bool AutoPlayRewardSound = false;
    [HideInInspector]
    [SerializeField]
    public string RewardSounedAssetName = "Sound_Ui_CommonReward";
    private Action pressCallback;
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if(AutoPlaySound)
        {
            CoreUtils.audioService.PlayOneShot(SoundAssetName);
        }
        if(AutoPlayRewardSound)
        {
            CoreUtils.audioService.PlayOneShot(RewardSounedAssetName);
        }
    }

    public void SetSound(string soundName)
    {
        SoundAssetName = soundName;
    }

    public void SetMute()
    {
        this.AutoPlaySound = false;
    }
    
    private bool isPress = false;
    public override void OnPointerUp(PointerEventData eventData)
    {
        isPress = false;
        CancelInvoke("OnPress");
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        isPress = true;
        InvokeRepeating("OnPress", 0, 0.1f);
    }

    private void OnPress()
    {
        if (isPress)
        {
            if (pressCallback != null)
            {
                pressCallback.Invoke();
            }
        }
    }

    public void AddPressClick(Action callback)
    {
        pressCallback += callback;
    }
    
    public void RemovePressClick(Action callback)
    {
        pressCallback -= callback;
    }

    
}
