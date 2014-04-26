using UnityEngine;
using System.Collections;

public class HeroInput : CustomBehaviour {

	public bool PressingUp { get { return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W); } }
	public bool PressingDown { get { return Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S); } }
	public bool PressingLeft { get { return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A); } }
	public bool PressingRight { get { return Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D); } }
	public bool PressedJump { get { return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W); } }
	public bool PressedItem { get { return Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return); } }
		
}
