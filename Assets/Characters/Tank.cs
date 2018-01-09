using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MeleeAttacker {

	public GameObject spinAttackObject;
	private GameObject currentSpinAttack;

	private float spinAttackWindUpTime = 0.75f;
	private float spinAttackDuration = 3f;

	private float normalMoveSpeed = 2f;
	private float spinAttackMoveSpeed = 0.5f;

	private float spinAttackCost = 20f;

	protected override void Awake()
	{
		base.Awake();
		this.moveSpeed = this.normalMoveSpeed;
		this.attackWindUpTime = 0.5f;
		this.attackSwingTime = 0.2f;
		this.maxHP = 250f;
		this.maxSP = 50f;
		this.hpRegenRate = 0.7f;
		this.spRegenRate = 0.7f;
		this.characterName = "Tank";
		this.attackField.weaponTransform.localScale = new Vector3(4f, 1f, 1f);
		this.spinAttackObject = Resources.Load("SpinAttack") as GameObject;
	}

	protected new void Update()
	{
		base.Update();

		if (this.device.Action2.WasPressed == true && this.attackingCoroutine == null)
		{
			this.attackingCoroutine = StartCoroutine(this.SpinAttack());
		}
	}

	private IEnumerator SpinAttack()
	{
		if (this.currentSP < this.spinAttackCost)
		{
			yield break;
		}

		this.currentSP -= this.spinAttackCost;

		yield return new WaitForSeconds(this.spinAttackWindUpTime);

		this.currentSpinAttack = GameObject.Instantiate(this.spinAttackObject, this.transform.position, new Quaternion()) as GameObject;
		this.moveSpeed = spinAttackMoveSpeed;

		for (float i = 0; i < this.spinAttackDuration; i += Time.deltaTime)
		{
			if (this.device.LeftStick.Vector.magnitude > 0)
			{
				this.Move(this.device.LeftStick.Vector);
				currentSpinAttack.transform.position = this.transform.position;
			}

			yield return null;
		}

		Destroy(this.currentSpinAttack);
		this.moveSpeed = this.normalMoveSpeed;
		this.attackingCoroutine = null;
	}

	protected new void OnDestroy()
	{
		base.OnDestroy();
		Destroy(this.currentSpinAttack);
	}
}
