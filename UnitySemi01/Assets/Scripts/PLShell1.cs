using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLShell1 : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rigid2D;
    public int speed = 400;
    public int liveSecond = 3;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");        // プレイヤーを取得
        rigid2D = GetComponent<Rigidbody2D>(); // コンポーネント取得
        // PL向きを取得
        float plDir = player.transform.localScale.x;
        // 速度設定
        rigid2D.velocity = new Vector2(plDir * speed, 0);
        //画像の向きをユニティちゃんに合わせる
        transform.localScale = new Vector2(plDir, transform.localScale.y);
        //〇秒後に消滅
        Destroy(gameObject, liveSecond);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 )
        {
            Destroy(gameObject);
        }
    }
}
