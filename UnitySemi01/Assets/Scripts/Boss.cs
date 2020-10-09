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
    private float ctr2;
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
    const string ANAME_SHOT_READY_SD = "BS_ShotReadySlopeD";
    const string ANAME_SHOT_F = "BS_ShotF";
    const string ANAME_SHOT_SD = "BS_ShotSlopeD";
    const string ANAME_SLASH = "BS_Slash";
    const string ANAME_DIE = "BS_Die";

    // コンポーネント
    private Slider slider;

    // パラメータ
    private int HitPoint = 0;

    // 調整用
    [SerializeField] float jumpPower = 400;
    [SerializeField] float runSpeed = 300;
    [SerializeField] float runLength = 48;
    [SerializeField] float shotReadyWait = 0.3f;
    [SerializeField] float groundShotWait = 0.4f;
    [SerializeField] float airShotReadyWait = 0.3f;
    [SerializeField] float airShotWait = 0.4f;
    [SerializeField] float landingWait = 0.15f;
    [SerializeField] float swartWait = 0.5f;

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
                ctr0 = 2.0f;        // ＨＰがマックスになる時間
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


    void shotMethodCtrl(int rotDegree)
    {
        // 発射
        GameObject shell1 = (GameObject)Resources.Load("Prefabs/EnemyShell1");

		Vector3 bootOffset;
		if ( rotDegree == 0 )
		{
			// 正面
			bootOffset = new Vector3(30, 48, 0);
		}
		else {
			// 斜め下
			bootOffset = new Vector3(30, 20, 0);
		}
		// ＰＬが向いている方向にオフセットをずらす
		bootOffset.x *= transform.localScale.x;

        // 弾起動
        GameObject boot;
        boot = Instantiate(shell1,
            transform.position + bootOffset,
            transform.rotation);
        boot.GetComponent<EnemyShell1>().setup(
            (int)transform.localScale.x,
            rotDegree);
    }

    void ActShotF()
    {
        switch (actMethodNo)
        {
            case 0:
                animator.Play(ANAME_SHOT_READY_F);

                ctr0 = shotReadyWait;
                ++actMethodNo;
                break;

            case 1:
                setDirToPl();

                if (decCtr0())
                {
                    ++actMethodNo;
                }
                break;

            case 2:
                animator.Play(ANAME_SHOT_F);
                ++actMethodNo;

                ctr0 = groundShotWait;        // 終了時間
                ctr1 = 0;           // 発射数
                ctr2 = 2;           // 

                break;

            case 3:

                if (decCtr1())
                {
                    if (ctr2 > 0)
                    {
                        --ctr2;                         // 発射数減算
                        ctr1 = 0.1f;                    // 再発射間隔を設定
                        // 正面発射
                        shotMethodCtrl(0);
                    }
                }

                if (decCtr0())
                {
                    changeActNo(ACTNO.JUMP_SHOT);
                }
                break;

        }
    }

    enum JS_NO
    {
        UP_INIT,
        UP_MAIN,
        SHOT_READY_INIT,
        SHOT_READY_MAIN,
        SHOT_INIT,
        SHOT_MAIN,

        DOWN_INIT,
        DOWN_MAIN,

        LAND_INIT,
        LAND_MAIN,
    };
    void ActJumpShot()
    {
        switch (actMethodNo)
        {
            case (int)JS_NO.UP_INIT:
                animator.Play(ANAME_JUMP_U);

                rigid2D.velocity.Set(0, 0);    // Ｙ軸速度を０に
                rigid2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

                ++actMethodNo;
                break;

            case (int)JS_NO.UP_MAIN:
                if (rigid2D.velocity.y <= 0)
                {
                    // ここでジャンプショットに行くかは確率
                    if (Random.Range(0, 10) < 5 )
                    {
                        actMethodNo = (int)JS_NO.SHOT_READY_INIT;
                    }
                    else
                    {
                        actMethodNo = (int)JS_NO.DOWN_INIT;
                    }
                    ActJumpShot();
                }
                break;

            case (int)JS_NO.SHOT_READY_INIT:
                float height = transform.position.y - getPlLengthY() + 30.0f;
                if (Mathf.Abs(height) <= 30.0f)
                {
                    // 正面
                    animator.Play(ANAME_SHOT_READY_F);
                }
                else
                {
                    // 斜め
                    animator.Play(ANAME_SHOT_READY_SD);
                }

                rigid2D.velocity = new Vector2(0, 0);       // Ｙ軸速度を０に
                rigid2D.angularVelocity = 0;
                rigid2D.bodyType = RigidbodyType2D.Kinematic;

                ctr0 = airShotReadyWait;
                ctr1 = 0;           // 発射数
                ctr2 = 2;           // 

                ++actMethodNo;
                break;
            case (int)JS_NO.SHOT_READY_MAIN:
                setDirToPl();
                if (decCtr0())
                {
                    ++actMethodNo;
                }
                break;

            case (int)JS_NO.SHOT_INIT:

                if( getAnimeName() == ANAME_SHOT_READY_F)
                {
                    // 正面
                    animator.Play(ANAME_SHOT_F);
                }
                else
                {
                    // 斜め
                    animator.Play(ANAME_SHOT_SD);
                }

				ctr0 = airShotWait;        // 終了時間
				ctr1 = 0;           // 発射数
				ctr2 = 2;           // 

                ++actMethodNo;
                break;
            case (int)JS_NO.SHOT_MAIN:

                if (decCtr1())
                {
                    if (ctr2 > 0)
                    {
                        --ctr2;                         // 発射数減算
                        ctr1 = 0.1f;                    // 再発射間隔を設定 
                        shotMethodCtrl(getAnimeName() == ANAME_SHOT_F ? 0 : -45);
                    }
                }

                if (decCtr0())
                {
                    actMethodNo = (int)JS_NO.DOWN_INIT;
                    rigid2D.bodyType = RigidbodyType2D.Dynamic;
                }
                break;

            case (int)JS_NO.DOWN_INIT:
                animator.Play(ANAME_JUMP_D);
                ++actMethodNo;
                break;
            case (int)JS_NO.DOWN_MAIN:

                // 着地したら終了
                if (checkGrounded())
                {
                    actMethodNo = (int)JS_NO.LAND_INIT;
                }
                break;

            case (int)JS_NO.LAND_INIT:

                animator.Play(ANAME_LANDING);
                ctr0 = landingWait;
                ++actMethodNo;
                break;

            case (int)JS_NO.LAND_MAIN:
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

                float length = getPlLengthX();
                if ( Mathf.Abs(length) <= runLength) {
//                    rigid2D.velocity = new Vector2(0, 0);
                    ++actMethodNo;
                }
                break;

            case 2:
                animator.Play(ANAME_SLASH);
                ctr0 = swartWait;
                ++actMethodNo;
                break;

            case 3:
                // 現在のアニメーターの状態をチェック
                if ( 0.3 < getAnimeCtr() && getAnimeCtr() < 0.6 )
                {
                    transform.root.GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    transform.root.GetChild(1).gameObject.SetActive(false);
                }

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
                rigid2D.bodyType = RigidbodyType2D.Dynamic;

                // 子供を消す
                GameObject hit = transform.Find("BossHit").gameObject;
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
    private bool decCtr1()
    {
        ctr1 -= Time.deltaTime;
        if (ctr1 <= 0)
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
    private float getPlLengthX()
    {
        if (getPlObject() == null)
        {
            return 0;
        }
        return getPlObject().transform.position.x - transform.position.x;
    }
    private float getPlLengthY()
    {
        if (getPlObject() == null)
        {
            return 0;
        }
        return getPlObject().transform.position.y - transform.position.y;
    }

    private int getPlDir()
    {
        if (getPlObject() == null )
        {
            return (int)transform.localScale.x;
        }
        return getPlLengthX() < 0 ? -1 : 1;
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
