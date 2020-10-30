using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkParam : MonoBehaviour
{
	[SerializeField] private int atkPower = 1;      // 攻撃力	
	[SerializeField] private int atkElement = 0;      // グループ
	[SerializeField] private int atkKind = 0;       // 攻撃の種類	
	[SerializeField] private Group atkGroup = Group.PLAYER;    // 属性

	private List<GameObject> atkList = new List<GameObject>();

	// 列挙対
	public enum Group : int{
		PLAYER,
		ENEMY,

		NONE
	};

	private void Start()
	{
		atkList.Clear();
	}
	private void Update()
	{
		// アップデート毎にリストリセット
		atkList.Clear();
	}

	public int getAtkPower() { return atkPower; }
	public Group getAtkGroup() { return atkGroup; }

	public List<GameObject> getAtkList() { return atkList; }
}
