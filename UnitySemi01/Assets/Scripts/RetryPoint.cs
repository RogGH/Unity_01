using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RetryPoint : MonoBehaviour
{
	[SerializeField] int retryPointNum = 0;
	[SerializeField] bool debugForceSet = false;

	RetrySystem retrySys;


	private void Awake()
	{
		retrySys = RetrySystem.Instance;

		// 強制設定フラグ
		if (debugForceSet)
		{
			// リトライポイントを更新する
			changeRetryPoint();
		}
	}
	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		// 可視化
		Debug.DrawLine(transform.position + transform.up,
			transform.position + -transform.up * 100,
		    Color.red);
	}

	// 接触したときに更新
	void OnTriggerEnter2D(Collider2D collision)
	{
		// プレイヤーに接触時
		if (collision.tag == "Player")
		{
			// システムに登録されているポイントが自分より下だったら
			if (retrySys.getRetryNo() < retryPointNum)
			{
				// リトライポイントを更新する
				changeRetryPoint();
			}
		}
	}


	// リトライポイント更新
	void changeRetryPoint()
	{
		retrySys.updateRetryPoint(
			retryPointNum,
			transform.position);
	}
}
