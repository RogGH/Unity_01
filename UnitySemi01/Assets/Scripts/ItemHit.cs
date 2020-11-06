﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHit : MonoBehaviour
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
        // 接触したタイミング
        // 接触した相手のタグがPlayerかチェック
        if (col.tag == "Player")
        {
			// ダメージ計算
			int result = hitBase.damageCalc(
				GetComponent<AtkParam>(),
				col.gameObject.GetComponent<HitBase>());

			// プレイヤーにリアクションをさせる
			GameObject player = GameObject.Find("Player");
			player.GetComponent<Player>().setPLReaction(result);

			// 消す
			Destroy(transform.root.gameObject);
		}
	}
}
