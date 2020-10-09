using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShell1 : MonoBehaviour
{
    [SerializeField] int speed = 400;
    [SerializeField] int liveSecond = 2;

	private Rigidbody2D rigid2D;
	private int rotDeg;

	// Start is called before the first frame update
	void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>(); // コンポーネント取得
											   // 速度設定
		Vector2 spd = new Vector2(speed * Mathf.Cos(Mathf.Deg2Rad * rotDeg),
		speed * Mathf.Sin(Mathf.Deg2Rad * rotDeg));
        rigid2D.velocity = new Vector2(transform.localScale.x * Mathf.Abs(spd.x), spd.y);
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

		rotDeg = rotate;
	}
}
