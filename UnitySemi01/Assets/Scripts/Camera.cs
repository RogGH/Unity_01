using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public float cameraOffsetY = 80.0f;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        // Updateの後に行われる処理。
        // プレイヤーの移動の後に行われるので、カメラの移動が正しくなる
        Vector3 setVector = transform.position;

        // プレイヤーの座標を設定する（オフセット指定もする 
        setVector.x = player.transform.position.x;
        setVector.y = player.transform.position.y + cameraOffsetY;

        transform.position = setVector;
    }
}
