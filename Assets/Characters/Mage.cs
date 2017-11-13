using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : PlayerCharacter {

	protected override void Awake()
	{
		this.moveSpeed = 9f;
		base.Awake();
	}
}
