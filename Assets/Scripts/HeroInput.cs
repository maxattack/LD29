using InControl;
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
	
	public bool PressingUp { 
		get { 
			return !Halting && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || InputManager.ActiveDevice.DPadUp.IsPressed); 
		}
	}
	
	public bool PressingDown { 
		get { 
			return !Halting && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || InputManager.ActiveDevice.DPadDown.IsPressed); 
		}
	}
	
	public bool PressingLeft { 
		get { 
			return !Halting && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || InputManager.ActiveDevice.DPadLeft.IsPressed); 
		}
	}
	
	public bool PressingRight { 
		get { 
			return !Halting && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || InputManager.ActiveDevice.DPadRight.IsPressed); 
		}
	}
	
	public bool PressedJump { 
		get { 
			return !Halting && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.RightShift) || InputManager.ActiveDevice.Action1.WasPressed); 
		}
	}
	
	public bool PressedItem { 
		get { 
			return !Halting && (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Return) || InputManager.ActiveDevice.Action3.WasPressed); 
		}
	}
	
	public bool PressedDrop { 
		get { 
			return !Halting && (Input.GetKeyDown (KeyCode.C) || InputManager.ActiveDevice.Action2.WasPressed); 
		}
	}
	
	public bool AnyPress {
		get {
			foreach(var button in InputManager.ActiveDevice.Buttons) {
				if (button.WasPressed) { return true; }
			}
			return Input.anyKeyDown;
		}
	}
	
	
	//--------------------------------------------------------------------------------
	// EVENTS
	//--------------------------------------------------------------------------------
	
	void Awake() {
		GameInput.Setup();
	}
	
}
