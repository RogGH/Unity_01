
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLShell1Hit : MonoBehaviour
{
	private HitBase hitBase;
	// Start is called before the first frame update
	void Start()
    {
		hitBase = GetComponentInParent<HitBase>();
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
			// ダメージ計算
			hitBase.damageCalc(
				GetComponent<AtkParam>(),
				col.gameObject.GetComponent<HitBase>());

			// ヒットエフェクト起動
			GameObject hit = (GameObject)Resources.Load("Prefabs/EffectHit");
			Vector2 pos = transform.position;
			pos.y += -5;
			Instantiate(hit, pos, transform.rotation);

			// ダメージ音
			SeManager.Instance.Play("damage1");
			// 敵に接触したら親毎消す
			Destroy(transform.root.gameObject);
        }
    }
}
