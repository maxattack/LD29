using InControl;
using System.Collections;
using UnityEngine;

public class GameInput : MonoBehaviour {
	static GameInput inst;
	
	public static void Setup() {
		if (inst == null) {
			DontDestroyOnLoad( new GameObject("GameInput", typeof(GameInput)) );
		}
	}
	
	void Awake() {
		inst = this;
		InputManager.Setup();
	}
	
	void OnDestroy() {
		if (inst == this) { inst = null; }
	}
	
	void Update() {
		InputManager.Update();
	}
	
	public static bool AnyPress {
		get {
			Setup();
			foreach(var btn in InputManager.ActiveDevice.Buttons) {
				if (btn.WasPressed) { return true; }
			}
			return Input.anyKeyDown;
		}
	}
}
