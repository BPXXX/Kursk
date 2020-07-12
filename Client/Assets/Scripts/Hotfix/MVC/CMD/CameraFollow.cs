using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Player;
    private Vector3 Pos; void LateUpdate()
    {
        GameObject[] tank;
        tank = GameObject.FindGameObjectsWithTag("Tank");
        for(int i=0;i<tank.Length;i++)
        {
            if (tank[i].GetComponent<controller>().mode == 1)
            {
                Player = tank[i].transform;
            }
                     
        }
         Pos = Player.transform.position - gameObject.transform.position; Pos.z = 0;
          gameObject.transform.position += Pos / 20;
        
    }

}