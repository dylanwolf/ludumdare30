using UnityEngine;
using System.Collections;

public class RestartButton : MonoBehaviour {

	void OnMouseUpAsButton()
	{
		if (GameState.Mode == GameState.GameMode.Playing)
		{
			GameState.ResetGame();
			Soundboard.PlaySwap();
		}
	}
}
