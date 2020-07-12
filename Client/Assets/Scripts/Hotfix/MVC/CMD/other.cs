using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class other : MonoBehaviour
{
    //  private Rigidbody2D playerRig;
    public int id = 0;
    public int dir = 3;
    public Sprite left;
    public Sprite right;
    public Sprite up;
    public Sprite down;
    public Sprite left2;
    public Sprite right2;
    public Sprite up2;
    public Sprite down2;
    public Sprite left3;
    public Sprite right3;
    public Sprite up3;
    public Sprite down3;
    public Sprite left4;
    public Sprite right4;
    public Sprite up4;
    public Sprite down4;
    public Sprite left5;
    public Sprite right5;
    public Sprite up5;
    public Sprite down5;
    public Sprite die;
    public int hp = 50;
    public int type = 1;
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (type == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = left;
        }
        if (type == 2)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = left2;
        }
        if (type == 3)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = left3;
        }
        if (type == 4)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = left4;
        }
        if (type == 5)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = left5;
        }
        if (hp <= 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = die;
            Destroy(gameObject,0.5f);
        }

        if(dir == 1)
        {
	turn1();
        }
        if(dir == 2)
        {
	turn2();
        }
        if(dir == 3)
        {
	turn3();
        }
        if(dir == 4)
        {
	turn4();
        }
  }
   private void turn1()
    {
        if (type == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = up;
        }
        if (type == 2)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = up2;
        }
        if (type == 3)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = up3;
        }
        if (type == 4)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = up4;
        }
        if (type == 5)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = up5;
        }
    }
    private void turn2()
    {
        if (type == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = down;
        }
        if (type == 2)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = down2;
        }
        if (type == 3)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = down3;
        }
        if (type == 4)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = down4;
        }
        if (type == 5)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = down5;
        }
    } 
    private void turn3()
    {
        if (type == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = left;
        }
        if (type == 2)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = left2;
        }
        if (type == 3)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = left3;
        }
        if (type == 4)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = left4;
        }
        if (type == 5)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = left5;
        }
    }
    private void turn4()
    {
        if (type == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = right;
        }
        if (type == 2)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = right2;
        }
        if (type == 3)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = right3;
        }
        if (type == 4)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = right4;
        }
        if (type == 5)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = right5;
        }
    }
}
