using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MageAttacker {

	protected override void Awake()
	{
		this.moveSpeed = 9f;
		this.attackWindUpTime = 0.5f;
		base.Awake();
	}

	protected new void Update()
	{
		base.Update();

		if (this.device.Action2.WasPressed == true && this.attackingCoroutine == null)
		{
			this.attackingCoroutine = StartCoroutine(HealParty());
		}
	}

	private IEnumerator HealParty()
	{
		while (this.device.Action2.IsPressed)
		{
			//TODO: Do healing stuff
			this.Strafe(this.device.LeftStick.Vector);
			yield return null;
		}

		this.attackingCoroutine = null;
	}
}
