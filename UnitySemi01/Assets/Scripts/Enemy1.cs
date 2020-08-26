﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public int speed = 80;

    public LayerMask groundLayer;       // 地面チェック用のレイヤー
    public GameObject bomb1;

    private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxColi2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxColi2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // 直線移動して、壁にぶつかったら反転する雑魚
        rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);

        // 壁にぶつかったら速度反転
        // レイを飛ばす始点、方向、長さを設定する
        Vector3 chkPos = transform.position;
        float length = boxColi2D.size.x / 2;
        float offsetY = boxColi2D.offset.y * transform.localScale.y;

        chkPos.y += offsetY;

        // レイを飛ばして地面レイヤーに接触しているかチェック
        RaycastHit2D result = 
            Physics2D.Raycast(
                new Vector2(chkPos.x, chkPos.y),
                new Vector2(speed < 0 ? -1 : 1, 0),
                length,
                groundLayer);

        // レイを可視化してみる（デバッグ表示
        Vector3 endPos = chkPos;
        endPos.x += length * (speed < 0 ? -1 : 1);
        Debug.DrawLine(chkPos,endPos, Color.red);

        // 接触結果があるのかをチェック
        if ( result.collider != null )
        {
            speed *= -1;            // 接触しているので速度を反転
        }

        // 反転処理（ｘのスケール値がー１になると反転するので、
        // 入力方向が左（－１）の時は右の絵が反転して表示される
        Vector2 temp = transform.localScale;
        temp.x = (speed < 0 ? 1 : -1);
        transform.localScale = temp;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // 接触したタイミング
        // 接触した相手のタグがPLShellかチェック
        if (col.tag == "PLShell")
        {
            // 爆発エフェクトを出す
            Vector3 bootpos = transform.position;
            float offsetY = boxColi2D.offset.y;
            Instantiate(bomb1, 
                new Vector3(transform.position.x, 
                transform.position.y + (offsetY * transform.localScale.y),
                transform.position.z),
                transform.rotation);

            // オブジェクトを消す
            Destroy(gameObject);
        }
    }
}