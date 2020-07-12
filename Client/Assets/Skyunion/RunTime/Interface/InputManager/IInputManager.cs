using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Skyunion
{
    public interface IInputManager : IModule
    {
        bool IsTouchedUI();
        bool IsTouchUIGiven(List<GameObject> objects);
        void SetIgnoreUITouchPanels(string[] panelNames);
        int GetTouchCount();
        void AddTouch2DEvent(Action<int, int> eventOnToucheBegin, Action<int, int> eventOnToucheMove, Action<int, int> eventOnToucheEnd);
        void RemoveTouch2DEvent(Action<int, int> eventOnToucheBegin, Action<int, int> eventOnToucheMove, Action<int, int> eventOnToucheEnd);
        void SetTouchZoomEvent(Action<int, int> eventOnToucheZoomBegin, Action<int, int, float> eventOnToucheZoomed);
        void SetTouch3DEvent(Action<int, int, string, string> eventOnTouche3DBegin, Action<int, int, string, string> eventOnTouche3D, Action<int, int, string, string> eventOnTouche3DEnd, Action<int, int, string, string> eventOnTouche3DReleaseOutside);
    }
}
