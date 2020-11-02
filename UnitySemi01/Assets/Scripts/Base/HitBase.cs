using UnityEngine;

/*
	ヒットの仕様

	HitBaseを親にアタッチする。
	体力はそこで管理

	防御用のアタリ判定は親が持つ（現状）

	子供のゲームオブジェクトを作成し、
	トリガーのアタリ判定、AtkParmをアタッチする。
	攻撃力の設定をする。
	※現状は子供の攻撃は同タイミングでは1つしか当たらない

	敵の攻撃の場合はEnemyHitを持たせる。

	PLの攻撃の場合はPLShellHitを持たせる。

	攻撃した時に判定をさせるような仕組みにしている（現状）

*/

public class HitBase : MonoBehaviour
{
	[SerializeField] int maxHP = 1;         // 最大体力
	[SerializeField] int HP = 0;            // 体力

	[SerializeField] int hitResult = 0;

	public enum RESULT
	{
		NONE,
		DAMAGE,
		DIE,
		RECOVERY,

		NUM
	};

	private void Start()
	{
		HP = maxHP;
	}
	private void Update()
	{		
	}
	private void LateUpdate()
	{
		hitResult = 0;
	}

	// アクセサ
	public int getMaxHP() { return maxHP; }
	public int getHP() { return HP; }
	public void setHP(int value) {
		if (value < 0)
		{
			value = 0;
		}
		else if (value > maxHP)
		{
			value = maxHP;
		}
		HP = value;
	}
	public void addHP(int value){
		value += HP;
		setHP(value);
	}
	public int getResult(){ return hitResult; }
	public bool checkResultDie() { return hitResult == (int)RESULT.DIE ? true : false; }







	public bool checkDie() { return HP > 0 ? false : true; }


	public void bootDamageText(Vector3 pos, string numString, Color col)
	{
		// （現状毎回ロードしてる分重いかもしれない
		GameObject textPre = (GameObject)Resources.Load("Prefabs/DamageText");
		GameObject bootText = Instantiate(textPre, pos, Quaternion.identity);

		TextMesh mesh = bootText.GetComponent<DamageText>().GetComponent<TextMesh>();

		mesh.text = numString;
		mesh.color = col;
	}


	public int damageCalc(AtkParam atk, HitBase def)
	{
		int result = (int)RESULT.NONE;

		// nullチェック
		if (atk == null || def == null)
		{
			return result;
		}

		// 攻撃側のリストをチェック
		if (atk.getAtkList().Count > 0) {
			// リストがあるので、すでに攻撃したオブジェクトを攻撃していないかチェック
			foreach (GameObject defObj in atk.getAtkList()) {
				// 同じインスタンスIDを攻撃していたらこれ以上行わない
				if (defObj.GetInstanceID() == def.gameObject.GetInstanceID())
				{
					return result;
				}
			}
		}

		int atkPow = atk.getAtkPower();
		int hp = def.getHP();

		// 敵の体力が無い場合は行わない
		if (hp <= 0)
		{
			result = (int)RESULT.DIE;
			hitResult = result;
			return result;
		}

		// 数値の値によって回復かダメージか
		if (atkPow >= 0)
		{
			// 便宜上プラスの値がダメージ
			result = (int)RESULT.DAMAGE;
		}
		else
		{
			// 回復
			result = (int)RESULT.RECOVERY;
		}
		hp += -atkPow;

		if (hp <= 0)
		{
			result = (int)RESULT.DIE;
		}

		// HPを設定
		def.setHP(hp);

		// リストに追加
		atk.getAtkList().Add(def.gameObject);


		// カラーを変更
		Color col;
		if (atkPow < 0)
		{
			// 回復
			col = new Color(0.5f, 1, 0.5f, 1);
			atkPow = Mathf.Abs(atkPow);
		}
		else
		{
			// ダメージ
			if (atk.getAtkGroup() == AtkParam.Group.PLAYER)
			{
				// プレイヤーの攻撃は黄色
				col = new Color(1, 1, 0.5f, 1);
			}
			else
			{
				// 敵の攻撃は赤
				col = new Color(1, 0, 0, 1);
			}
		}

		// ダメージテキストの起動
		bootDamageText(def.transform.position,
			atkPow.ToString(),
			col
		);

		// 結果を保存
		hitResult = result;
		return result;
	}
}
