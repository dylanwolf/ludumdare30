using UnityEngine;
using System.Collections;

public class TestButton : MonoBehaviour {

	void OnMouseUpAsButton()
	{
		if (GameState.Mode == GameState.GameMode.Playing)
		{
			Board.SwapBoards();
		}
	}
}
