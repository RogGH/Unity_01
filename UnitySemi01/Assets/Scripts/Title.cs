using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
	public AudioClip decideSE;            // ジャンプの発射SE

	private SoundManager sndMng;

	// Start is called before the first frame update
	void Start()
    {
		sndMng = SoundManager.Instance;

	}

    // Update is called once per frame
    void Update()
    {
		if (!FadeManager.Instance.checkFading())
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				// オーディオを再生
				AudioSource.PlayClipAtPoint(decideSE, transform.position);
				// ＢＧＭフェードアウト
				FadeManager.Instance.LoadScene("Select", 1.0f);
			}
		}
	}
}
