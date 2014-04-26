using UnityEngine;
using System.Collections;

public class HeroInput : CustomBehaviour {

	
	public bool PressingLeft { get { return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A); } }
	public bool PressingRight { get { return Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D); } }
	public bool PressedJump { get { return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W); } }
		
}
