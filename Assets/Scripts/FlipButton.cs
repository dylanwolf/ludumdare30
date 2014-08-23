using UnityEngine;
using System.Collections;

public class FlipButton : MonoBehaviour {

	void OnMouseUpAsButton()
	{
		if (GameState.Mode == GameState.GameMode.Playing)
		{
			Board.SwapBoards();
			Soundboard.PlaySwap();
		}
	}
}
