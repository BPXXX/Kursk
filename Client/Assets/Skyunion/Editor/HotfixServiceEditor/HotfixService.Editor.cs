//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Skyunion
{
    public class HotfixServiceEditor : UnityEditor.Editor
    {
        public static string HotfixDebugModeKey = "HofixService_HofixMode";
        private const string mSelectHotfixModeNativeMenuPath = "Skyunion/HotService/本地代码模式";
        private const string mSelectHotfixModeRefletMenuPath = "Skyunion/HotService/脚本反射模式";
        private const string mSelectHotfixModeILRTMenuPath = "Skyunion/HotService/脚本ILRT模式";

        [MenuItem(mSelectHotfixModeNativeMenuPath, priority = 1000)]
        public static void SelectNativeMode_Menu()
        {
            EditorPrefs.SetInt(HotfixDebugModeKey, (int)HotfixMode.NativeCode);
            Menu.SetChecked(mSelectHotfixModeNativeMenuPath, true);
            Menu.SetChecked(mSelectHotfixModeRefletMenuPath, false);
            Menu.SetChecked(mSelectHotfixModeILRTMenuPath, false);
        }

        [MenuItem(mSelectHotfixModeNativeMenuPath, true)]
        public static bool SelectNativeMode_Check_Menu()
        {
            Menu.SetChecked(mSelectHotfixModeNativeMenuPath, EditorPrefs.GetInt(HotfixDebugModeKey, (int)HotfixMode.NativeCode) == (int)HotfixMode.NativeCode);
            return true;
        }

        [MenuItem(mSelectHotfixModeRefletMenuPath, priority = 1000)]
        public static void SelectReflectMode_Menu()
        {
            EditorPrefs.SetInt(HotfixDebugModeKey, (int)HotfixMode.Reflect);
            Menu.SetChecked(mSelectHotfixModeNativeMenuPath, false);
            Menu.SetChecked(mSelectHotfixModeRefletMenuPath, true);
            Menu.SetChecked(mSelectHotfixModeILRTMenuPath, false);
        }

        [MenuItem(mSelectHotfixModeRefletMenuPath, true)]
        public static bool SelectReflectMode_Check_Menu()
        {
            Menu.SetChecked(mSelectHotfixModeRefletMenuPath, EditorPrefs.GetInt(HotfixDebugModeKey, (int)HotfixMode.NativeCode) == (int)HotfixMode.Reflect);
            return true;
        }
        [MenuItem(mSelectHotfixModeILRTMenuPath, priority = 1000)]
        public static void SelectILRTMode_Menu()
        {
            EditorPrefs.SetInt(HotfixDebugModeKey, (int)HotfixMode.ILRT);
            Menu.SetChecked(mSelectHotfixModeNativeMenuPath, false);
            Menu.SetChecked(mSelectHotfixModeRefletMenuPath, false);
            Menu.SetChecked(mSelectHotfixModeILRTMenuPath, true);
            UnityScripsCompiling.OnUnityScripsCompilingCompleted();
        }

        [MenuItem(mSelectHotfixModeILRTMenuPath, true)]
        public static bool SelectILRTMode_Check_Menu()
        {
            Menu.SetChecked(mSelectHotfixModeILRTMenuPath, EditorPrefs.GetInt(HotfixDebugModeKey, (int)HotfixMode.NativeCode) == (int)HotfixMode.ILRT);
            return true;
        }
    }
}
