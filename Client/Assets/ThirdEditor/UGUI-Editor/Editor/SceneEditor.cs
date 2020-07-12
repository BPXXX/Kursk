using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace U3DExtends { 
public class SceneEditor {

    static Object LastSelectObj = null;//用来记录上次选中的GameObject，只有它带有Image组件时才把图片赋值给它
    static Object CurSelectObj = null;
    
    
    [InitializeOnLoadMethod]
    static void Init()
    {
        SceneView.duringSceneGui += OnSceneGUI;

        //选中Image节点并点击图片后即帮它赋上图片
        if (Configure.IsEnableFastSelectImage)
            Selection.selectionChanged += OnSelectChange;
    }

    static void OnSelectChange()
    {
        LastSelectObj = CurSelectObj;
        CurSelectObj = Selection.activeObject;
        //如果要遍历目录，修改为SelectionMode.DeepAssets
        UnityEngine.Object[] arr = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.TopLevel);
        if (arr != null && arr.Length > 0)
        {
            GameObject selectObj = LastSelectObj as GameObject;
            if (selectObj != null && (arr[0] is Sprite || arr[0] is Texture2D))
            {
                string assetPath = AssetDatabase.GetAssetPath(arr[0]);
                Image image = selectObj.GetComponent<Image>();
                bool isImgWidget = false;
                if (image != null)
                {
                    isImgWidget = true;
                    UIEditorHelper.SetImageByPath(assetPath, image, Configure.IsAutoSizeOnFastSelectImg);
                }
                if (isImgWidget)
                {
                    //赋完图后把焦点还给Image节点
                    EditorApplication.delayCall = delegate
                    {
                        Selection.activeGameObject = LastSelectObj as GameObject;
                    };
                }
            }
        }
    }
    
    static GUIStyle buttonStyle = new GUIStyle();
    
    private static List<RectTransform> selectObjs = new List<RectTransform>();
    private static Vector3 selectPos;
    private static int selectIndex = -1;
    
    static void GetAllOverlapping(Vector2 mousePosition, List<GameObject> list)
    {
        list.Clear();

        GameObject nearestGameObject = null;

        do
        {
            nearestGameObject = HandleUtility.PickGameObject(mousePosition, false, list.ToArray());

            if (nearestGameObject != null)
                list.Add(nearestGameObject);
            else
                break;
        }
        while (nearestGameObject != null);
    }

    private static List<GameObject> _selecedObjs = new List<GameObject>();

    
   
    static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;


        bool is_handled = false;
        
        if (sceneView.in2DMode)
        {
            
            //2D模式下重写鼠标选择UI模式
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                Vector3 mousePos = e.mousePosition;
                float mult = EditorGUIUtility.pixelsPerPoint;
                mousePos.y = sceneView.camera.pixelHeight - mousePos.y * mult;
                mousePos.x *= mult;


                if (selectPos == null || !selectPos.Equals(e.mousePosition))
                {
                    selectObjs.Clear();
                    selectPos = e.mousePosition;
                    selectIndex = -1;
                    
                    GetAllOverlapping(e.mousePosition, _selecedObjs);

                    foreach (var obj in _selecedObjs)
                    {
                        var trt = obj.GetComponent<RectTransform>();

                        if (trt!=null)
                        {
                            if (trt.childCount == 0)
                            {
                                continue;
                            }
                            if (!selectObjs.Contains(trt))
                            {
                                selectObjs.Add(trt);
                                Debug.LogFormat("点击到 " + trt.gameObject.name);
                            }
                        }
                        
                    }
                
                }
               
            }

            if (e.type == EventType.Used && selectObjs.Count>0)
            {
                Debug.Log(e.mousePosition+"   "+selectPos);
                if (e.mousePosition.Equals(selectPos))
                {
                    selectIndex++;
                    if (selectIndex>selectObjs.Count-1)
                    {
                        selectIndex = 0;
                    }
                    Selection.activeTransform = selectObjs[selectIndex];
                    Selection.activeGameObject = selectObjs[selectIndex].gameObject;
                    is_handled = true;
                    Debug.Log("循环选择"+selectIndex+"  "+selectObjs[selectIndex]);
                }
            }
            
            
            Handles.BeginGUI();

            if (GUI.Button(new Rect(5, 8, 50, 20), $"ShowUI"))
                SetSceneViewZoom(sceneView, 50f);

            int x = 60;
            int y = 10;
            foreach (var name in Configure.QuickUICreate)
            {
                Rect rect = new Rect(x, y, 100, 20);
                string nameshort = name.Substring(name.LastIndexOf("/") + 1);
                GUI.Box(rect, nameshort);
                if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
                {
                    Transform last_trans = Selection.activeTransform;
                    bool isOk = EditorApplication.ExecuteMenuItem(name);
                    if (isOk)
                    {
//                        Selection.activeTransform.SetParent(UIEditorHelper.GetGoodContainer(last_trans), false);
                    }
                }
                x += 110;
                
                if (x > sceneView.position.width-50)
                {
                    y += 30;
                    x = 60;
                }
            }
            Handles.EndGUI();
            SceneView.RepaintAll ();

            
        }

  
        
        if (Configure.IsEnableDragUIToScene && (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform))
        {
            
            if (DragAndDrop.objectReferences.Length==0)
            {
                return;
            }
            //拉UI prefab或者图片入scene界面时帮它找到鼠标下的Canvas并挂在其上，若鼠标下没有画布就创建一个
            Object handleObj = DragAndDrop.objectReferences[0];
            if (!IsNeedHandleAsset(handleObj))
            {
                //让系统自己处理
                return;
            }
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            //当松开鼠标时
            if (Event.current.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                foreach (var item in DragAndDrop.objectReferences)
                {
                    HandleDragAsset(sceneView, item);
                }

                PrefabWin.instance.EndMouseDrop();
            }
            is_handled = true;
        }
        else if (e.type == EventType.KeyDown && Configure.IsMoveNodeByArrowKey)
        {
            //按上按下要移动节点，因为默认情况下只是移动Scene界面而已
            foreach (var item in Selection.transforms)
            {
                Transform trans = item;
                if (trans != null)
                {
                    if (e.keyCode == KeyCode.UpArrow)
                    {
                        Vector3 newPos = new Vector3(trans.localPosition.x, trans.localPosition.y + 1, trans.localPosition.z);
                        trans.localPosition = newPos;
                        is_handled = true;
                    }
                    else if (e.keyCode == KeyCode.DownArrow)
                    {
                        Vector3 newPos = new Vector3(trans.localPosition.x, trans.localPosition.y - 1, trans.localPosition.z);
                        trans.localPosition = newPos;
                        is_handled = true;
                    }
                    else if (e.keyCode == KeyCode.LeftArrow)
                    {
                        Vector3 newPos = new Vector3(trans.localPosition.x - 1, trans.localPosition.y, trans.localPosition.z);
                        trans.localPosition = newPos;
                        is_handled = true;
                    }
                    else if (e.keyCode == KeyCode.RightArrow)
                    {
                        Vector3 newPos = new Vector3(trans.localPosition.x + 1, trans.localPosition.y, trans.localPosition.z);
                        trans.localPosition = newPos;
                        is_handled = true;
                    }
                }
            }
        }
        else if (Event.current != null && Event.current.button == 1 && Event.current.type == EventType.MouseUp && Configure.IsShowSceneMenu)
        {
            if (Selection.gameObjects == null || Selection.gameObjects.Length==0 || Selection.gameObjects[0].transform is RectTransform)
            {
                ContextMenu.AddCommonItems(Selection.gameObjects);
                ContextMenu.Show();
                is_handled = true;
            }
        }
        //else if (e.type == EventType.MouseMove)//show cur mouse pos
        //{
        //    Camera cam = sceneView.camera;
        //    Vector3 mouse_abs_pos = e.mousePosition;
        //    mouse_abs_pos.y = cam.pixelHeight - mouse_abs_pos.y;
        //    mouse_abs_pos = sceneView.camera.ScreenToWorldPoint(mouse_abs_pos);
        //    Debug.Log("mouse_abs_pos : " + mouse_abs_pos.ToString());
        //}
        if (e!=null && Event.current.type == EventType.KeyUp && e.control && e.keyCode==KeyCode.E)
            LayoutInfo.IsShowLayoutName = !LayoutInfo.IsShowLayoutName;
        if (is_handled)
            Event.current.Use();
    }

    
    static float GetSceneViewHeight(SceneView sceneView)
    {
        // Don't use sceneView.position.height, as it does not account for the space taken up by
        // toolbars.
        return sceneView.position.width / sceneView.camera.aspect;
    }

    static void SetSceneViewZoom(SceneView sceneView, float zoom)
    {
        float orthoHeight = GetSceneViewHeight(sceneView) / 2f / zoom;

        // We can't set camera.orthographicSize directly, because SceneView overrides it
        // every frame based on SceneView.size, so set SceneView.size instead.
        //
        // See SceneView.GetVerticalOrthoSize for the source of these sqrts.
        //sceneView.size = orthoHeight * Mathf.Sqrt(2f) * Mathf.Sqrt(sceneView.camera.aspect);
        float size = orthoHeight * Mathf.Sqrt(2f) * Mathf.Sqrt(sceneView.camera.aspect);
        sceneView.LookAt(Vector3.zero, sceneView.rotation, size);
    }
    

    
    //处理拖拽上来的prefab
    static bool HandleDragAsset(SceneView sceneView, Object handleObj)
    {
        Event e = Event.current;
        Camera cam = sceneView.camera;
        Vector3 mouse_abs_pos = e.mousePosition;
        
        // Retina 屏幕需要拉伸值
        float mult = EditorGUIUtility.pixelsPerPoint;
        
        // 转换成摄像机可接受的屏幕坐标，左下角是（0，0，0）右上角是（camera.pixelWidth，camera.pixelHeight，0)
        mouse_abs_pos.y = cam.pixelHeight - mouse_abs_pos.y * mult;
        mouse_abs_pos.x *= mult;
        mouse_abs_pos = sceneView.camera.ScreenToWorldPoint(mouse_abs_pos);
        
        //如果是图片
        if (handleObj.GetType() == typeof(Sprite) || handleObj.GetType() == typeof(Texture2D))
        {
            GameObject box = new GameObject("Image_1", typeof(Image));
            Undo.RegisterCreatedObjectUndo(box, "create image on drag pic");
            box.transform.position = mouse_abs_pos;
            Transform container_trans = UIEditorHelper.GetContainerUnderMouse(mouse_abs_pos, box);
            if (container_trans == null)
            {
                //没有容器的话就创建一个
                container_trans = NewLayoutAndEventSys(mouse_abs_pos);
            }
            box.transform.SetParent(container_trans);
            mouse_abs_pos.z = container_trans.position.z;
            box.transform.position = mouse_abs_pos;
            box.transform.localScale = Vector3.one;
            Selection.activeGameObject = box;
                
            //生成唯一的节点名字
            box.name = CommonHelper.GenerateUniqueName(container_trans.gameObject, handleObj.name);
            //赋上图片
            Image imageBoxCom = box.GetComponent<Image>();
            if (imageBoxCom != null)
            {
                imageBoxCom.raycastTarget = false;
                string assetPath = AssetDatabase.GetAssetPath(handleObj);
                UIEditorHelper.SetImageByPath(assetPath, imageBoxCom);
                return true;
            }
        }
        else
        {
            //prefab
            GameObject new_obj = GameObject.Instantiate(handleObj) as GameObject;
            if (new_obj != null)
            {
                Undo.RegisterCreatedObjectUndo(new_obj, "create obj on drag prefab");
                new_obj.transform.position = mouse_abs_pos;
                GameObject ignore_obj = new_obj;
               
                Transform container_trans = UIEditorHelper.GetContainerUnderMouse(mouse_abs_pos, ignore_obj);
                if (container_trans == null)
                {
                    container_trans = NewLayoutAndEventSys(mouse_abs_pos);
                }
                new_obj.transform.SetParent(container_trans);
                mouse_abs_pos.z = container_trans.position.z;
                new_obj.transform.position = mouse_abs_pos;
                new_obj.transform.localScale = Vector3.one;
                Selection.activeGameObject = new_obj;
                //生成唯一的节点名字
                new_obj.name = CommonHelper.GenerateUniqueName(container_trans.gameObject, handleObj.name);
                return true;
            }
        }
        return false;
    }

    private static Transform NewLayoutAndEventSys(Vector3 pos)
    {
        GameObject layout = UIEditorHelper.CreatNewLayout();
        pos.z = 0;
        layout.transform.position = pos;
        Vector3 last_pos = layout.transform.localPosition;
        last_pos.z = 0;
        layout.transform.localPosition = last_pos;
        return UIEditorHelper.GetRootLayout(layout.transform);
    }
    
    //判断是否为图片 
    static bool IsNeedHandleAsset(Object obj)
    {
        if (obj.GetType() == typeof(Sprite) || obj.GetType() == typeof(Texture2D))
            return true;
        else
        {
            GameObject gameObj = obj as GameObject;
            if (gameObj != null)
            {
                RectTransform uiBase = gameObj.GetComponent<RectTransform>();
                if (uiBase != null)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
    
}