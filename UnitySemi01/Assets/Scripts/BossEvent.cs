using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEvent : MonoBehaviour
{
    public Collider2D defaultCamera;
    public Collider2D bossCamera;
    public GameObject boss;

    int battleStat = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // 戦闘中
        if (battleStat == 1)
        {
			if (boss.GetComponent<Boss1>().chkDie())
			{
                GameObject cam1 = GameObject.Find("CMvcam1");
				CinemachineVirtualCamera vCam = cam1.GetComponent<CinemachineVirtualCamera>();
				vCam.Priority = 10;	// 優先度を戻す

				BgmManager.Instance.Play("bgm_Stage1");
			}
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // クリアラインに入ったら
        if (collision.tag == "Player")
        {
            // カメラのコンポーネントを変更したい
            if (battleStat == 0)
            {
                GameObject cam1 = GameObject.Find("CMvcam1");
				CinemachineVirtualCamera vCam = cam1.GetComponent<CinemachineVirtualCamera>();
				vCam.Priority = 0;	// 優先度を最低に


				// ボスを起動してみる
				boss.SetActive(true);

				// ＢＧＭ変更
				BgmManager.Instance.Stop();

                ++battleStat;
            }
        }
    }
}
