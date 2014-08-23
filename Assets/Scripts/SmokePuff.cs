using UnityEngine;
using System.Collections;

public class SmokePuff : MonoBehaviour {

	private Animator _a;

	void Awake()
	{
		_a = GetComponent<Animator>();
	}

	public void Finish()
	{
		DestroyObject(this.gameObject);
	}
}
