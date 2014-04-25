using UnityEngine;
using System.Collections;

public class TaskMaster : MonoBehaviour {

	internal static TaskMaster inst;
	
	public static TaskMaster GetInstance() { 
		if (inst == null) {
			new GameObject("TaskMaster", typeof(TaskMaster));
		}
		return inst;
	}
	
	void Awake() {
		inst = this;
	}
	
	void OnDestroy() {
		if (inst == this) { inst = null; }
	}
	
}
