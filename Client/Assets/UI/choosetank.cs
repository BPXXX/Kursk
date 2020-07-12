using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class choosetank : MonoBehaviour
{
    // Start is called before the first frame update
    public player player001;
    public int tankid;
    void Start()
    {
        tankid = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void debug()
    //{
    //    Debug.Log("11111");
    //}
    public void ChooseTank1()
    {
        player001.setTankid(1);
        tankid = player001.getTankid();
        Debug.Log(player001.tankid);
    }
    public void ChooseTank2()
    {
        player001.setTankid(2);
        tankid = player001.getTankid();
        Debug.Log(player001.tankid);
    }
    public void ChooseTank3()
    {
        player001.setTankid(3);
        tankid = player001.getTankid();
        Debug.Log(player001.tankid);
    }
    public void ChooseTank4()
    {
        player001.setTankid(4);
        tankid = player001.getTankid();
        Debug.Log(player001.tankid);
    }
    public void ChooseTank5()
    {
        player001.setTankid(5);
        tankid = player001.getTankid();
        Debug.Log(player001.tankid);
    }
}

