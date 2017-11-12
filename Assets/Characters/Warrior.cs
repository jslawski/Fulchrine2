using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : PlayerCharacter {

	protected override void Awake()
	{
		this.moveSpeed = 5f;
		base.Awake();
	}
}
