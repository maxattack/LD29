using System.Collections;
using UnityEngine;

public class StatsGUI : CustomBehaviour {
	
	void OnGUI() {
		GUI.Label(Rectangle(8, 8, 256, 256), string.Format("DT(ms) = {0}", Mathf.FloorToInt(1000 * Time.deltaTime)));
	}
	
}
