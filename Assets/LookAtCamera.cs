using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

	[SerializeField]
	private Camera camera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Vector3 viewportPoint = camera.WorldToViewportPoint(this.transform.position);
		Ray cameraRay = camera.ViewportPointToRay(viewportPoint);

		this.transform.LookAt(camera.transform, this.transform.up);
		this.transform.forward = -this.transform.forward;
	}
}
