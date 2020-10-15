using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLShell1Hit : MonoBehaviour
{
    public int AtkPower = 5;

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
        if (col.gameObject.tag == "Enemy"
        || col.gameObject.tag == "Boss" )
        {
            // ボスタグに接触していた場合、体力を削る
            if (col.gameObject.tag == "Boss")
            {
                 // ボスを取得して、HPを減らす
                GameObject boss = col.gameObject;
                boss.GetComponent<Boss>().AddHitPoint(-AtkPower);
            }

			GameObject hit = (GameObject)Resources.Load("Prefabs/EffectHit");
			Vector2 pos = transform.position;
			pos.y += -5;
			Instantiate(hit, pos, transform.rotation);

			// 敵に接触したら親毎消す
			Destroy(transform.root.gameObject);
        }
    }
}
