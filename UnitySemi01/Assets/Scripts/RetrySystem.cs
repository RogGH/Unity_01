using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrySystem : MonoBehaviour
{
	private static RetrySystem mInstance;

	private int retryNo = 0;


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
		set
		{
		}
	}

	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	public int getRetryNo()
	{
		return retryNo;
	}
	public void setRetryNo(int No)
	{
		retryNo = No;
	}
	public void incRetryNo()
	{
		++retryNo;
	}
	public void clearRetryNo()
	{
		retryNo = 0;
	}
}
