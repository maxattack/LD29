using System.Collections;
using UnityEditor;
using UnityEngine;

public class SfxPostprocessor : AssetPostprocessor {
	void OnPreprocessAudio() {
		if (assetPath.Contains("/SFX/")) {
			var audioImporter = assetImporter as AudioImporter;
			audioImporter.threeD = false;
		}
	}
}
