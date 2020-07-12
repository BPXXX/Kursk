using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cleanchair : MonoBehaviour
{
    public Text text;
    public player player001;
    public int sitid;

    // Start is called before the first frame update
    void Start()
    {
        text = GameObject.Find("UICanvas/GamePlayer/蓝方玩家1/name/Text").GetComponent<Text>();
        text.text = "空";
        text = GameObject.Find("UICanvas/GamePlayer/蓝方玩家2/name/Text").GetComponent<Text>();
        text.text = "空";
        text = GameObject.Find("UICanvas/GamePlayer/蓝方玩家3/name/Text").GetComponent<Text>();
        text.text = "空";
        text = GameObject.Find("UICanvas/GamePlayer/蓝方玩家4/name/Text").GetComponent<Text>();
        text.text = "空";
        text = GameObject.Find("UICanvas/GamePlayer/蓝方玩家5/name/Text").GetComponent<Text>();
        text.text = "空";
        text = GameObject.Find("UICanvas/GamePlayer/蓝方玩家6/name/Text").GetComponent<Text>();
        text.text = "空";
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void CleanChairname(int sit_chair)
    {
       sitid = player001.getsitid();
        if(sitid == 1)
        {   
            text = GameObject.Find("UICanvas/GamePlayer/蓝方玩家2/name/Text").GetComponent<Text>();
            text.text = "空";
        }
        else if (sitid == 2)
        {
            text = GameObject.Find("UICanvas/GamePlayer/蓝方玩家1/name/Text").GetComponent<Text>();
            text.text = "空";
        }


    }
}

