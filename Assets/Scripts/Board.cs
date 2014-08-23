using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	public const int BoardSize = 7;
	public const int GenerateRows = 4;
	public const float WorldOffset = 1.92f;
	public const float TileSize = 0.64f;

	private static Transform _t;

	public Sprite[] Textures;
	public GameTile TilePrefab;

	public Transform SmokePuffPrefab;

	public static GameTile[,] Tiles1 = new GameTile[BoardSize, BoardSize];
	public static GameTile[,] Tiles2 = new GameTile[BoardSize, BoardSize];

	public static int NextTile1;
	public static int NextTile2;

	public static Board Current;

	void Awake()
	{
		_t = GetComponent<Transform>();
		Current = this;

		// Test code
		GenerateBoard();

	}

	private static float timer;
	private const float DisappearTimer = 0.667f;
	void Update()
	{
		if (GameState.Mode == GameState.GameMode.Disappearing)
		{
			timer -= Time.deltaTime;
			if (timer <= 0)
			{
				GameState.Mode = GameState.GameMode.Playing;
				SettleBlocks(Tiles1);
				SettleBlocks(Tiles2);
			}
		}
		if (GameState.Mode == GameState.GameMode.Falling && fallingBlocks.Count == 0)
		{
			GameState.Mode = GameState.GameMode.Playing;
			MatchAndClear(ShowingBoard2 ? Tiles2 : Tiles1);
		}
	}

	private Vector3 mouseClick;
	private int mouseX;
	private int mouseY;
	void OnMouseDown()
	{
		mouseClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouseX = (int)Mathf.Round ((mouseClick.x + WorldOffset) / TileSize);
		mouseY = (int)Mathf.Round ((mouseClick.y + WorldOffset) / TileSize);
		PutNextTile(mouseX, mouseY);
	}

	static GameTile currentTile;
	static List<GameTile> collector = new List<GameTile>();
	static GameTile[,] toTest  = new GameTile[BoardSize, BoardSize];

	public static void PutNextTile(int x, int y)
	{
		Tiles1[x, y] = (GameTile)Instantiate(Current.TilePrefab);
		Tiles1[x,y].transform.parent = _t;
		Tiles1[x, y].TileColor = NextTile1;
		Tiles1[x, y].ToggleVisibility(!ShowingBoard2);

		Tiles2[x, y] = (GameTile)Instantiate(Current.TilePrefab);
		Tiles2[x, y].TileColor = NextTile2;
		Tiles2[x,y].transform.parent = _t;
		Tiles2[x, y].ToggleVisibility(ShowingBoard2);

		UpdateIndexes(true);
		SettleBlocks(Tiles1);
		SettleBlocks(Tiles2);
		GenerateNextTile();
		MatchAndClear(ShowingBoard2 ? Tiles2 : Tiles1);
	}

	public static void GenerateBoard()
	{
		ShowingBoard2 = false;
		for (int y = 0; y < GenerateRows; y++)
		{
			for (int x = 0; x < BoardSize; x++)
			{
				Tiles1[x, y] = (GameTile)Instantiate(Current.TilePrefab);
				Tiles1[x,y].transform.parent = _t;
				Tiles1[x, y].TileColor = (int)Random.Range(0, Current.Textures.Length);

				Tiles2[x, y] = (GameTile)Instantiate(Current.TilePrefab);
				Tiles2[x, y].TileColor = (int)Random.Range(0, Current.Textures.Length);
				Tiles2[x,y].transform.parent = _t;
				Tiles2[x, y].ToggleVisibility(false);
			}
		}

		GenerateNextTile();
		UpdateIndexes(true);
		MatchAndClear(Tiles1);
	}

	public static void GenerateNextTile()
	{
		NextTile1 = (int)Random.Range(0, Current.Textures.Length);
		NextTile2 = (int)Random.Range(0, Current.Textures.Length);
	}

	public static bool ShowingBoard2 = false;
	public static void SwapBoards()
	{
		ShowingBoard2 = !ShowingBoard2;
		SetBoardVisibility(ShowingBoard2 ? Tiles1 : Tiles2, false);
		SetBoardVisibility(ShowingBoard2 ? Tiles2 : Tiles1, true);
		MatchAndClear(ShowingBoard2 ? Tiles2 : Tiles1);
	}

	static void SetBoardVisibility(GameTile[,] board, bool visible)
	{
		for (int y = 0; y < BoardSize; y++)
		{
			for (int x = 0; x < BoardSize; x++)
			{
				if (board[x,y] != null)
				{
					board[x,y].ToggleVisibility(visible);
				}
			}
		}
	}

	public static void UpdateIndexes(bool updatePositions)
	{
		for (int y = 0; y < BoardSize; y++)
		{
			for (int x = 0; x < BoardSize; x++)
			{
				if (Tiles1[x,y] != null)
				{
					Tiles1[x, y].Row = y;
					Tiles1[x, y].Column = x;
					if (updatePositions)
						Tiles1[x, y].UpdatePosition();
				}
				if (Tiles2[x,y] != null)
				{
					Tiles2[x, y].Row = y;
					Tiles2[x, y].Column = x;
					if (updatePositions)
						Tiles2[x, y].UpdatePosition();
				}
			}
		}
	}

	static void CopyBoard(GameTile[,] source, GameTile[,] destination)
	{
		for (int y = 0; y < BoardSize; y++)
		{
			for (int x = 0; x < BoardSize; x++)
			{
				destination[x, y] = source[x, y];
			}
		}
	}

	static Vector3 tmpPos;
	static void ClearTile(int x, int y)
	{
		tmpPos.x = (x * Board.TileSize) - WorldOffset;
		tmpPos.y = (y * Board.TileSize) - WorldOffset;
		Instantiate (Current.SmokePuffPrefab, tmpPos, Quaternion.identity);

		if (Tiles1[x, y] != null)
		{
			DestroyImmediate(Tiles1[x, y].gameObject);
		}
		if (Tiles2[x, y] != null)
		{
			DestroyImmediate(Tiles2[x, y].gameObject);
		}
	}

	static GameTile tmpTile;
	public static void FlipTile(GameTile tile)
	{
		// Tile1[x,y] should never be null if and Tile2[x,y] isn't, and vice versa

		// Pull the matching tile from the hidden board
		tmpTile = ShowingBoard2 ? Tiles1[tile.Column, tile.Row] : Tiles2[tile.Column, tile.Row];

		// If showing board 2, board 1 should have the clicked tile, and board 2 should have the hidden tile
		// If showing board 1, board 2 should have the clicked tile, and board 1 should have the hidden tile

		Tiles1[tile.Column, tile.Row] = ShowingBoard2 ? tile : tmpTile;
		Tiles2[tile.Column, tile.Row] = ShowingBoard2 ? tmpTile : tile;

		tile.ToggleVisibility(false);
		tmpTile.ToggleVisibility(true);

		UpdateIndexes(true);
		MatchAndClear(ShowingBoard2 ? Tiles2 : Tiles1);
	}

	static bool clearedTiles = false;
	public static void MatchAndClear(GameTile[,] board)
	{
		clearedTiles = false;
		// Make a copy of the board to test
		CopyBoard(board, toTest);
		currentTile = null;
		collector.Clear ();

		for (int y = 0; y < BoardSize; y++)
		{
			for (int x = 0; x < BoardSize; x++)
			{
				TestTile (x, y);
				if (collector.Count >= 3)
				{
					foreach (GameTile tile in collector)
					{
						ClearTile(tile.Column, tile.Row);
						clearedTiles = true;
					}
				}
				currentTile = null;
				collector.Clear ();
			}
		}

		if (clearedTiles)
		{
			timer = DisappearTimer;
			GameState.Mode = GameState.GameMode.Disappearing;
		}
	}

	static int? firstEmpty;
	public static List<GameTile> fallingBlocks = new List<GameTile>();
	public static void SettleBlocks(GameTile[,] board)
	{
		fallingBlocks.Clear ();
		for (int x = 0; x < BoardSize; x++)
		{
			firstEmpty = null;
			for (int y = 0; y < BoardSize; y++)
			{
				if (board[x, y] == null && !firstEmpty.HasValue)
				{
					firstEmpty = y;
				}
				else if (firstEmpty.HasValue && board[x, y] != null)
				{
					board[x, y].LastRow = y;
					fallingBlocks.Add(board[x, y]);
					board[x, firstEmpty.Value] = board[x, y];
					board[x, y] = null;
					firstEmpty++;
				}
			}
		}

		if (fallingBlocks.Count > 0)
		{
			GameState.Mode = GameState.GameMode.Falling;
		}

		UpdateIndexes(false);
	}

	static void TestTile(int x, int y)
	{
		// Tile already tested; skip
		if (toTest[x,y] == null)
		{
			return;
		}
		// Start testing a block
		if (currentTile == null)
		{
			currentTile = toTest[x, y];
			toTest[x, y] = null;
			collector.Add(currentTile);
		}
		// Tile doesn't match; skip
		else if (currentTile.TileColor != toTest[x, y].TileColor)
		{
			return;
		}
		// Tile matches
		else
		{
			collector.Add(toTest[x, y]);
			toTest[x, y] = null;
		}

		// If we're processing this tile, test all tiles around it
		if (x > 0)
			TestTile(x - 1, y);
		if (y > 0)
			TestTile(x, y - 1);
		if (x < Board.BoardSize - 1)
			TestTile(x + 1, y);
		if (y < Board.BoardSize - 1)
			TestTile(x, y + 1);
	}
}
