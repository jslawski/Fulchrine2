using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;
using PolarCoordinates;

public abstract class PlayerCharacter : MonoBehaviour {

	private InputDevice device;

	protected float moveSpeed;

	private GameObject[] selectionTextObjects;

	protected virtual void Awake()
	{
		this.device = InputManager.Devices[0];

		this.selectionTextObjects = GameObject.FindGameObjectsWithTag("CharacterSelection");
	}

	// Use this for initialization
	private void Start() {
		CameraFollow.instance.SetPointOfInterest(this.gameObject);
	}
	
	// Update is called once per frame
	private void Update() {
		if (this.device.LeftStick.Vector.magnitude > 0)
		{
			this.Move(this.device.LeftStick.Vector);
		}
		this.DPadCharacterSelection();
		this.AnalogCharacterSelection();
	}

	private void Move(Vector2 moveVector)
	{
		this.Move(new Vector3(moveVector.x, 0, moveVector.y));
	}

	private void Move(Vector3 moveVector)
	{	
		this.gameObject.transform.forward = moveVector;
		this.gameObject.transform.Translate(moveVector * Time.deltaTime * this.moveSpeed, Space.World);
	}

	private void DPadCharacterSelection()
	{
		if (this.device.DPad.X == 0)
		{
			if (this.device.DPad.Y < 0)
			{
				Destroy(this);
				this.gameObject.AddComponent<Mage>();
				ChangeCharacter("Mage");
			} 
			else if (this.device.DPad.Y > 0)
			{
				Destroy(this);
				this.gameObject.AddComponent<Warrior>();
				ChangeCharacter("Warrior");
			}
		}
		if (this.device.DPad.Y == 0)
		{
			if (this.device.DPad.X < 0)
			{
				Destroy(this);
				this.gameObject.AddComponent<Archer>();
				ChangeCharacter("Archer");
			} 
			else if (this.device.DPad.X > 0)
			{
				Destroy(this);
				this.gameObject.AddComponent<Tank>();
				ChangeCharacter("Tank");
			}
		}
	}

	private void AnalogCharacterSelection()
	{
		if (this.device.RightStick.Vector.magnitude > 0)
		{
			this.HighlightSelection();	
		}
	}

	private void HighlightSelection()
	{
		PolarCoordinate polarDirection = new PolarCoordinate(this.device.RightStick.Vector);
		float directionAngle = polarDirection.angle * Mathf.Rad2Deg;
		int selectionTextIndex = -1;
		Material selectionMaterial = Resources.Load("Materials/char1") as Material;

		if (directionAngle > 315f || directionAngle < 45f)
		{
			selectionTextIndex = 0;
			selectionMaterial = Resources.Load("Materials/char4") as Material;
		}
		else if (directionAngle > 45f && directionAngle < 135f)
		{
			selectionTextIndex = 3;
			selectionMaterial = Resources.Load("Materials/char1") as Material;
		}
		else if (directionAngle > 135f && directionAngle < 225f)
		{
			selectionTextIndex = 1;
			selectionMaterial = Resources.Load("Materials/char2") as Material;
		}
		else if (directionAngle > 225f && directionAngle < 315f)
		{
			selectionTextIndex = 2;
			selectionMaterial = Resources.Load("Materials/char3") as Material;
		}

		for (int i = 0; i < this.selectionTextObjects.Length; i++)
		{
			Text selectionText = this.selectionTextObjects[i].GetComponent<Text>();
			if (i == selectionTextIndex)
			{
				selectionText.color = selectionMaterial.color;
			}
			else
			{
				selectionText.color = Color.black;
			}
		}
	}

	private void ChangeCharacter(string characterType)
	{
		Material characterMaterial;

		switch (characterType)
		{
			case "Warrior":
				characterMaterial = Resources.Load("Materials/char1") as Material;
				break;
			case "Archer":
				characterMaterial = Resources.Load("Materials/char2") as Material;
				break;
			case "Mage":
				characterMaterial = Resources.Load("Materials/char3") as Material;
				break;
			case "Tank":
				characterMaterial = Resources.Load("Materials/char4") as Material;
				break;
			default:
				characterMaterial = Resources.Load("Materials/char1") as Material;
				break;
		}

		this.gameObject.GetComponent<Renderer>().material = characterMaterial;
	}
}
