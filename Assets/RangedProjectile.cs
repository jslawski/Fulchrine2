using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedProjectile : MonoBehaviour {

	protected float projectileSpeed;
	public Vector3 projectileDirection;

	protected void Awake()
	{
		this.projectileSpeed = 15f;
	}

	// Update is called once per frame
	void Update () {

		if (this.projectileDirection != null)
		{
			this.transform.Translate(this.projectileDirection * Time.deltaTime * this.projectileSpeed);
		}

	}
}
