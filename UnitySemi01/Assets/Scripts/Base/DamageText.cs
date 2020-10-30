using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class DamageText : MonoBehaviour
{
	void Start()
	{
//		mesh = GetComponent<TextMesh>();

		float x = Random.Range(-50, 50);
		GetComponent<Rigidbody2D>().AddForce(new Vector3(x, 400, 0), ForceMode2D.Impulse);

		StartCoroutine(DestroyObject());
	}

	// Update is called once per frame
	void Update()
	{
	}

	private IEnumerator DestroyObject()
	{
		yield return new WaitForSeconds(1);
		Destroy(this.gameObject);
	}
}