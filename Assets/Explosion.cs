using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Weapon {

	float explosionDuration = 0.3f;


	private void Awake()
	{
		Invoke("DestroyExplosion", this.explosionDuration);
	}

	private void DestroyExplosion()
	{
		Destroy(this.gameObject);
	}

	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
		if (other.tag == "Enemy")
		{
			//TODO: Do sum damage boiii
		}
	}
}
