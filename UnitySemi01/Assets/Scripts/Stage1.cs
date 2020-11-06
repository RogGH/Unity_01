using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage1 : MonoBehaviour
{
	[SerializeField] private bool retryEnabled = true;

	public LayerMask groundLayer;       // 地面チェック用のレイヤー
	public GameObject player;

	RetrySystem retrySystem;

	private int Stage1Stat = 0;
	private Text readyText;
	private float ctr0;

	private Vector2[] retryPosTbl = {
		new Vector2(-20,-66),
		new Vector2(2280,-66),
	};

	// Start is called before the first frame update
	void Start()
	{
		readyText = GameObject.Find("Ready").GetComponent<Text>();
		retrySystem = RetrySystem.Instance;

		// リトライポイントを有効にする
		if (retryEnabled)
		{
			// 足元の座標を取得
			Vector3 retryPos = retrySystem.getRetryPos();
			float dist = 100;
			RaycastHit2D hit = Physics2D.Linecast(
				retryPos,
				retryPos + -transform.up * dist, 
				groundLayer);
			if (hit != null) {
				retryPos.y += -hit.distance;
			}			
			// PLの足元座標に設定
			player.GetComponent<Player>().setFootPos(retryPos);
		}

		// ＢＧＭ再生
		BgmManager.Instance.Play("bgm_Stage1");
	}

	// Update is called once per frame
	void Update()
	{
		if (FadeManager.Instance.checkFading())
		{
			return;
		}

		switch (Stage1Stat)
		{
			case 0:
				// READY表示
				ctr0 = 2.0f;
				++Stage1Stat;
				break;

			case 1:
				ctr0 -= Time.deltaTime;
				if (ctr0 <= 0)
				{
					// テキストを非アクティブに
					readyText.gameObject.SetActive(false);
					++Stage1Stat;
				}
				break;

			case 2:
				if (player.activeSelf == false)
				{
					GameObject light = (GameObject)Resources.Load("Prefabs/EffectLight");
					Instantiate(light, player.transform.position, player.transform.rotation);
					player.SetActive(true);
					// ＳＥ再生
					SeManager.Instance.Play("magic-worp1");
					//
					++Stage1Stat;
				}
				break;

			case 3:
				break;
		}
	}
}