﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    // public変数（Unityのインスペクタの値が優先される
    public float dashSpeedX = 120.0f;   // 歩くスピード
    public float jumpPower = 400.0f;  // ジャンプ力
    public LayerMask groundLayer;       // 地面チェック用のレイヤー
    public GameObject shell1;           // 通常弾

    // private変数(クラス外からアクセス不可能）
    // [SerializeField]をつけるとインスペクタで操作可能になる
    private Rigidbody2D rigidbody2D;    // コンポーネント用変数
    private BoxCollider2D boxcollider2D;// コンポーネント用変数
    private Animator animator;          // コンポーネント用変数 

    private string nowAnimName = null;  // 現在のアニメーションの名前
    private string oldAnimName = null;  // ひとつ前のアニメーションの名前
    private float nowAnimCtr = 0;       // アニメーションの再生場所を保存しておく

    // プライベート変数
    private bool isGrounded;            // 地上にいるかの判定
    private float inputLR = 0;          // 左右入力
    private bool inputJump = false;     // ジャンプ入力
    private float shotWait = 0;         // 発射間隔用
 
    // const変数
    const float JUMPUP_CHECK_SPEED = 1.0f;   // ジャンプ上昇中チェック用
    const float SHOT_WAIT_DEFAULT = 0.2f;    // 発射アニメーション終了時間 
    Vector3 SHOT_GROUND_OFS = new Vector3(30.0f, 36.0f, 0 );    // 地上での発射オフセット
    Vector3 SHOT_JUMP_OFS = new Vector3(30.0f, 42.0f, 0);       // 空中での発射オフセット

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントを取得
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxcollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        // 前処理
        preSetup();

        // ジャンプ制御
        jumpControl();

        // 歩き制御
        moveControl();

        // ショット制御
        shotControl();

        // 後処理
        postSetup();
    }

    private void LateUpdate()
    {
        // Updateの後で行われる処理
    }


    private void FixedUpdate()
    {
        // fixedupdate内部でrididbody関連の処理を行うのが正しい
        // 物理演算はfixedupdateの周期で行われるため、移動処理の精度が上がる

        // 接地状態を更新
        isGrounded = checkGrounded();

        // ジャンプ入力でジャンプさせる
        if (inputJump)
        {
            rigidbody2D.velocity.Set(rigidbody2D.velocity.x, 0);    // Ｙ軸速度を０に
            rigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            inputJump = false;
        }

        // 左右入力をチェック
        if (inputLR != 0)
        {
            // 入力方向へ移動
            rigidbody2D.velocity = new Vector2(inputLR * dashSpeedX, rigidbody2D.velocity.y);
        }
        else
        {
            // 入力がないので停止
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        }
    }

    // 接地チェック
    bool checkGrounded()
    {
        // 上昇中は何もしない
        if ( rigidbody2D.velocity.y > JUMPUP_CHECK_SPEED)
        {
            return false;
        }

        Vector3 chkPos = transform.position;
        float boxHarfWitdh = boxcollider2D.size.x / 2;
        bool result = false;
        Vector3 lineLength = transform.up * 0.05f;

        // ３点チェック（とりあえず）
        chkPos.x = transform.position.x - boxHarfWitdh;
        for (int loopNo = 0; loopNo < 3; ++loopNo )
        {
            // 自身の位置から↓に向かって線を引いて、地面に接触したかをチェックする
            result |= Physics2D.Linecast(
                chkPos + transform.up,
                chkPos - lineLength,
                groundLayer
                );
            // レイを表示してみる
            //Debug.DrawLine(chkPos + transform.up,
            //    chkPos - lineLength,
            //    Color.red);

            // オフセット加算
            chkPos.x += boxHarfWitdh;
        }
        return result;
    }

    // 前処理
    void preSetup()
    {
        // アニメーションの名前を取得
        AnimatorClipInfo[]　currentClipInfo = this.animator.GetCurrentAnimatorClipInfo(0);
        string clipName = currentClipInfo[0].clip.name;
        // アニメーション変更があったとき
        if (clipName != nowAnimName)
        {
            // 古いアニメーションを保存
            oldAnimName = nowAnimName;
            // 現在のアニメーションの名前を保存
            nowAnimName = clipName;
            // 発射アニメ制御
            shotAnimControl();
        }

        // 接地チェック
        if (isGrounded)
        {
            // 地上
            rigidbody2D.velocity.Set(rigidbody2D.velocity.x, 0);    // Ｙ軸速度を０に
            animator.SetBool("JumpUp", false);                    // ジャンプ上昇をOFF
            animator.SetBool("JumpDown", false);                  // ジャンプ下降をOFF 
        }
        else
        {
            // 空中
            // Ｙ軸方向の速度で、上昇中か下降中かを設定する(０の時は下降設定)
            bool isJumpUp = rigidbody2D.velocity.y > JUMPUP_CHECK_SPEED ? true : false;
            bool isJumpDown = rigidbody2D.velocity.y <= 0f ? true : false;
            // アニメーション設定
            animator.SetBool("JumpUp", isJumpUp);
            animator.SetBool("JumpDown", isJumpDown);
        }
    }

    // 後処理
    void postSetup()
    {
        // 後処理
        nowAnimCtr = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    // 発射アニメーション制御
    void shotAnimControl()
    {
        // アニメーションの名前を取得
        // アニメーション変更があったとき
        // 前のアニメーションが歩きで現在が歩き発射の場合
        if ((oldAnimName == "PLRun" && nowAnimName == "PLRunShot")
        || (oldAnimName == "PLRunShot" && nowAnimName == "PLRun")
            )
        {
            // アニメーターの再生位置を継続させてみる
            animator.Play(nowAnimName, -1, nowAnimCtr);         // 引継ぎ
        }
    }

    // ジャンプ制御
    void jumpControl()
    {
        //着地していた時
        if (isGrounded)
        {
            // スペースキー入力をチェック
            if (Input.GetKeyDown("space"))
            {
                // ジャンプ入力設定
                inputJump = true;
                // 着地判定をfalse
                isGrounded = false;
                // ジャンプ上昇へ
                animator.SetBool("JumpUp", true);
                animator.SetBool("JumpDown", false);
            }
        }
    }

    // 移動制御
    void moveControl()
    {
        // 入力を取得（WASDor十字キー）
        float inputX = Input.GetAxisRaw("Horizontal");

        // 左右入力があったら
        if (inputX != 0)
        {
            inputLR = inputX;

            // 反転処理（ｘのスケール値がー１になると反転するので、
            // 入力方向が左（－１）の時は右の絵が反転して表示される
            Vector2 temp = transform.localScale;
            temp.x = inputX;
            transform.localScale = temp;

            if (isGrounded)
            {
                // 入力があるときは走り状態へ
                animator.SetBool("Run", true);
            }
        }
        else
        {
            inputLR = 0;
            // 入力がないときは走り状態解除
            animator.SetBool("Run", false);
        }
    }

    // 発射制御
    void shotControl()
    {
        // 発射待機をチェック
        if (shotWait > 0)
        {
            // 発射待機時間を減算する
            shotWait -= Time.deltaTime;
            if (shotWait <= 0)
            {
                // 発射フラグをOFFに
                animator.SetBool("Shot", false);
            }
        }

        // Ｚキー入力で発射
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Vector3 bootOffset = isGrounded  ? SHOT_GROUND_OFS : SHOT_JUMP_OFS;
            // ＰＬが向いている方向にオフセットをずらす
            bootOffset.x *= transform.localScale.x;
            // 弾起動
            Instantiate(shell1,
                transform.position + bootOffset,
                transform.rotation);

            // 発射フラグをONに
            animator.SetBool("Shot", true);
            // 次弾発射までの待機時間を設定
            shotWait = SHOT_WAIT_DEFAULT;

            // 待機の時のみ撃つたび初期化してみる
            if (nowAnimName == "PLStayShot")
            {
                animator.Play(nowAnimName, -1, 0);         // 最初から再生
            }
        }
    }











    void OnCollisionEnter2D(Collision2D other)
    {
        // ヒットした瞬間
        Debug.Log("Hit");
    }

    void OnCollisionStay2D(Collision2D other)
    {
        // 何かと接触している間ずっとこの関数を通る
    }
}
