using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class zidan_u : MonoBehaviour
{
    public float move;
    public float speed;
    public float time;
    public int hit = 5;
    public int hurt_id;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Destroy(gameObject, time);
    }
    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, gameObject.transform.localPosition + new Vector3(0, move, 0), step);
    }
    //碰撞检测
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall" || collision.transform.tag == "Tank")
        {
            GameObject.Destroy(gameObject);
        }
    }
}
