using UnityEngine;
using System.Collections;

public class Debris : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	float totalTime = 0;
	// Update is called once per frame
	void Update () {

		totalTime += Time.deltaTime;

		if (totalTime > 5) {
						Destroy (gameObject);
				}
	
	}
}
