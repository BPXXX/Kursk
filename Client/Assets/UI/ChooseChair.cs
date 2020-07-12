using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseChair : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject btn;
    public player player001;
    public string Name;
    public Text text;
    public bool sit_chair; //判断座位上是否有人
    void Start()
    {
        Name = "test";
        sit_chair = false;//默认座位上没人
    }

    // Update is called once per frame
    void Update()
    {
                   
    }
   
    public int getsitid()
    {
        int i;
        i = player001.sitid;
        return i;
    }
    public void changechairb1()
    {
        //return player001.sitid;
        player001.sitid = 1;
        player001.groopid = true; //阵营变换为蓝方
        sit_chair = true;
        text = GameObject.Find("UICanvas/GamePlayer/蓝方玩家1/name/Text").GetComponent<Text>();
        name = player001.getName();
        text.text = name;
        
        
    }
    public void changechairb2()
    {
        //return player001.sitid;
        player001.sitid = 2;
        player001.groopid = true; //阵营变换为蓝方
        sit_chair = true;
        text = GameObject.Find("UICanvas/GamePlayer/蓝方玩家2/name/Text").GetComponent<Text>();
        name = player001.getName();
        text.text = name;
    }
    public void changechairb3()
    {
        player001.groopid = true; //阵营变换为蓝方
        sit_chair = true;
        text = GameObject.Find("UICanvas/GamePlayer/蓝方玩家3/name/Text").GetComponent<Text>();
        name = player001.getName();
        text.text = name;
    }
    public void changechairb4()
    {
        player001.groopid = true; //阵营变换为蓝方
        sit_chair = true;
        text = GameObject.Find("UICanvas/GamePlayer/蓝方玩家4/name/Text").GetComponent<Text>();
        name = player001.getName();
        text.text = name;
    }
    public void changechairb5()
    {
        player001.groopid = true; //阵营变换为蓝方
        sit_chair = true;
        text = GameObject.Find("UICanvas/GamePlayer/蓝方玩家5/name/Text").GetComponent<Text>();
        name = player001.getName();
        text.text = name;
    }
    public void changechairb6()
    {
        player001.groopid = true; //阵营变换为蓝方
        sit_chair = true;
        text = GameObject.Find("UICanvas/GamePlayer/蓝方玩家6/name/Text").GetComponent<Text>();
        name = player001.getName();
        text.text = name;
    }

    public void changechairr1()
    {
        player001.groopid = false; //阵营变换为红方
        text = GameObject.Find("UICanvas/GamePlayer/红方玩家1/name/Text").GetComponent<Text>();
        name = player001.getName();
        text.text = name;
    }
    public void changechairr2()
    {
        player001.groopid = false; //阵营变换为红方
        text = GameObject.Find("UICanvas/GamePlayer/红方玩家2/name/Text").GetComponent<Text>();
        name = player001.getName();
        text.text = name;
    }
    public void changechairr3()
    {
        player001.groopid = false; //阵营变换为红方
        text = GameObject.Find("UICanvas/GamePlayer/红方玩家3/name/Text").GetComponent<Text>();
        name = player001.getName();
        text.text = name;
    }
    public void changechairr4()
    {
        player001.groopid = false; //阵营变换为红方
        text = GameObject.Find("UICanvas/GamePlayer/红方玩家4/name/Text").GetComponent<Text>();
        name = player001.getName();
        text.text = name;
    }
    public void changechairr5()
    {
        player001.groopid = false; //阵营变换为红方
        text = GameObject.Find("UICanvas/GamePlayer/红方玩家5/name/Text").GetComponent<Text>();
        name = player001.getName();
        text.text = name;
    }
    public void changechairr6()
    {
        player001.groopid = false; //阵营变换为红方
        text = GameObject.Find("UICanvas/GamePlayer/红方玩家6/name/Text").GetComponent<Text>();
        name = player001.getName();
        text.text = name;
    }
}
