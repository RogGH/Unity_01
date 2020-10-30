using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHit : MonoBehaviour
{
    public int RecoveryValue = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // 接触したタイミング
        // 接触した相手のタグがPlayerかチェック
        if (col.tag == "Player")
        {
            // プレイヤーを取得して、HPを減らす
            GameObject player = GameObject.Find("Player");
            player.GetComponent<Player>().addPLHitPoint(RecoveryValue);
			// 消す
			GameObject.Destroy(this.gameObject);
        }
    }
}
