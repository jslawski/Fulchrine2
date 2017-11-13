using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : PlayerCharacter {

	protected override void Awake()
	{
		this.moveSpeed = 5f;
		base.Awake();
	}
}
