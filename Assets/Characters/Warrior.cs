﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MeleeAttacker {

	private float diveSpeed = 50f;
	private float diveTime = 0.25f;
	private float diveCooldown = 0.2f;

	private float diveAttackCost = 20f;

	protected override void Awake()
	{
		base.Awake();
		this.moveSpeed = 4f;
		this.attackWindUpTime = 0.2f;
		this.attackSwingTime = 0.3f;
		this.maxHP = 150f;
		this.maxSP = 50f;
		this.hpRegenRate = 0.6f;
		this.spRegenRate = 0.7f;
		this.characterName = "Warrior";
		this.attackField.weaponTransform.localScale = new Vector3(1.5f, 1f, 1f);
	}

	protected new void Update()
	{
		base.Update();

		if (this.device.Action2.WasPressed == true && this.attackingCoroutine == null)
		{
			this.attackingCoroutine = StartCoroutine(DiveAttack());
		}
	}

	private IEnumerator DiveAttack()
	{
		if (this.currentHP <= this.diveAttackCost)
		{
			this.attackingCoroutine = null;
			yield break;
		}

		this.currentHP -= this.diveAttackCost;

		this.attackField.gameObject.SetActive(true);

		for (float i = 0; i < this.diveTime; i += Time.deltaTime)
		{
			this.transform.Translate(this.transform.forward * Time.deltaTime * diveSpeed, Space.World);
			yield return null;
		}

		this.attackField.gameObject.SetActive(false);

		yield return new WaitForSeconds(this.diveCooldown);
		this.attackingCoroutine = null;
	}
}
