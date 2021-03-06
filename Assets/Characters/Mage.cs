﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MageAttacker {

	private float healSPRate = 0.02f;
	private float healPartyHPRate = 0.05f;

	protected override void Awake()
	{
		base.Awake();
		this.moveSpeed = 9f;
		this.attackWindUpTime = 0.5f;
		this.maxHP = 750f;
		this.maxSP = 1500f;
		this.hpRegenRate = 0.5f;
		this.spRegenRate = 0.5f;
		this.characterName = "Mage";
		this.armor.damageBlocked = 10f;
		this.attackEnchantment = Enchantment.None;
		this.defenseEnchantment = Enchantment.Fire;
	}

	protected new void Update()
	{
		base.Update();

		if (this.device.Action2.WasPressed == true && this.attackingCoroutine == null)
		{
			this.attackingCoroutine = StartCoroutine(HealAttack());
		}
	}

	private IEnumerator HealAttack()
	{
		StartCoroutine(this.DrainSPHeal());
		StartCoroutine(this.AddHPToParty());

		while (this.device.Action2.IsPressed)
		{
			if (this.currentSP <= 0)
			{
				this.attackingCoroutine = null;
				yield break;
			}

			this.Strafe(this.device.LeftStick.Vector);
			yield return null;
		}
		this.attackingCoroutine = null;
	}

	private IEnumerator AddHPToParty()
	{
		while (this.device.Action2.IsPressed)
		{
			if (this.currentSP <= 0)
			{
				yield break;
			}

			StaticPlayerInfo.instance.HealParty(10.0f);
			yield return new WaitForSeconds(this.healPartyHPRate);
		}
	}

	private IEnumerator DrainSPHeal()
	{
		while (this.device.Action2.IsPressed)
		{
			if (this.currentSP <= 0)
			{
				yield break;
			}

			this.currentSP -= 10;
			yield return new WaitForSeconds(this.healSPRate);
		}
	}
}
