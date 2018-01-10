using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public float damageOutput;

	//Add attack stats and such here
	public Transform weaponTransform;

	private void Awake()
	{
		this.weaponTransform = this.gameObject.GetComponent<Transform>();
	}

	protected virtual void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			other.gameObject.GetComponent<Enemy>().TakeDamage(damageOutput);
		}
	}
}
