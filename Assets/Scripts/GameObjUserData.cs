using UnityEngine;
using System.Collections;

public class GameObjUserData : MonoBehaviour {

	public enum GOType
	{
		DontCare,
		Tile,


	}

	public GOType goType = GOType.DontCare;
}
