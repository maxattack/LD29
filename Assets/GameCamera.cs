using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

    public Transform playerTransform;

	// Use this for initialization
	void Start () {
	


	}
	
	// Update is called once per frame
	void Update () {

        transform.position = playerTransform.position + new Vector3(10, 10, 10);

        transform.LookAt(playerTransform.position);

	}
}
