using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    public GameObject btn;
    public Text text;
    public int tankid;
    public int sitid;
    public bool groopid;
    public string playerName;
    //public int playerID;        //用来判断是房间里的第几个人
    // Start is called before the first frame update
    void Start()
    {
        tankid = 1;  //设置默认坦克为第一种
        sitid = 0; //默认不在座位上
        groopid = true;  //默认为蓝方阵营
        playerName = "游客玩家";
        text = GameObject.Find("UICanvas/player/playername/Text").GetComponent<Text>();
        text.text = playerName;
    }

    // Update is called once per frame
    void Update()
    {

    }
   public void setTankid(int a)  //设置坦克型号ID
    {
        tankid = a;
    }
    public int getTankid()  //调用坦克型号ID
    {
        return tankid;
    }
    public void setName( )  //设置名称
    {
       // playerName = "player_test"; //完整后可设置成get player的昵称
    }
    public string getName()  //调用名称
    {
        return playerName;
    }
    public int getsitid()  //调用坦克型号ID
    {
        return sitid;
    }
}


