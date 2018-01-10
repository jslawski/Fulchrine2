using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Weapon {

	float explosionDuration = 0.1f;

	private void Awake()
	{
		this.damageOutput = 500f;
		Invoke("DestroyExplosion", this.explosionDuration);
	}

	private void DestroyExplosion()
	{
		Destroy(this.gameObject);
	}
}
