using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ready : MonoBehaviour
{

    public void toready()
    {
        gameObject.SetActive(false);  //设置隐藏
        Debug.Log("已准备");
    }
    public void toprepare()
    {
        gameObject.SetActive(true); //设置显示
        Debug.Log("已取消准备");
    }
}
