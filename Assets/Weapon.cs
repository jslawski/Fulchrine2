using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	//Add attack stats and such here
	public Transform weaponTransform;

	private void Awake()
	{
		this.weaponTransform = this.gameObject.GetComponent<Transform>();
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			Debug.LogError("I got 'em!");
		}
	}
}
