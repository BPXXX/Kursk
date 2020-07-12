// **********************************************************************
//
// Copyright (c) 2017
// All Rights Reserved
//
// Author: Johance
// Email:421465201@qq.com
// Created:	2017/6/13   08:00
//
// **********************************************************************
using UnityEngine;
using UnityEngine.UI;
using System.Collections;  
  
namespace Skyunion
{
    public class FpsScript : MonoBehaviour {

        public float UpdateInterval = 0.5F;     //在每个updateInterval间隔处计算，帧/秒，这样显示就不会随意的改变  

        private float LastInterval;     // Last interval end time 最后间隔结束时间  

        private int Frames = 0;         // Frames over current interval 超过当前间隔帧  

        private float Fps;              // Current FPS //当前FPS

        private Rect DrawRect = new Rect(0, 0, 100, 20);
            
        private GUIStyle guiStyle = new GUIStyle();

        void Start()
        {
            //Application.targetFrameRate=30;       //限定帧数  

            LastInterval = Time.realtimeSinceStartup;  //自游戏开始实时时间  

            Frames = 0;
            guiStyle.fontSize = 40;       //当然，这是字体大小
        }

        void OnGUI()
        {
            GUI.TextArea(DrawRect, "FPS:" + Fps.ToString("f2"), guiStyle);       //显示两位小数  
        }

        void Update()
        {
            ++Frames;

            if (Time.realtimeSinceStartup > LastInterval + UpdateInterval)
            {
                Fps = Frames / (Time.realtimeSinceStartup - LastInterval);

                Frames = 0;

                LastInterval = Time.realtimeSinceStartup;
            }
        }
    }
}