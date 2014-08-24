using UnityEngine;
using System.Collections;

public static class GameState {

	public enum GameMode
	{
		Playing,
		Disappearing,
		Falling
	}

	public static GameMode Mode = GameMode.Playing;

	public static int ActionsTaken = 0;

	public static void ResetGame()
	{
		if (Mode == GameMode.Playing)
		{
			ActionsTaken = 0;
			Board.ShowingBoard2 = true;
			Board.GenerateBoard();
		}
	}
}
