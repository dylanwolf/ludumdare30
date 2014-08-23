using UnityEngine;
using System.Collections;

public class NextTile : MonoBehaviour {

	private SpriteRenderer _r;

	void Awake () {
		_r = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		_r.sprite = Board.Current.Textures[Board.ShowingBoard2 ? Board.NextTile2 : Board.NextTile1];
	}
}
