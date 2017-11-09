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
		this.Move(new Vector3(moveVector.x, 0, moveVector.y));
	}

	void Move(Vector3 moveVector)
	{	
		this.gameObject.transform.forward = moveVector;
		this.gameObject.transform.Translate(moveVector * Time.deltaTime * this.moveSpeed, Space.World);
	}
}
