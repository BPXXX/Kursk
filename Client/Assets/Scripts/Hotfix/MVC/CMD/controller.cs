using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using ProtoBuf;
using Game;
using Skyunion;

public class controller : MonoBehaviour
{
    //  private Rigidbody2D playerRig;
    public int id;
    public int mode = 1;
    public float speed = 5;
    public float speed_J = 1;
    public float speed_K = 1;
    public float movesize = 1.0f;
    public int startdirection = 3;
    //1上2下3左4右
    private bool isMove = false;
    private bool isTurn = false;
    private bool isFireJ = false;
    private bool isFireK = false;
    private int iswall = 0;
    private float process = 0;
    private float p = 0;
    private float q = 0;
    private Vector3 dest;
    public float time;
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
    public GameObject chuanjia_u;
    public GameObject chuanjia_d;
    public GameObject chuanjia_l;
    public GameObject chuanjia_r;
    public GameObject gaobao_u;
    public GameObject gaobao_d;
    public GameObject gaobao_l;
    public GameObject gaobao_r;
    public int hp = 50;
    public int type = 1;

    public int dir = 3;
    public int firej = 0;
    public int firek = 0;
    private bool alive = true;

    private Rect bloodBar;
    private float barUpLength = 50f;
    public int team = 0;

    void Start()
    {
        bloodBar = new Rect(0, 0, 60, 20);
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
        if (startdirection == 1)
        {
            turn1();
        }
        if (startdirection == 2)
        {
            turn2();
        }
        if (startdirection == 3)
        {
            turn3();
        }
        if (startdirection == 4)
        {
            turn4();
        }
        if (hp <= 0)
        {
            alive = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = die;
            Destroy(gameObject, 0.5f);
            hp = 100;
            

            //坦克重生  本来应该写在收到重生包的cmd里  但是时间来不及，就先写死了生成一个重生坦克
            Timer.Register(2.0f,()=>
            {
                GameObject m_playerRoot;
                GameObject m_tankRoot;
                m_playerRoot = GameObject.Find("/GameObject");
                //int count = TmpData.userInfoAll.Length;
                //for (int i = 0; i < count; i++)
                //    if ()
                CoreUtils.assetService.Instantiate($"tank", (GameObject) =>
                {
                    GameObject.transform.SetParent(m_playerRoot.transform);
                    m_tankRoot = GameObject;


                    m_tankRoot.GetComponent<controller>().id = 1;// xMsg.userinfo[index].id;
                    m_tankRoot.GetComponent<controller>().type = 1;// xMsg.userinfo[index].type;
                    m_tankRoot.GetComponent<controller>().hp = 100;//xMsg.userinfo[index].hp;
                    m_tankRoot.transform.position = new Vector3(10, -10, 0);

                    //string SUsername = System.Text.Encoding.Default.GetString(xMsg.userinfo[index].username);
                    //if (string.Equals(TmpData.userInfo.user_name, SUsername) == true)
                    //{
                    //    m_tankRoot.GetComponent<controller>().mode = 1;//本地坦克
                    //}
                    //else
                    //{
                    m_tankRoot.GetComponent<controller>().mode = 2;//mode =2    other坦克
                                                                   // }
                    
                });
                
            },null,false,false,null);
           

        }
        //自己
        if (mode == 1)
        {
            //移动与转向
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (startdirection == 3)
                {
                    if (iswall != 3)
                    { //移动

                        moveA();
                        //发包移动后的坐标
                        Debug.Log("移动位置");
                        Msg.Msg_VEC_C2S xMsg = new Msg.Msg_VEC_C2S();
                        xMsg.direction = startdirection;
                        xMsg.x = this.transform.position.x - movesize;
                        xMsg.y = this.transform.position.y;
                        xMsg.ingrass = 0;
                        MemoryStream stream = new MemoryStream();
                        Serializer.Serialize<Msg.Msg_VEC_C2S>(stream, xMsg);
                        AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.VEC_C2S, stream);

                    }
                }
                else
                {
                    isTurn = true;
                    Invoke("turn3", time);

                    Debug.Log("改变方向");
                    startdirection = 3;
                    Msg.Msg_VEC_C2S xMsg = new Msg.Msg_VEC_C2S();
                    xMsg.x = this.transform.position.x;
                    xMsg.y = this.transform.position.y;
                    xMsg.direction = 3;
                    xMsg.ingrass = 0;
                    MemoryStream stream = new MemoryStream();
                    Serializer.Serialize<Msg.Msg_VEC_C2S>(stream, xMsg);
                    AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.VEC_C2S, stream);
                }
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (startdirection == 4)
                {
                    if (iswall != 4)
                    {
                        //移动
                        moveD();
                        Debug.Log("移动位置");
                        Msg.Msg_VEC_C2S xMsg = new Msg.Msg_VEC_C2S();
                        xMsg.direction = startdirection;
                        xMsg.x = this.transform.position.x + movesize;
                        xMsg.y = this.transform.position.y;
                        xMsg.ingrass = 0;
                        MemoryStream stream = new MemoryStream();
                        Serializer.Serialize<Msg.Msg_VEC_C2S>(stream, xMsg);
                        AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.VEC_C2S, stream);
                    }
                }
                else
                {
                    isTurn = true;
                    Invoke("turn4", time);
                    Debug.Log("改变方向");
                    startdirection = 4;
                    Msg.Msg_VEC_C2S xMsg = new Msg.Msg_VEC_C2S();
                    xMsg.x = this.transform.position.x;
                    xMsg.y = this.transform.position.y;
                    xMsg.direction = 4;
                    xMsg.ingrass = 0;
                    MemoryStream stream = new MemoryStream();
                    Serializer.Serialize<Msg.Msg_VEC_C2S>(stream, xMsg);
                    AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.VEC_C2S, stream);
                }
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (startdirection == 1)
                {
                    if (iswall != 1)
                    {
                        //移动
                        moveW();
                        Debug.Log("移动位置");
                        Msg.Msg_VEC_C2S xMsg = new Msg.Msg_VEC_C2S();
                        xMsg.x = this.transform.position.x;
                        xMsg.y = this.transform.position.y + movesize;
                        xMsg.direction = startdirection;
                        xMsg.ingrass = 0;
                        MemoryStream stream = new MemoryStream();
                        Serializer.Serialize<Msg.Msg_VEC_C2S>(stream, xMsg);
                        AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.VEC_C2S, stream);
                    }
                }
                else
                {
                    isTurn = true;
                    Invoke("turn1", time);
                    Debug.Log("改变方向");
                    Msg.Msg_VEC_C2S xMsg = new Msg.Msg_VEC_C2S();
                    startdirection = 1;
                    xMsg.direction = 1;
                    xMsg.x = this.transform.position.x;
                    xMsg.y = this.transform.position.y;
                    xMsg.ingrass = 0;
                    MemoryStream stream = new MemoryStream();
                    Serializer.Serialize<Msg.Msg_VEC_C2S>(stream, xMsg);
                    AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.VEC_C2S, stream);
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (startdirection == 2)
                {
                    if (iswall != 2)
                    {
                        //移动
                        moveS();
                        Debug.Log("移动位置");
                        Msg.Msg_VEC_C2S xMsg = new Msg.Msg_VEC_C2S();
                        xMsg.direction = startdirection;
                        xMsg.x = this.transform.position.x;
                        xMsg.y = this.transform.position.y - movesize;
                        xMsg.ingrass = 0;
                        MemoryStream stream = new MemoryStream();
                        Serializer.Serialize<Msg.Msg_VEC_C2S>(stream, xMsg);
                        AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.VEC_C2S, stream);
                    }
                }
                else
                {
                    isTurn = true;
                    Invoke("turn2", time);
                    Debug.Log("改变方向");
                    startdirection = 2;
                    Msg.Msg_VEC_C2S xMsg = new Msg.Msg_VEC_C2S();
                    xMsg.direction = 2;
                    xMsg.x = this.transform.position.x;
                    xMsg.y = this.transform.position.y;
                    xMsg.ingrass = 0;
                    MemoryStream stream = new MemoryStream();
                    Serializer.Serialize<Msg.Msg_VEC_C2S>(stream, xMsg);
                    AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.VEC_C2S, stream);
                }
            }

            //射击
            if (Input.GetKeyDown(KeyCode.J))
            {
                fireJ();


            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                fireK();

                Debug.Log("发射K子弹");
                Msg.Msg_AMMO_C2S xMsg = new Msg.Msg_AMMO_C2S();
                xMsg.direction = startdirection;
                xMsg.x = this.transform.position.x;
                xMsg.y = this.transform.position.y;
                xMsg.type = 2;

                MemoryStream stream = new MemoryStream();
                Serializer.Serialize<Msg.Msg_AMMO_C2S>(stream, xMsg);
                AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.Ammo_C2S, stream);
            }
        }
        //其他坦克
        if (mode == 2)
        {
            if (alive)
            {
                //转向
                if (startdirection == 1)
                {
                    turn1();
                }
                if (startdirection == 2)
                {
                    turn2();
                }
                if (startdirection == 3)
                {
                    turn3();
                }
                if (startdirection == 4)
                {
                    turn4();
                }

                //发射
                if (firej == 1)
                {
                    FireJ(1);
                }
                if (firej == 2)
                {
                    FireJ(2);
                }
                if (firej == 3)
                {
                    FireJ(3);
                }
                if (firej == 4)
                {
                    FireJ(4);
                }
                if (firek == 1)
                {
                    FireK(1);
                }
                if (firek == 2)
                {
                    FireK(2);
                }
                if (firek == 3)
                {
                    FireK(3);
                }
                if (firek == 4)
                {
                    FireK(4);
                }
            }
        }
        if (isMove)
        {
            process += Time.deltaTime * speed;
            if (process < 1)
                transform.position = Vector3.Lerp(transform.position, dest, process);
            else
            {
                isMove = false;
            }
        }

        if (isFireJ)
        {
            p += Time.deltaTime * speed_J;
            if (p >= 2)
            {
                isFireJ = false;
            }
        }

        if (isFireK)
        {
            q += Time.deltaTime * speed_K;
            if (q >= 2)
            {
                isFireK = false;
            }
        }
    }

    //碰撞检测
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(mode == 1)
        {
            if (collision.transform.tag == "Wall" || collision.transform.tag == "Tank")
            {
                iswall = startdirection;

            }
            if (collision.transform.tag  == "zidan_chuanjia")
            {
                Debug.Log("被穿甲弹打中");
                Msg.Msg_DAMAGE_C2S xMsg = new Msg.Msg_DAMAGE_C2S();
                xMsg.damage = 20;
                xMsg.id_hurt = id;

                MemoryStream stream = new MemoryStream();
                Serializer.Serialize<Msg.Msg_DAMAGE_C2S>(stream, xMsg);
                AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.Damage_C2S, stream);
            }
            if (collision.transform.tag == "zidan_gaobao")
            {
                Debug.Log("被高爆弹打中");
                Msg.Msg_DAMAGE_C2S xMsg = new Msg.Msg_DAMAGE_C2S();
                xMsg.damage = 35;
                xMsg.id_hurt = id;

                MemoryStream stream = new MemoryStream();
                Serializer.Serialize<Msg.Msg_DAMAGE_C2S>(stream, xMsg);
                AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.Damage_C2S, stream);
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(mode == 1)
        {
            if (collision.transform.tag == "Wall" || collision.transform.tag == "Tank")
            {
                iswall = 0;
            }
        }
        
    }
    
    private void moveA()
    {
        if (!isMove && !isTurn)
        {
            dest = new Vector3(this.transform.position.x - movesize, this.transform.position.y, 0);
            isMove = true;
            process = 0;
        }
    }
    private void moveD()
    {
        if (!isMove && !isTurn)
        {
            dest = new Vector3(this.transform.position.x + movesize, this.transform.position.y, 0);
            isMove = true;
            process = 0;
        }
    }
    private void moveW()
    {
        if (!isMove && !isTurn)
        {
            dest = new Vector3(this.transform.position.x, this.transform.position.y + movesize, 0);
            isMove = true;
            process = 0;
        }
    }
    private void moveS()
    {
        if (!isMove && !isTurn)
        {
            dest = new Vector3(this.transform.position.x, this.transform.position.y - movesize, 0);
            isMove = true;
            process = 0;
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
        isTurn = false;
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
        isTurn = false;
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
        isTurn = false;
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
        isTurn = false;
    }
    private void fireJ()
    {
        if (!isFireJ)
        {
            if (startdirection == 1)
            {
                Instantiate(chuanjia_u, new Vector3(this.transform.position.x, this.transform.position.y + 0.8f, 0), chuanjia_u.transform.rotation);
                Debug.Log("发射J子弹");
                Msg.Msg_AMMO_C2S xMsg = new Msg.Msg_AMMO_C2S();
                xMsg.direction = startdirection;
                xMsg.x = this.transform.position.x;
                xMsg.y = this.transform.position.y + 0.8f;
                xMsg.type = 1;

                MemoryStream stream = new MemoryStream();
                Serializer.Serialize<Msg.Msg_AMMO_C2S>(stream, xMsg);
                AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.Ammo_C2S, stream);
            }
            if (startdirection == 2)
            {
                Instantiate(chuanjia_d, new Vector3(this.transform.position.x, this.transform.position.y - 0.8f, 0), chuanjia_d.transform.rotation);
                Debug.Log("发射J子弹");
                Msg.Msg_AMMO_C2S xMsg = new Msg.Msg_AMMO_C2S();
                xMsg.direction = startdirection;
                xMsg.x = this.transform.position.x;
                xMsg.y = this.transform.position.y - 0.8f;
                xMsg.type = 1;

                MemoryStream stream = new MemoryStream();
                Serializer.Serialize<Msg.Msg_AMMO_C2S>(stream, xMsg);
                AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.Ammo_C2S, stream);
            }
            if (startdirection == 3)
            {
                Instantiate(chuanjia_l, new Vector3(this.transform.position.x - 1.0f, this.transform.position.y + 0.1f, 0), chuanjia_l.transform.rotation);
                Debug.Log("发射J子弹");
                Msg.Msg_AMMO_C2S xMsg = new Msg.Msg_AMMO_C2S();
                xMsg.direction = startdirection;
                xMsg.x = this.transform.position.x - 1.0f;
                xMsg.y = this.transform.position.y + 0.1f;
                xMsg.type = 1;

                MemoryStream stream = new MemoryStream();
                Serializer.Serialize<Msg.Msg_AMMO_C2S>(stream, xMsg);
                AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.Ammo_C2S, stream);
            }
            if (startdirection == 4)
            {
                Instantiate(chuanjia_r, new Vector3(this.transform.position.x + 1.0f, this.transform.position.y + 0.1f, 0), chuanjia_r.transform.rotation);
                Debug.Log("发射J子弹");
                Msg.Msg_AMMO_C2S xMsg = new Msg.Msg_AMMO_C2S();
                xMsg.direction = startdirection;
                xMsg.x = this.transform.position.x + 1.0f;
                xMsg.y = this.transform.position.y + 0.1f;
                xMsg.type = 1;

                MemoryStream stream = new MemoryStream();
                Serializer.Serialize<Msg.Msg_AMMO_C2S>(stream, xMsg);
                AppFacade.GetInstance().SendPBMsg((int)Msg.MsgType.Ammo_C2S, stream);
            }
            isFireJ = true;
            p = 0;
        }

    }
    private void fireK()
    {
        if (!isFireK)
        {
            if (startdirection == 1)
            {
                Instantiate(gaobao_u, new Vector3(this.transform.position.x, this.transform.position.y + 0.8f, 0), chuanjia_u.transform.rotation);
            }
            if (startdirection == 2)
            {
                Instantiate(gaobao_d, new Vector3(this.transform.position.x, this.transform.position.y - 0.8f, 0), chuanjia_d.transform.rotation);
            }
            if (startdirection == 3)
            {
                Instantiate(gaobao_l, new Vector3(this.transform.position.x - 1.0f, this.transform.position.y + 0.1f, 0), chuanjia_l.transform.rotation);
            }
            if (startdirection == 4)
            {
                Instantiate(gaobao_r, new Vector3(this.transform.position.x + 1.0f, this.transform.position.y + 0.1f, 0), chuanjia_r.transform.rotation);
            }
            isFireK = true;
            q = 0;
        }
    }
    private void FireJ(int d)
    {
        if (!isFireJ)
        {
            if (d == 1)
            {
                Instantiate(chuanjia_u, new Vector3(this.transform.position.x, this.transform.position.y + 0.8f, 0), chuanjia_u.transform.rotation);
            }
            if (d == 2)
            {
                Instantiate(chuanjia_d, new Vector3(this.transform.position.x, this.transform.position.y - 0.8f, 0), chuanjia_d.transform.rotation);
            }
            if (d == 3)
            {
                Instantiate(chuanjia_l, new Vector3(this.transform.position.x - 1.0f, this.transform.position.y + 0.1f, 0), chuanjia_l.transform.rotation);
            }
            if (d == 4)
            {
                Instantiate(chuanjia_r, new Vector3(this.transform.position.x + 1.0f, this.transform.position.y + 0.1f, 0), chuanjia_r.transform.rotation);
            }
            isFireJ = true;
            p = 0;
            firej = 0;
        }
    }
    private void FireK(int d)
    {
        if (!isFireK)
        {
            if (d == 1)
            {
                Instantiate(gaobao_u, new Vector3(this.transform.position.x, this.transform.position.y + 0.8f, 0), chuanjia_u.transform.rotation);
            }
            if (d == 2)
            {
                Instantiate(gaobao_d, new Vector3(this.transform.position.x, this.transform.position.y - 0.8f, 0), chuanjia_d.transform.rotation);
            }
            if (d == 3)
            {
                Instantiate(gaobao_l, new Vector3(this.transform.position.x - 1.0f, this.transform.position.y + 0.1f, 0), chuanjia_l.transform.rotation);
            }
            if (d == 4)
            {
                Instantiate(gaobao_r, new Vector3(this.transform.position.x + 1.0f, this.transform.position.y + 0.1f, 0), chuanjia_r.transform.rotation);
            }
            isFireK = true;
            q = 0;
            firek = 0;
        }
    }

    void OnGUI()
    {
        Vector2 player2DPosition = Camera.main.WorldToScreenPoint(transform.position);
        player2DPosition.y = Screen.height - player2DPosition.y - barUpLength;
        bloodBar.center = player2DPosition + new Vector2(0, 0);
        if (player2DPosition.x > Screen.width || player2DPosition.y > Screen.height
            || player2DPosition.x < 0 || player2DPosition.y < 0)
        {

        }
        else
        {
            if (team == 0)
            {
                GUI.color = Color.green;
                GUI.HorizontalScrollbar(bloodBar, 0.0f, 1.0f * hp / 50, 0.0f, 2.0f);
            }
            if (team == 1)
            {
                GUI.color = Color.red;
                GUI.HorizontalScrollbar(bloodBar, 0.0f, 1.0f * hp / 50, 0.0f, 2.0f);
            }
        }
    }
}

