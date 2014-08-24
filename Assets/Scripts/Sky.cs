using UnityEngine;
using System.Collections;

public class Sky : MonoBehaviour {

	public Sprite World1;
	public Sprite World2;

	SpriteRenderer _s;

	void Awake()
	{
		_s = GetComponent<SpriteRenderer>();
	}

	void Update () {
		_s.sprite = Board.ShowingBoard2 ? World1 : World2;
	}
}
