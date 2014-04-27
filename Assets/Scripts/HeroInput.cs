using UnityEngine;
using System.Collections;

public class HeroInput : CustomBehaviour {
	
	//--------------------------------------------------------------------------------
	// INTERACTION HALTING
	//--------------------------------------------------------------------------------
	
	int haltSemaphore = 0;
	
	public bool Halting { get { return haltSemaphore > 0; } }
	
	public void Halt() {
		++haltSemaphore;
	}
	
	public void Unhalt() {
		if (haltSemaphore > 0) { --haltSemaphore; }
	}
	
	//--------------------------------------------------------------------------------
	// POLLING
	//--------------------------------------------------------------------------------
	
	public bool PressingUp { get { return !Halting && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)); } }
	public bool PressingDown { get { return !Halting && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)); } }
	public bool PressingLeft { get { return !Halting && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)); } }
	public bool PressingRight { get { return !Halting && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)); } }
	public bool PressedJump { get { return !Halting && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)); } }
	public bool PressedItem { get { return !Halting && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)); } }
		
}
