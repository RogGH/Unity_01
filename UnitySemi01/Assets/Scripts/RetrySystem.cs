using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	現状のリトライポイントの仕様について

	retryPointのprefabをリトライしたい座標に設置する。
	スタート座標をリトライ番号０として、
	そのあとは番号を１ずつ足していく形でリトライ番号を設定する。

	最初が０として、次にその数値より大きいリトライポイントに触れると、
	その座標が保存されて次のリトライポイントの座標となる。
	（同じ場合は最初に触れた場所の座標が保存されていると思われる）

	プレイヤーをステージ開始時などにリトライポイントの座標に設定すれば使えます。

	※デバッグ機能
	リトライポイントにDebugForceSetのような設定があるので、
	それをすれば必ずそこをリトライポイントとして設定します。

 */

public class RetrySystem : MonoBehaviour
{
	private static RetrySystem mInstance;
	private int retryNo = 0;
	private Vector2 retryPos;

	public static RetrySystem Instance {
		get
		{
			if (mInstance == null)
			{
				GameObject obj = new GameObject("RetrySystem");
				mInstance = obj.AddComponent<RetrySystem>();
			}
			return mInstance;
		}
	}

	// アクセサ
	public int getRetryNo() { return retryNo; }
	public void setRetryNo(int No) { retryNo = No; }
	public void clearRetryNo() { retryNo = 0; }
	public Vector3 getRetryPos() { return retryPos; }
	public void setRetryPos(Vector3 setPos) { retryPos = setPos; }

	// リトライポイント更新
	public void updateRetryPoint(int num, Vector3 pos)
	{
		setRetryNo(num);
		setRetryPos(pos);
	}

	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
