using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	private static SoundManager mInstance;
	public static SoundManager Instance
	{
		get
		{
			if (mInstance == null)
			{
				GameObject obj = new GameObject("SoundManager");
				mInstance = obj.AddComponent<SoundManager>();
			}
			return mInstance;
		}
		set
		{
		}
	}

	// Start is called before the first frame update
	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
