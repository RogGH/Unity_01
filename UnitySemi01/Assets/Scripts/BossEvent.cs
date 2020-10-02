using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEvent : MonoBehaviour
{
    public Collider2D bossCamera;
    int battleStat = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
                CinemachineConfiner conf = cam1.GetComponent<CinemachineConfiner>();
                conf.m_BoundingShape2D = bossCamera;

                ++battleStat;
            }
        }
    }



}
