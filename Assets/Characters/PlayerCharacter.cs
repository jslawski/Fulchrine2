using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;
using PolarCoordinates;

public abstract class PlayerCharacter : MonoBehaviour {

	protected InputDevice device;

	protected float moveSpeed;

	private GameObject[] selectionTextObjects;

	private Coroutine characterSelectorCoroutine;
	private float selectionThreshold = 0.1f;

	private Text hpText;
	private Text spText;

	public float maxHP;
	public float maxSP;
	protected float _currentHP;
	protected float _currentSP;
	public float hpRegenRate;
	public float spRegenRate;

	public string characterName = string.Empty;

	public float currentHP 
	{
		get 
		{ 
			return this._currentHP; 
		}
		set 
		{ 
			string hpString = "<color=green>";
			this._currentHP = value;
			StaticPlayerInfo.instance.SetCharacterHP(this.characterName, this._currentHP);

			if (this._currentHP < (maxHP / 2))
			{
				hpString = "<color=orange>";
			}

			hpString += "HP: " + this._currentHP + "</color>";
			this.hpText.text = hpString;

			if (this._currentHP <= 0)
			{
				this._currentHP = 0;
				StaticPlayerInfo.instance.SetDeadState(this.characterName, true);
				this.ForceSwapCharacter();
			}
		}
	}

	public float currentSP 
	{
		get 
		{ 
			return _currentSP; 
		}
		set 
		{ 
			string spString = "<color=blue>";
			this._currentSP = value;
			StaticPlayerInfo.instance.SetCharacterSP(this.characterName, this._currentSP);

			if (this._currentSP < (maxSP / 2))
			{
				spString = "<color=black>";
			}

			spString += "SP: " + this._currentSP + "</color>";
			this.spText.text = spString; 
		}
	}

	protected virtual void Awake()
	{
		Text[] texts = this.gameObject.GetComponentsInChildren<Text>();
		this.hpText = texts[0];
		this.spText = texts[1];

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

		if (StaticPlayerInfo.instance.CharacterInfoExists(this.characterName) == false)
		{
			StaticPlayerInfo.instance.SetInitialInfo(this);
		}

		this.currentHP = StaticPlayerInfo.instance.GetCharacterHP(this.characterName);
		this.currentSP = StaticPlayerInfo.instance.GetCharacterSP(this.characterName);

		StaticPlayerInfo.OnHPRegen += this.OnHPRegen;
		StaticPlayerInfo.OnSPRegen += this.OnSPRegen;
	}

	private void OnDestroy()
	{
		StaticPlayerInfo.OnHPRegen -= this.OnHPRegen;
		StaticPlayerInfo.OnSPRegen -= this.OnSPRegen;
	}

	protected void SetInitialValues()
	{
		StaticPlayerInfo.instance.SetCharacterHP(this.characterName, this.maxHP);
		StaticPlayerInfo.instance.SetCharacterSP(this.characterName, this.maxSP);
	}

	// Update is called once per frame
	protected void Update() {
		if (this.device.LeftStick.Vector.magnitude > 0)
		{
			this.Move(this.device.LeftStick.Vector);
		}

		this.DPadCharacterSelection();
		this.AnalogCharacterSelection();
	}

	protected void Move(Vector2 moveVector)
	{
		this.Move(new Vector3(moveVector.x, 0, moveVector.y));
	}

	private void Move(Vector3 moveVector)
	{	
		this.gameObject.transform.forward = moveVector;
		this.gameObject.transform.Translate(moveVector * Time.deltaTime * this.moveSpeed, Space.World);
	}

	protected void Rotate(Vector2 forwardVector)
	{
		this.Rotate(new Vector3(forwardVector.x, 0, forwardVector.y));
	}

	private void Rotate(Vector3 forwardVector)
	{
		this.gameObject.transform.forward = forwardVector;
	}

	protected void DPadCharacterSelection()
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

	protected void AnalogCharacterSelection()
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

			string parsedCharacterName = selectionText.text.Substring(3, selectionText.text.Length - 7);
			if (StaticPlayerInfo.instance.CharacterInfoExists(parsedCharacterName) && StaticPlayerInfo.instance.GetDeadState(parsedCharacterName))
			{
				selectionText.color = Color.red;
			}
		}

		return characterSelection;
	}

	//Either pick a character that hasn't been selected yet, or the next "alive" character
	private void ForceSwapCharacter()
	{
		if (!StaticPlayerInfo.instance.CharacterInfoExists("Warrior") || !StaticPlayerInfo.instance.GetDeadState("Warrior"))
		{
			ChangeCharacter("Warrior");
		}
		else if (!StaticPlayerInfo.instance.CharacterInfoExists("Archer") || !StaticPlayerInfo.instance.GetDeadState("Archer"))
		{
			ChangeCharacter("Archer");
		}
		else if (!StaticPlayerInfo.instance.CharacterInfoExists("Mage") || !StaticPlayerInfo.instance.GetDeadState("Mage"))
		{
			ChangeCharacter("Mage");
		}
		else if (!StaticPlayerInfo.instance.CharacterInfoExists("Tank") || !StaticPlayerInfo.instance.GetDeadState("Tank"))
		{
			ChangeCharacter("Tank");
		}
		else
		{
			Debug.LogError("GAME OVER!");
		}
	}

	private void ChangeCharacter(string characterType)
	{
		if (StaticPlayerInfo.instance.CharacterInfoExists(characterType) && StaticPlayerInfo.instance.GetDeadState(characterType))
		{
			return;
		}

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

	private void OnHPRegen()
	{
		this.currentHP = StaticPlayerInfo.instance.GetCharacterHP(this.characterName);
	}

	private void OnSPRegen()
	{
		this.currentSP = StaticPlayerInfo.instance.GetCharacterSP(this.characterName);
	}
}
