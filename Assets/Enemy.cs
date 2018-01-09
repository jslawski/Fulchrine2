using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private bool invulnerable = false;
	private int invulnerabilityFrames = 10;

	private float maxHP = 100f;
	private float _currentHP;
	public float currentHP
	{
		get { return this._currentHP; }
		set 
		{ 
			this._currentHP = value;
			if (this._currentHP <= 0)
			{
				Destroy(this.gameObject);
			}
		}
	}

	public void TakeDamage(float attackDamage)
	{
		if (this.invulnerable == true)
		{
			return;
		}

		this.currentHP -= attackDamage;
	}

	private IEnumerator TriggerInvulnerabilityFrames()
	{
		this.invulnerable = true;
		for (int i = 0; i < this.invulnerabilityFrames; i++)
		{
			yield return null;
		}

		this.invulnerable = false;
	}
}
