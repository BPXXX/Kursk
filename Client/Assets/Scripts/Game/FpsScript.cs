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

        public float UpdateInterval = 0.5F;     //��ÿ��updateInterval��������㣬֡/�룬������ʾ�Ͳ�������ĸı�  

        private float LastInterval;     // Last interval end time ���������ʱ��  

        private int Frames = 0;         // Frames over current interval ������ǰ���֡  

        private float Fps;              // Current FPS //��ǰFPS

        private Rect DrawRect = new Rect(0, 0, 100, 20);
            
        private GUIStyle guiStyle = new GUIStyle();

        void Start()
        {
            //Application.targetFrameRate=30;       //�޶�֡��  

            LastInterval = Time.realtimeSinceStartup;  //����Ϸ��ʼʵʱʱ��  

            Frames = 0;
            guiStyle.fontSize = 40;       //��Ȼ�����������С
        }

        void OnGUI()
        {
            GUI.TextArea(DrawRect, "FPS:" + Fps.ToString("f2"), guiStyle);       //��ʾ��λС��  
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