using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
	RetrySystem retrySystem;

    // Start is called before the first frame update
    void Start()
    {
		retrySystem = RetrySystem.Instance;
	}

    // Update is called once per frame
    void Update()
    {
		if (!FadeManager.Instance.checkFading())
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				retrySystem.clearRetryNo();
				FadeManager.Instance.LoadScene("Stage1", 0.5f);
			}
		}
    }
}
