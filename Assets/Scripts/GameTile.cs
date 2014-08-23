using UnityEngine;
using System.Collections;

public class GameTile : MonoBehaviour {

	private Transform _t;
	private SpriteRenderer _s;
	private BoxCollider _c;

	[System.NonSerialized]
	public int TileColor;

	[System.NonSerialized]
	public int Row;

	[System.NonSerialized]
	public int Column;
	
	void Awake () {
		_t = GetComponent<Transform>();
		_s = GetComponent<SpriteRenderer>();
		_c = GetComponent<BoxCollider>();
	}

	Vector3 tmpPos;
	void Update () {
		tmpPos = _t.position;
		tmpPos.x = ((Column * Board.TileSize) - Board.WorldOffset);
		tmpPos.y = (Row * Board.TileSize) - Board.WorldOffset;
		_t.position = tmpPos;

		_s.sprite = Board.Current.Textures[TileColor];
	}

	void OnMouseDown()
	{
		if (GameState.Mode == GameState.GameMode.Playing)
		{
			Debug.Log ("Flipping tile");
			Board.FlipTile(this);
		}
	}

	public void ToggleVisibility(bool visibility)
	{
		_s.enabled = visibility;
		_c.enabled = visibility;
	}
}
