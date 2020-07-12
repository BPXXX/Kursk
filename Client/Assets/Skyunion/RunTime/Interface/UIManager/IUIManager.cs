using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Skyunion
{
    public delegate object CreateInstance(Type type);

    public delegate void OnShowUI(UIInfo ui);
    public delegate void OnCloseUI(UIInfo ui);
    public interface IUIManager : IModule
    {
        void AddShowUIListener(OnShowUI param);
        void RemoveShowUIListener(OnShowUI param);

        void AddCloseUIListener(OnCloseUI param);
        void RemoveCloseUIListener(OnCloseUI param);

        void ShowUI(UIInfo uiInfo, Action callback = null, object data = null);
        void HideUI(UIInfo uiInfo);
        void CloseUI(UIInfo uiInfo, bool IsManual = true, bool IsForceClose = false, bool returnToLogin = false);
        void CloseAll(bool isForceClose = false, bool returnToLogin = false);
        void CloseLayerUI(UILayer uiLayer, bool isForceClose = false, bool returnToLogin = false);

        bool ExistUI(UIInfo uiInfo);

        bool ExistUI(int uiId);

        UIInfo GetUI(int uiId);

        Canvas GetCanvas();

        Camera GetUICamera();

        RectTransform GetUILayer(int layerIndex);

        bool IsHasPopView();
    }
}
