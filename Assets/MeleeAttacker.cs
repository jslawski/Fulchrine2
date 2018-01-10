using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeAttacker : PlayerCharacter {

	//Attack variables
	[SerializeField]
	protected Weapon attackField;
	protected float attackWindUpFrames;
	protected float attackSwingFrames = 5f;
	protected float attackCooldownFrames;
	protected Coroutine attackingCoroutine;

	protected override void Awake()
	{
		Weapon[] weaponList = this.gameObject.GetComponentsInChildren<Weapon>(true);
		this.attackField = weaponList[0];

		foreach (Weapon weapon in weaponList)
		{
			weapon.gameObject.SetActive(false);
		}

		base.Awake();
	}

	// Update is called once per frame
	protected new void Update() {
		if (this.device.LeftStick.Vector.magnitude > 0 && this.attackingCoroutine == null)
		{
			this.Move(this.device.LeftStick.Vector);
		}

		if (this.device.Action1.WasPressed == true && this.attackingCoroutine == null)
		{
			this.attackingCoroutine = StartCoroutine(this.Attack());
		}

		this.DPadCharacterSelection();
		this.AnalogCharacterSelection();
	}
		
	private IEnumerator Attack()
	{
		//Wind Up
		for (int i = 0; i < this.attackWindUpFrames; i++)
		{
			yield return null;
		}
		this.attackField.gameObject.SetActive(true);

		for (int i = 0; i < this.attackSwingFrames; i++)
		{
			yield return null;
		}

		this.attackField.gameObject.SetActive(false);

		for (int i = 0; i < this.attackCooldownFrames; i++)
		{
			yield return null;
		}

		this.attackingCoroutine = null;
	}

	protected void OnDestroy()
	{	
		this.attackField.gameObject.SetActive(false);
	}
}
