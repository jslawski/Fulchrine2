using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MageAttacker : PlayerCharacter {

	//Attack variables
	[SerializeField]
	protected Weapon attackField;
	protected float attackWindUpTime;
	protected float attackSwingTime;
	protected Coroutine attackingCoroutine;

	private float strafeSpeed = 1.5f;

	private float attackSPRate = 0.05f; 

	protected override void Awake()
	{
		Weapon[] weaponList = this.gameObject.GetComponentsInChildren<Weapon>(true);
		this.attackField = weaponList[1];

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
		float currentCastTime = 0;

		StartCoroutine(DrainSPAttack());

		while (this.device.Action1.IsPressed)
		{
			if (this.currentSP <= 0)
			{
				this.attackField.gameObject.SetActive(false);
				this.attackingCoroutine = null;
				yield break;
			}

			if (this.device.LeftStick.Vector.magnitude > 0)
			{
				this.Strafe(this.device.LeftStick.Vector);
			}

			if (currentCastTime < this.attackWindUpTime)
			{
				currentCastTime += Time.deltaTime;
			}
			else
			{
				this.attackField.gameObject.SetActive(true);
			}

			yield return null;
		}

		this.attackField.gameObject.SetActive(false);
		this.attackingCoroutine = null;
	}

	protected void Strafe(Vector2 direction)
	{
		Vector3 direction3D = new Vector3(direction.x, 0, direction.y);
		this.Strafe(direction3D);
	}

	protected void Strafe(Vector3 direction)
	{
		this.gameObject.transform.Translate(direction * Time.deltaTime * this.strafeSpeed, Space.World);
	}

	protected IEnumerator DrainSPAttack()
	{
		while (this.device.Action1.IsPressed)
		{
			if (this.currentSP <= 0)
			{
				yield break;
			}

			this.currentSP -= 1;
			yield return new WaitForSeconds(this.attackSPRate);
		}
	}
}
