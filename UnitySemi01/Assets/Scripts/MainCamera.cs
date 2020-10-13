using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCamera : MonoBehaviour
{
	[SerializeField] private bool retryEnabled = true;
	[SerializeField] private int debugRetryNo = 1;

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

		if (debugRetryNo != 0)
		{
			retrySystem.setRetryNo(debugRetryNo);
		}

		if (retryEnabled)
		{
			player.transform.position = retryPosTbl[retrySystem.getRetryNo()];
		}
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
					//
					++Stage1Stat;
				}
				break;

			case 3:
				// リトライポイント更新チェック
				if (retrySystem.getRetryNo() < retryPosTbl.Length - 1 )
				{
					if (player.transform.position.x > retryPosTbl[retrySystem.getRetryNo() + 1].x)
					{
						retrySystem.incRetryNo();
					}
				}
				break;
		}
	}
}