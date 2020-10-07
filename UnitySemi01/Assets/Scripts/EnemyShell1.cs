using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShell1 : MonoBehaviour
{
    public int speed = 400;
    private Rigidbody2D rigid2D;
    public int liveSecond = 2;

    // Start is called before the first frame update
    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>(); // コンポーネント取得
        // 速度設定
        rigid2D.velocity = new Vector2(transform.localScale.x * speed, 0);
        //〇秒後に消滅
        Destroy(gameObject, liveSecond);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }

    public void setup(int dir, int rotate)
    {
        Vector2 temp = transform.localScale;
        temp.x = dir;
        transform.localScale = temp;
    }
}
