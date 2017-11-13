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

	private Coroutine characterSelectorCoroutine;
	private float selectionThreshold = 0.1f;

	protected virtual void Awake()
	{
		this.selectionTextObjects = new GameObject[4];

		this.device = InputManager.Devices[0];

		this.selectionTextObjects[0] = GameObject.Find("Tank");
		this.selectionTextObjects[1] = GameObject.Find("Warrior");
		this.selectionTextObjects[2] = GameObject.Find("Archer");
		this.selectionTextObjects[3] = GameObject.Find("Mage");


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
				ChangeCharacter("Mage");
			} 
			else if (this.device.DPad.Y > 0)
			{
				ChangeCharacter("Warrior");
			}
		}
		if (this.device.DPad.Y == 0)
		{
			if (this.device.DPad.X < 0)
			{
				ChangeCharacter("Archer");
			} 
			else if (this.device.DPad.X > 0)
			{
				ChangeCharacter("Tank");
			}
		}
	}

	private void AnalogCharacterSelection()
	{
		if (this.device.RightStick.Vector.magnitude > this.selectionThreshold && this.characterSelectorCoroutine == null)
		{
			this.characterSelectorCoroutine = StartCoroutine(this.SelectCharacter());
			this.HighlightSelection();	
		}
	}

	IEnumerator SelectCharacter()
	{
		string characterSelection = string.Empty;
		while (this.device.RightStick.Vector.magnitude > this.selectionThreshold)
		{
			characterSelection = this.HighlightSelection();
			yield return null;
		}

		this.ChangeCharacter(characterSelection);
		this.characterSelectorCoroutine = null;

		for (int i = 0; i < this.selectionTextObjects.Length; i++)
		{
			Text thisText = this.selectionTextObjects[i].GetComponent<Text>();
			thisText.color = new Color(thisText.color.r, thisText.color.g, thisText.color.b, 0);
		}
	}

	private string HighlightSelection()
	{
		PolarCoordinate polarDirection = new PolarCoordinate(this.device.RightStick.Vector);
		float directionAngle = polarDirection.angle * Mathf.Rad2Deg;
		int selectionTextIndex = -1;
		Material selectionMaterial = Resources.Load("Materials/char1") as Material;
		string characterSelection = string.Empty;

		if (directionAngle > 315f || directionAngle < 45f)
		{
			selectionTextIndex = 0;
			selectionMaterial = Resources.Load("Materials/char4") as Material;
			characterSelection = "Tank";
		}
		else if (directionAngle > 45f && directionAngle < 135f)
		{
			selectionTextIndex = 1;
			selectionMaterial = Resources.Load("Materials/char1") as Material;
			characterSelection = "Warrior";
		}
		else if (directionAngle > 135f && directionAngle < 225f)
		{
			selectionTextIndex = 2;
			selectionMaterial = Resources.Load("Materials/char2") as Material;
			characterSelection = "Archer";
		}
		else if (directionAngle > 225f && directionAngle < 315f)
		{
			selectionTextIndex = 3;
			selectionMaterial = Resources.Load("Materials/char3") as Material;
			characterSelection = "Mage";
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

		return characterSelection;
	}

	private void ChangeCharacter(string characterType)
	{
		Material characterMaterial;

		Destroy(this);

		switch (characterType)
		{
			case "Warrior":
				characterMaterial = Resources.Load("Materials/char1") as Material;
				this.gameObject.AddComponent<Warrior>();
				break;
			case "Archer":
				characterMaterial = Resources.Load("Materials/char2") as Material;
				this.gameObject.AddComponent<Archer>();
				break;
			case "Mage":
				characterMaterial = Resources.Load("Materials/char3") as Material;
				this.gameObject.AddComponent<Mage>();
				break;
			case "Tank":
				characterMaterial = Resources.Load("Materials/char4") as Material;
				this.gameObject.AddComponent<Tank>();
				break;
			default:
				characterMaterial = Resources.Load("Materials/char1") as Material;
				break;
		}

		this.gameObject.GetComponent<Renderer>().material = characterMaterial;
	}
}
