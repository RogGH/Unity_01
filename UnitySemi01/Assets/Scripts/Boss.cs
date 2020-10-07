using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    // レイヤー
    public LayerMask groundLayer;       // 地面チェック用のレイヤー
    public GameObject bossSlider;       // 体力バー

    private Animator animator;          // コンポーネント用変数
    private Rigidbody2D rigid2D;        //

    // 行動関連
    enum ACTNO {
        APP,
        SHOT_F,
        JUMP_SHOT,
        RUN,

        DIE,

        NUM
    };
    private int actNo = 0;
    private int actMethodNo = 0;
    private float ctr0;
    private float ctr1;
    // 関数テーブル
    delegate void ActFunc();
    private ActFunc[]actFuncTbl;

    // アニメーション名前
    const string ANAME_RUN = "BS_Run";
    const string ANAME_JUMP_U = "BS_JumpUp";
    const string ANAME_JUMP_D = "BS_JumpDown";
    const string ANAME_LANDING = "BS_Landing";
    const string ANAME_IDLE = "BS_Idle";
    const string ANAME_PAUSE = "BS_Pause";
    const string ANAME_SHOT_READY_F = "BS_ShotReadyF";
    const string ANAME_SHOT_F = "BS_ShotF";
    const string ANAME_SLASH = "BS_Slash";
    const string ANAME_DIE = "BS_Die";

    // コンポーネント
    private Slider slider;

    // パラメータ
    private int HitPoint = 0;

    // 調整用
    public float jumpPower = 400;
    public float runSpeed = 200;
    public float runLength = 48;

    // ＳＥ
    public AudioClip damageSE;          // ダメージSE

    // 定数
    const int DIE_END_NO = 0xff;

    // Start is called before the first frame update
    void Start()
    {
        changeActNo(ACTNO.APP);

        // 関数テーブル登録
        actFuncTbl = new ActFunc[(int)ACTNO.NUM];
        actFuncTbl[(int)ACTNO.APP] = ActApp;
        actFuncTbl[(int)ACTNO.SHOT_F] = ActShotF;
        actFuncTbl[(int)ACTNO.JUMP_SHOT] = ActJumpShot;
        actFuncTbl[(int)ACTNO.RUN] = ActRun;
        actFuncTbl[(int)ACTNO.DIE] = ActDie;

        // 
        gameObject.layer = LayerMask.NameToLayer("Player");

        animator = GetComponent<Animator>();
        slider = bossSlider.GetComponent<Slider>();
        rigid2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // 体力値をバーに反映させる
        if (actNo != 0) {
            slider.value = HitPoint;

            // 体力０チェック
            if (HitPoint <= 0)
            {
                // 強制的に死亡処理へ
                if (actNo != (int)ACTNO.DIE)
                {
                    changeActNo(ACTNO.DIE);
                }
            }
        }

        actFuncTbl[actNo]();
    }

    // 登場
    void ActApp()
    {
        if (!checkGrounded())
        {
            return;
        }

        switch (actMethodNo)
        {
            case 0:
                // ジャンプ落下中のみ
                if (getAnimeName() == ANAME_JUMP_D)
                {
                    animator.Play(ANAME_LANDING);
                    ctr0 = 1.0f;
                }
                // 待機中になったら
                else
                if (getAnimeName() == ANAME_IDLE)
                {
                    if (decCtr0())
                    {
                        animator.Play(ANAME_PAUSE);
                    }
                }
                // ポーズ中
                else
                if (getAnimeName() == ANAME_PAUSE)
                {
                    // 再生が終わったかチェック
                    if (getAnimeCtr() > 1.0f)
                    {
                        // ボスＨＰをアクティブに
                        bossSlider.SetActive(true);
                        // 次の工程へ
                        ++actMethodNo;
                    }
                }
                break;

            case 1:
                ctr0 = 2;
                ctr1 = bossSlider.GetComponent<Slider>().maxValue / ctr0 * Time.deltaTime;

                ++actMethodNo;
                break;

            case 2:
                // 体力を加算していく
                slider.value += ctr1;
                HitPoint = (int)slider.value;

                if (decCtr0())
                {
                    slider.value = slider.maxValue;
                    HitPoint = (int)slider.maxValue;

                    gameObject.layer = LayerMask.NameToLayer("Enemy");

                    // 登場演出終了
                    changeActNo(ACTNO.SHOT_F);
                }
                break;
        }
    }

    void ActShotF()
    {
        switch (actMethodNo)
        {
            case 0:
                animator.Play(ANAME_SHOT_READY_F);
                setDirToPl();

                ctr0 = 0.5f;
                ++actMethodNo;
                break;

            case 1:
                if (decCtr0())
                {
                    ++actMethodNo;
                }
                break;

            case 2:
                animator.Play(ANAME_SHOT_F);

                GameObject shell1 = (GameObject)Resources.Load("Prefabs/EnemyShell1");

                Vector3 bootOffset = new Vector3(30, 48, 0);
                // ＰＬが向いている方向にオフセットをずらす
                bootOffset.x *= transform.localScale.x;

                // 弾起動
                Instantiate(shell1,
                    transform.position + bootOffset,
                    transform.rotation);
                shell1.GetComponent<EnemyShell1>().setup(
                    (int)-transform.localScale.x,
                    0);

                ++actMethodNo;

                ctr0 = 1.0f;
                break;

            case 3:
                if (decCtr0())
                {
                    changeActNo(ACTNO.JUMP_SHOT);
                }
                break;

        }
    }

    void ActJumpShot()
    {
        switch (actMethodNo)
        {
            case 0:
                animator.Play(ANAME_JUMP_U);

                rigid2D.velocity.Set(0, 0);    // Ｙ軸速度を０に
                rigid2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

                ++actMethodNo;
                break;

            case 1:
                if (rigid2D.velocity.y <= 0)
                {
                    if (getAnimeName() != ANAME_JUMP_D)
                    {
                        animator.Play(ANAME_JUMP_D);
                    }

                    // 着地したら終了
                    if (checkGrounded())
                    {
                        ++actMethodNo;
                    }
                }
                break;

            case 2:
                animator.Play(ANAME_LANDING);
                ctr0 = 0.2f;
                ++actMethodNo;

                break;

            case 3:
                // 着地したら終了
                if (decCtr0())
                {
                    changeActNo(ACTNO.RUN);
                }
                break;
        }
    }

    void ActRun()
    {
        switch (actMethodNo)
        {
            case 0:
                animator.Play(ANAME_RUN);
                ++actMethodNo;
                break;

            case 1:
                // プレイヤーの方向に近づいたら停止して剣を振る
                setDirToPl();
                rigid2D.velocity = new Vector2(runSpeed * getPlDir(), 0);

                float length = getPlObject().transform.position.x - transform.position.x;
                if ( Mathf.Abs(length) <= runLength) {
//                    rigid2D.velocity = new Vector2(0, 0);
                    ++actMethodNo;
                }
                break;

            case 2:
                animator.Play(ANAME_SLASH);
                ctr0 = 1.0f;
                ++actMethodNo;
                break;

            case 3:
                if (decCtr0())
                {
                    changeActNo(ACTNO.SHOT_F);
                }
                break;
        }
    }


    void ActDie()
    {
        switch (actMethodNo) {
            case 0:
                animator.Play(ANAME_DIE);

                // 子供を消す
                GameObject hit = transform.Find("BossAtkHit").gameObject;
                Destroy(hit);

                gameObject.layer = LayerMask.NameToLayer("Player");

                ctr0 = 3.0f;

                ++actMethodNo;
                break;

            case 1:
                ctr0 -= Time.deltaTime;
                if (ctr0 <= 0)
                {
                    // カメラを修正
                    // 死んだ通知用
                    actMethodNo = DIE_END_NO;
                }
                break;
        }
    }

    private bool checkGrounded()
    {
        Vector3 chkPos = transform.position;
        float boxHarfWitdh = GetComponent<BoxCollider2D>().size.x / 2;
        bool result = false;
        Vector3 lineLength = transform.up * 0.05f;

        // ３点チェック（とりあえず）
        chkPos.x = transform.position.x - boxHarfWitdh;
        for (int loopNo = 0; loopNo < 3; ++loopNo)
        {
            // 自身の位置から↓に向かって線を引いて、地面に接触したかをチェックする
            result |= Physics2D.Linecast(
                chkPos + transform.up,
                chkPos - lineLength,
                groundLayer
                );
            // レイを表示してみる
            Debug.DrawLine(chkPos + transform.up,
                chkPos - lineLength,
                Color.red);

            // オフセット加算
            chkPos.x += boxHarfWitdh;
        }
        return result;
    }
    private string getAnimeName()
    {
        AnimatorClipInfo[] currentClipInfo = this.animator.GetCurrentAnimatorClipInfo(0);
        return currentClipInfo[0].clip.name;
    }
    private float getAnimeCtr()
    {
        return GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    private bool decCtr0()
    {
        ctr0 -= Time.deltaTime;
        if ( ctr0 <= 0 )
        {
            return true;
        }
        return false;
    }

    private void changeActNo(ACTNO no)
    {
        actNo = (int)no;
        actMethodNo = 0;
    }
    private GameObject getPlObject()
    {
        GameObject pl = GameObject.Find("Player");
        return pl;
    }
    private int getPlDir()
    {
        return getPlObject().transform.position.x - transform.position.x < 0 ? -1 : 1;
    }
    private void setDirToPl()
    {
        Vector2 temp = transform.localScale;
        temp.x = getPlDir();
        transform.localScale = temp;
    }



    // 外部呼出しメソッド
    public void AddHitPoint(int point)
    {
        // 体力０の時は行わない
        if (this.HitPoint <= 0)
        {
            return;
        }

        // ＨＰに加算
        this.HitPoint += point;
        // ダメージかチェック
        if (point < 0)
        {
            // 念のためマイナスになったら０に設定しておく
            if (this.HitPoint <= 0)
            {
                // 死亡
                this.HitPoint = 0;
//                this.StartCoroutine("Die");
            }
            else
            {
                // ダメージなので、ダメージ専用呼び出し処理
                AudioSource.PlayClipAtPoint(this.damageSE, this.transform.position);
//               this.StartCoroutine("Damage");
            }
        }
    }

    public bool chkDie()
    {
        if (actNo == (int)ACTNO.DIE)
        {
            if( actMethodNo == DIE_END_NO)
            {
                return true;
            }
        }
        return false;
    }
}
