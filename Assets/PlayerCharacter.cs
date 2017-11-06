using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerCharacter : MonoBehaviour {

	protected InputDevice device;

	float moveSpeed = 5f;

	void Awake()
	{
		this.device = InputManager.Devices[0];
	}

	// Use this for initialization
	void Start() {
		CameraFollow.instance.SetPointOfInterest(this.gameObject);
	}
	
	// Update is called once per frame
	void Update() {
		if (this.device.LeftStick.Vector.magnitude > 0)
		{
			this.Move(this.device.LeftStick.Vector);
		}
	}

	void Move(Vector2 moveVector)
	{	
		Vector3 moveDirection = new Vector3(moveVector.x, 0, moveVector.y);
		this.gameObject.transform.Translate(moveDirection * Time.deltaTime * this.moveSpeed);
	}
}
