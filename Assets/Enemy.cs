using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : DamagableCharacter {

	private bool invulnerable = false;
	private int invulnerabilityFrames = 10;

	public bool defenseDebuffed = false;

	private float maxHP = 1000f;
	private float _currentHP;
	public float currentHP
	{
		get { return this._currentHP; }
		set 
		{ 
			this._currentHP = value;
			//Debug.LogError("Current HP: " + this._currentHP);
			if (this._currentHP <= 0)
			{
				Destroy(this.gameObject);
			}
		}
	}

	public void Awake()
	{
		this.armor = this.gameObject.GetComponent<Armor>();
		this.armor.damageBlocked = 30f;
		this.currentHP = this.maxHP;
	}

	public void TakeDamage(float attackDamage)
	{
		if (this.invulnerable == true || attackDamage <= 0)
		{
			return;
		}

		float calculatedDamageDone = this.CalculateDamage(attackDamage);
		if (calculatedDamageDone > 0)
		{
			this.currentHP -= calculatedDamageDone;
			StartCoroutine(this.TriggerInvulnerabilityFrames());
		}
	}

	private IEnumerator TriggerInvulnerabilityFrames()
	{
		MeshRenderer enemyRenderer = this.gameObject.GetComponent<MeshRenderer>();

		this.invulnerable = true;
		for (int i = 0; i < this.invulnerabilityFrames; i++)
		{

				if (enemyRenderer.material.name == "Damaged (Instance)")
				{
					enemyRenderer.material = Resources.Load<Material>("Materials/Enemy");
				}
				else
				{
					enemyRenderer.material = Resources.Load<Material>("Materials/Damaged");
				}

			yield return null;
		}

		enemyRenderer.material = Resources.Load<Material>("Materials/Enemy");

		this.invulnerable = false;
	}
}
