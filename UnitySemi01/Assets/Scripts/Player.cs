using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // public変数（Unityのインスペクタの値が優先される
    public float dashSpeedX = 120.0f;   // 歩くスピード
    public float jumpPower = 400.0f;  // ジャンプ力
    public LayerMask groundLayer;       // 地面チェック用のレイヤー

    // private変数(クラス外からアクセス不可能）
    // [SerializeField]をつけるとインスペクタで操作可能になる
    private Rigidbody2D rigidbody2D;    // コンポーネント用変数
    private BoxCollider2D boxcollider2D;// コンポーネント用変数
    private Animator animator;          // コンポーネント用変数
    private bool isGrounded;            // 地上にいるかの判定

    private float inputLR = 0;        // 左右入力
    private bool inputJump = false;     // ジャンプ入力


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
        // 接地していなければジャンプ中設定
        if (isGrounded)
        {
            // 地上
            rigidbody2D.velocity.Set(rigidbody2D.velocity.x, 0);    // Ｙ軸速度を０に
            animator.SetBool("JumpUp", false);      // ジャンプ上昇をOFF
            animator.SetBool("JumpDown", false);    // ジャンプ下降をOFF 
        }
        else
        {
            // 空中
            // Ｙ軸方向の速度で、上昇中か下降中かを設定する(０の時は下降設定)
            bool isJumpUp = rigidbody2D.velocity.y > 0 ? true : false;
            bool isJumpDown = rigidbody2D.velocity.y <= 0 ? true : false;

            animator.SetBool("JumpUp", isJumpUp);
            animator.SetBool("JumpDown", isJumpDown);

            // Runアニメーションを止める
            animator.SetBool("Run", false);
        }


        //着地していた時、
        if (isGrounded)
        {
            // スペースキー入力をチェック
            if (Input.GetKeyDown("space"))
            {
                // ジャンプ入力設定
                inputJump = true;
                // 着地判定をfalse
                isGrounded = false;
            }
        }

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

            // 入力があるときは走り状態へ
            animator.SetBool("Run", true);
        }
        else
        {
            inputLR = 0;
            // 入力がないときは走り状態解除
            animator.SetBool("Run", false);
        }
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
        Vector3 chkPos = transform.position;
        float boxHarfWitdh = boxcollider2D.size.x / 2;
        bool result = false;
        Vector3 lineLength = transform.up * 0.05f;

        // ３点チェック（とりあえず）
        chkPos.x = transform.position.x - boxHarfWitdh;
        Debug.Log(transform.position.x);
        Debug.Log(boxHarfWitdh);
        Debug.Log(chkPos.x);

        for (; chkPos.x <= transform.position.x + boxHarfWitdh; chkPos.x += boxHarfWitdh)
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

        }

        return result;
    }
}
