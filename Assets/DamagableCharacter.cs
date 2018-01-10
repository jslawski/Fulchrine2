using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableCharacter : MonoBehaviour {

	protected Armor armor;

	public float CalculateDamage(float attackDamage)
	{
		float finalDamageBlocked = armor.damageBlocked;
		float finalDamageInflicted = attackDamage;

		Buff[] allBuffs = this.gameObject.GetComponents<Buff>();

		foreach (Buff buff in allBuffs)
		{
			finalDamageInflicted += buff.attackBuff;
			finalDamageBlocked += buff.defenseBuff;
		}

		Debug.LogError("Base Armor Class: " + armor.damageBlocked + " Armor Class after Defense Debuff: " + finalDamageBlocked + " Damage done: " + (finalDamageInflicted - finalDamageBlocked));

		return finalDamageInflicted - finalDamageBlocked;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
