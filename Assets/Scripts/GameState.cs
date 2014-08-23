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

}
