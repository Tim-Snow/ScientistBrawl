using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {



	private Camera 	camera;
	private Rect	playerContainer;

	void Start () {
		camera = GetComponent<Camera> ();
	}

	void FixedUpdate () {
		//camera.transform.position = new Vector3 ();


	}
}
