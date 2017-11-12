using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : PlayerCharacter {

	protected override void Awake()
	{
		this.moveSpeed = 3f;
		base.Awake();
	}
}
