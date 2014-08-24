using UnityEngine;
using System.Collections;

[RequireComponent(typeof(tk2dTextMesh))]
public class ActionsLabel : MonoBehaviour {

	int? LastActions;
	tk2dTextMesh _tm;

	void Awake()
	{
		_tm = GetComponent<tk2dTextMesh>();
	}
	
	void Update () {
		if (!LastActions.HasValue || LastActions.Value != GameState.ActionsTaken)
		{
			_tm.text = GameState.ActionsTaken.ToString();
			_tm.Commit();
			LastActions = GameState.ActionsTaken;
		}
	}
}
