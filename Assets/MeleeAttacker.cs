using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeAttacker : PlayerCharacter {

	//Attack variables
	[SerializeField]
	protected Weapon attackField;
	protected float attackWindUpTime;
	protected float attackSwingTime;
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
		yield return new WaitForSeconds(this.attackWindUpTime);
		this.attackField.gameObject.SetActive(true);
		yield return new WaitForSeconds(this.attackSwingTime);
		this.attackField.gameObject.SetActive(false);
		this.attackingCoroutine = null;
	}
}
