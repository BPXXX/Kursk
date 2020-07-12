using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Skyunion
{
    internal class InputManager : Module, IInputManager
    {
        public static bool IsBgActived = false;

        private bool isTouching = false;

        private bool isMoving = false;

        private int lastTouchX = 0;

        private int lastTouchY = 0;

        private string m_touch3DGameobjectId = string.Empty;

        // 忽略判断点击UI的面板名称
        private HashSet<string> m_ignoreUITouchPanels = new HashSet<string>();

        private bool isMouseScaling = false;

        private bool isTwoTouchZooming = false;

        private float lastTowTouchDist = 0f;

        private float firstTwoTouchDist = 0f;

        private const  int MIN_MOVE_START_DIST = 10;

        private int[] Inputcountpre = new int[3];

        private bool m_ignoreUITouched = false;

        private Action<int, int> m_OnTouchBeginAction;
        private Action<int, int> m_OnTouchMoveAction;
        private Action<int, int> m_OnTouchEndAction;
        private Action<int, int> m_OnTouchZoomBeginAction;
        private Action<int, int, float> m_OnTouchZoomedAction;


        private Action<int, int, string, string> m_OnTouchBegan3DCheckAction;
        private Action<int, int, string, string> m_OnTouchEnded3DCheckAction;
        private Action<int, int, string, string> m_On3DCheckAction;
        private Action<int, int, string, string> m_OnTouch3DReleaseOutside;

        #region 实现 Module
        public override void Init()
        {
            isTouching = false;
            isMoving = false;
            InitTouchBg();
            OnInitialized();
        }
        #endregion

        private void RecordInputCount(int count)
        {
            int num = Inputcountpre.Length;
            if (num == 1)
            {
                Inputcountpre[0] = count;
                return;
            }
            for (int i = 0; i < num - 1; i++)
            {
                Inputcountpre[i + 1] = Inputcountpre[i];
            }
            Inputcountpre[0] = count;
        }

        private bool isAllInputCountIsZero()
        {
            int num = Inputcountpre.Length;
            for (int i = 0; i < num; i++)
            {
                if (Inputcountpre[i] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        public override void Update()
        {
            int touchCount = GetTouchCount();
            bool flag = touchCount <= 1;
            RecordInputCount(touchCount);
            if (isAllInputCountIsZero())
            {
                isTouching = false;
            }
            if (flag)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    int num = (int)Input.mousePosition.x;
                    int num2 = (int)Input.mousePosition.y;
                    if (!isTouching && num >= 0 && num <= Screen.width && num2 >= 0 && num2 <= Screen.height)
                    {
                        OnTouchBegan(num, num2);
                        TouchBegan3DCheck(num, num2);
                    }
                }
                else if (Input.GetMouseButton(0))
                {
                    int num3 = (int)Input.mousePosition.x;
                    int num4 = (int)Input.mousePosition.y;
                    if (num3 >= 0 && num3 <= Screen.width && num4 >= 0 && num4 <= Screen.height && (isMoving || Mathf.Abs(lastTouchX - num3) > 10 || Mathf.Abs(lastTouchY - num4) > 10) && (num3 != lastTouchX || num4 != lastTouchY))
                    {
                        OnTouchMoved(num3, num4);
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    int x = (int)Input.mousePosition.x;
                    int y = (int)Input.mousePosition.y;
                    if (isTouching)
                    {
                        OnTouchEnded(x, y);
                        TouchEnded3DCheck(x, y);

                    }
}
                else
                {
#if UNITY_EDITOR || UNITY_STANDALONE
                    float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
                    if (mouseWheel != 0)
                    {
                        int num3 = (int)Input.mousePosition.x;
                        int num4 = (int)Input.mousePosition.y;
                        if (num3 >= 0 && num3 <= Screen.width && num4 >= 0 && num4 <= Screen.height)
                        {
                            IsTouchedUI();
                            if (!m_ignoreUITouched)
                            {
                                m_OnTouchZoomBeginAction?.Invoke(-1, -1);
                                m_OnTouchZoomedAction?.Invoke(num3, num4, 1.0f / (mouseWheel + 1.0f));
                            }
                        }
                    }
#endif
                }
                isTwoTouchZooming = false;
            }
            else if (touchCount == 2)
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                Touch touch = new Touch();
                touch.phase = TouchPhase.Moved;
                touch.position = Input.mousePosition;
                Touch touch2 = touch;
                Vector2 vector = new Vector2(Screen.width, Screen.height)*0.5f;
                touch2.position = vector-(touch.position - vector);
#else
                Touch touch = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);
                Vector2 vector = (touch.position + touch2.position) * 0.5f;
#endif
                int centerX = (int)vector.x;
                int centerY = (int)vector.y;
                float num5 = Vector2.Distance(touch.position, touch2.position);
                if (!isTwoTouchZooming && !IsTouchedUI())
                {
                    isTwoTouchZooming = true;
                    lastTowTouchDist = num5;
                    firstTwoTouchDist = num5;
                    m_OnTouchZoomBeginAction?.Invoke(-1, -1);
                }
                else
                {
                    float f = lastTowTouchDist - num5;
                    if (Mathf.Abs(f) > 1f && (touch.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved) && lastTowTouchDist > 0f)
                    {
                        m_OnTouchZoomedAction?.Invoke(centerX, centerY, firstTwoTouchDist / lastTowTouchDist);
                        lastTowTouchDist = num5;
                    }
                }
                isTouching = false;
            }
            else if (isTwoTouchZooming)
            {
                isTwoTouchZooming = false;
            }
            IsBgActived = false;
        }

        public void InitTouchBg()
        {
            if(InputHandler.Instance == null)
            {
                var InputBG = CoreUtils.assetService.Instantiate(Resources.Load<GameObject>("InputBg"));
                Debug.LogWarning("你必须要把InputManager/Resources的预制体放到UIRoot节点下或者参考此预制体在场景创建一份！");
            }
            if (InputHandler.Instance.transform.parent == null)
            {
                UnityEngine.Object.DontDestroyOnLoad(InputHandler.Instance.gameObject);
            }
        }

        private Transform FindInParent(Transform trans, string name)
        {
            if (!(trans != null))
            {
                return null;
            }
            if (trans.name == name)
            {
                return trans;
            }
            return FindInParent(trans.parent, name);
        }

        public bool IsTouchedUI()
        {
            m_ignoreUITouched = false;
            EventSystem current = EventSystem.current;
            if (current != null)
            {
                GameObject currentSelectedGameObject = current.currentSelectedGameObject;
                bool result;
                if (currentSelectedGameObject != null)
                {
                    if (IsBgActived || !(currentSelectedGameObject.GetComponentInParent<Canvas>() != null))
                    {
                        return false;
                    }
                    if (!currentSelectedGameObject.name.StartsWith("BLOCK_UI_ONLY"))
                    {
                        Transform transform = currentSelectedGameObject.transform;
                        foreach (string current2 in m_ignoreUITouchPanels)
                        {
                            if (FindInParent(transform, current2) != null)
                            {
                                m_ignoreUITouched = true;
                                result = false;
                                return result;
                            }
                        }
                        return true;
                    }
                    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                    pointerEventData.position = Input.mousePosition;
                    List<RaycastResult> list = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointerEventData, list);
                    if (!(currentSelectedGameObject.name == "BLOCK_UI_ONLY_WITHOUT_COLLECT"))
                    {
                        return list.Count != 1 || !list[0].gameObject.name.StartsWith("BLOCK_UI_ONLY");
                    }
                    if (list.Count == 3)
                    {
                        return !(list[0].gameObject.name == "FTEButton") || !(list[1].gameObject.name == "BLOCK_UI_ONLY_WITHOUT_COLLECT") || (!(list[2].gameObject.name == "Clover_PopUp_Res") && !(list[2].gameObject.name == "Clover_PopUp_CDPopUp2"));
                    }
                    return list.Count != 1 || !list[0].gameObject.name.StartsWith("BLOCK_UI_ONLY_WITHOUT_COLLECT");
                }
                else
                {
                    if (!IsBgActived)
                    {
                        PointerEventData pointerEventData2 = new PointerEventData(EventSystem.current);
                        pointerEventData2.position = Input.mousePosition;
                        List<RaycastResult> list2 = new List<RaycastResult>();
                        EventSystem.current.RaycastAll(pointerEventData2, list2);
                        foreach (RaycastResult current3 in list2)
                        {
                            GameObject gameObject = current3.gameObject;
                            if (gameObject.GetComponent<MaskableGraphic>() != null && gameObject != InputHandler.Instance.gameObject)
                            {
                                // 原始代码是没有   m_ignoreUITouched = true; 的不知道是不是bug
                                m_ignoreUITouched = true;
                                result = true;
                                return result;
                            }
                        }
                        return false;
                    }
                    return false;
                }
            }
            return false;
        }

        public bool IsTouchUIGiven(List<GameObject> objects)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> list = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, list);
            foreach (RaycastResult current in list)
            {
                foreach (var current2 in objects)
                {
                    if (current2 != null && current.gameObject == current2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void SetIgnoreUITouchPanels(string[] panelNames)
        {
            m_ignoreUITouchPanels.Clear();
            for (int i = 0; i < panelNames.Length; i++)
            {
                string item = panelNames[i];
                m_ignoreUITouchPanels.Add(item);
            }
        }

        public int GetTouchCount()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButton(2) && Input.GetMouseButton(0))
                return 2;
            return Input.GetMouseButton(0) ? 1 : 0;
#else
            return Input.touchCount;
#endif
        }

        private void OnTouchBegan(int x, int y)
        {
            if (!IsBgActived)
                return;

            lastTouchX = x;
            lastTouchY = y;
            isTouching = true;
            m_OnTouchBeginAction?.Invoke(x, y);
        }

        private void OnTouchMoved(int x, int y)
        {
            if (!isTouching)
                return;

            lastTouchX = x;
            lastTouchY = y;
            isMoving = true;
            m_OnTouchMoveAction?.Invoke(x, y);
        }

        private void OnTouchEnded(int x, int y)
        {
            if (!isTouching)
                return;

            isTouching = false;

            IsTouchedUI();
            if (m_ignoreUITouched && !isMoving)
            {
                return;
            }
            isMoving = false;
            m_OnTouchEndAction?.Invoke(x, y);
        }

        public bool TouchBegan3DCheck(int x, int y)
        {
            if (IsTouchedUI())
            {
                return false;
            }
            if (!Camera.main)
            {
                return false;
            }
            Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)x, (float)y, 0f));
            int num = -1;
            int num2 = LayerMask.NameToLayer("RaycastCollider");
            RaycastHit[] array = Physics.RaycastAll(ray, float.PositiveInfinity, 1 << num2);
            for (int i = 0; i < array.Length; i++)
            {
                if (num == -1 || array[num].transform.position.y < array[i].transform.position.y)
                {
                    num = i;
                }
            }
            if (num > -1)
            {
                RaycastHit raycastHit = array[num];
                m_touch3DGameobjectId = raycastHit.transform.parent.gameObject.name;
                BoxCollider boxCollider = raycastHit.collider as BoxCollider;
                m_OnTouchBegan3DCheckAction?.Invoke(x, y, m_touch3DGameobjectId, boxCollider.name);
                return true;
            }
            return false;
        }

        public bool TouchEnded3DCheck(int x, int y)
        {
            if (IsTouchedUI())
            {
                return false;
            }
            if (!Camera.main)
            {
                return false;
            }
            Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)x, (float)y, 0f));
            int num = LayerMask.NameToLayer("RaycastCollider");
            string text = string.Empty;
            bool flag = false;
            RaycastHit[] array = Physics.RaycastAll(ray, float.PositiveInfinity, 1 << num);
            string colliderId = string.Empty;
            for (int i = 0; i < array.Length; i++)
            {
                text = array[i].transform.parent.gameObject.name;
                if (m_touch3DGameobjectId == text)
                {
                    flag = true;
                    BoxCollider boxCollider = array[i].collider as BoxCollider;
                    colliderId = boxCollider.name;
                    m_On3DCheckAction?.Invoke(x, y, text, colliderId);
                    break;
                }
            }
            if (!flag && m_touch3DGameobjectId != string.Empty)
            {
                m_OnTouch3DReleaseOutside?.Invoke(x, y, m_touch3DGameobjectId, colliderId);
            }
            m_touch3DGameobjectId = string.Empty;
            m_OnTouchEnded3DCheckAction?.Invoke(x, y, text, colliderId);
            return flag;
        }

        public void AddTouch2DEvent(Action<int, int> eventOnToucheBegin, Action<int, int> eventOnToucheMove, Action<int, int> eventOnToucheEnd)
        {
            m_OnTouchBeginAction += eventOnToucheBegin;
            m_OnTouchMoveAction += eventOnToucheMove;
            m_OnTouchEndAction += eventOnToucheEnd;
        }

        public void RemoveTouch2DEvent(Action<int, int> eventOnToucheBegin, Action<int, int> eventOnToucheMove, Action<int, int> eventOnToucheEnd)
        {
            m_OnTouchBeginAction -= eventOnToucheBegin;
            m_OnTouchMoveAction -= eventOnToucheMove;
            m_OnTouchEndAction -= eventOnToucheEnd;
        }


        public void SetTouchZoomEvent(Action<int, int> eventOnToucheZoomBegin, Action<int, int, float> eventOnToucheZoomed)
        {
            m_OnTouchZoomBeginAction = eventOnToucheZoomBegin;
            m_OnTouchZoomedAction = eventOnToucheZoomed;
        }

        public void SetTouch3DEvent(Action<int, int, string, string> eventOnTouche3DBegin, Action<int, int, string, string> eventOnTouche3D, Action<int, int, string, string> eventOnTouche3DEnd, Action<int, int, string, string> eventOnTouche3DReleaseOutside)
        {
            m_OnTouchBegan3DCheckAction = eventOnTouche3DBegin;
            m_On3DCheckAction = eventOnTouche3D;
            m_OnTouchEnded3DCheckAction = eventOnTouche3DEnd;
            m_OnTouch3DReleaseOutside = eventOnTouche3DReleaseOutside;
        }
    }
}
