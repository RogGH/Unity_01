using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		// BGM停止
		BgmManager.Instance.Play("bgm_Clear");
	}

	// Update is called once per frame
	void Update()
    {
		if (!FadeManager.Instance.checkFading())
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				// オーディオを再生
				SeManager.Instance.Play("decision18");
				// ＢＧＭフェードアウト
				BgmManager.Instance.Stop();

				FadeManager.Instance.LoadScene("Title", 1);
			}
		}
    }
}
