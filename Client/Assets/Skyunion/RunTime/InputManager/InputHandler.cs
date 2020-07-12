using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Skyunion
{
    public class InputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
    {
        public static InputHandler Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            InputManager.IsBgActived = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            InputManager.IsBgActived = true;
        }
    }
}