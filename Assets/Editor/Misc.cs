using UnityEditor;
using UnityEngine;
using System.Collections;

public static class Misc {
	
	[MenuItem("Misc/Save All Changes to Disk")]
	static void SaveAllChangesToDisk() {

		// UNITY OFTEN HAS SEVERAL HIDDEN META FILES WITH UNSAVED CHANGES
		// OPEN DURING NORMAL OPERATION WHICH NEED TO BE FLUSHED TO DISK
		// BEFORE COMMITTING CHANGES TO AN EXTERNAL VERSION CONTROL TOOL.
		AssetDatabase.SaveAssets();
		
	}
	
}

