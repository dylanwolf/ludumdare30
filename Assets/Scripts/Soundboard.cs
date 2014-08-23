using UnityEngine;
using System.Collections;

public class Soundboard : MonoBehaviour {

	public static Soundboard Current;

	public AudioSource[] FlipBank;
	public AudioSource[] SwapBank;
	public AudioSource[] DropBank;
	public AudioSource[] ClearBank;

	bool IsPlayingDrop = false;
	bool IsPlayingClear = false;

	void Awake () {
		Current = this;
	}

	void PlayRandomSound(AudioSource[] array)
	{
		array[Random.Range(0, array.Length)].Play();
	}

	void LateUpdate()
	{
		IsPlayingDrop = false;
		IsPlayingClear = false;
	}

	public static void PlayFlip()
	{
		Current.PlayRandomSound(Current.FlipBank);
	}

	public static void PlaySwap()
	{
		Current.PlayRandomSound(Current.SwapBank);
	}

	public static void PlayDrop()
	{
		if (!Current.IsPlayingDrop)
		{
			Current.PlayRandomSound(Current.DropBank);
			Current.IsPlayingDrop = true;
		}
	}

	public static void PlayClear()
	{
		if (!Current.IsPlayingClear)
		{
			Current.PlayRandomSound(Current.ClearBank);
			Current.IsPlayingClear = true;
		}
	}
}
