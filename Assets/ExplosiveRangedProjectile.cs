using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveRangedProjectile : RangedProjectile {

	[SerializeField]
	private GameObject explosionGameObject;

	protected new void Awake()
	{
		this.projectileSpeed = 30f;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy")
		{
			this.GenerateExplosion();
		}
	}

	private void GenerateExplosion()
	{
		GameObject.Instantiate(this.explosionGameObject, this.transform.position, new Quaternion());
		Destroy(this.gameObject);
	}
}
