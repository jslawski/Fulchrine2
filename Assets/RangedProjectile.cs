using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedProjectile : Weapon {

	protected float projectileSpeed;
	public Vector3 projectileDirection;

	protected void Awake()
	{
		this.projectileSpeed = 15f;
		this.damageOutput = 70f;

		Invoke("DestroyProjectile", 3f);
	}

	// Update is called once per frame
	void Update () {

		if (this.projectileDirection != null)
		{
			this.transform.Translate(this.projectileDirection * Time.deltaTime * this.projectileSpeed);
		}

	}

	private void OnTriggerStay(Collider other)
	{
		base.OnTriggerStay(other);

		if (other.tag == "Enemy" || other.tag == "Player")
		{
			Destroy(this.gameObject);
		}
	}

	private void DestroyProjectile()
	{
		Destroy(this.gameObject);
	}
}
