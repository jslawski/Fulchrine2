using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public float damageOutput;

	//Add attack stats and such here
	public Transform weaponTransform;

	public Enchantment enchantment;

	private void Awake()
	{
		this.weaponTransform = this.gameObject.GetComponent<Transform>();
	}

	protected virtual void OnTriggerStay(Collider other)
	{
		Debug.LogError("In on trigger stay");
		if (other.tag == "Enemy" || other.tag == "Player")
		{
			Debug.LogError("Collided");
			other.gameObject.GetComponent<DamagableCharacter>().TakeDamage(damageOutput, enchantment);
		}
	}
}
