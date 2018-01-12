using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamagableCharacter : MonoBehaviour {

	protected Armor armor;
	public Enchantment defenseEnchantment;

	private bool invulnerable = false;

	public abstract float currentHP { get; set; }

	protected int invulnerabilityFrames = 0;

	protected Material defaultMaterial;

	public float CalculateDamage(float attackDamage, Enchantment enchantment)
	{
		float finalDamageBlocked = armor.damageBlocked;
		float finalDamageInflicted = attackDamage;

		Buff[] allBuffs = this.gameObject.GetComponents<Buff>();

		foreach (Buff buff in allBuffs)
		{
			finalDamageInflicted += buff.attackBuff;
			finalDamageBlocked += buff.defenseBuff;
		}

		finalDamageInflicted = EnchantmentManager.ApplyEnchantmentModifier(finalDamageInflicted, enchantment, this.defenseEnchantment);

		Debug.LogError("Base Attack: " + attackDamage + " After Enchantment Attack: " + finalDamageInflicted + "Damage Blocked: " + finalDamageBlocked + " Damaged Inflicted: " + (finalDamageInflicted - finalDamageBlocked));

		//Debug.LogError("Base Armor Class: " + armor.damageBlocked + " Armor Class after Defense Debuff: " + finalDamageBlocked + " Damage done: " + (finalDamageInflicted - finalDamageBlocked));

		return finalDamageInflicted - finalDamageBlocked;
	}

	public void TakeDamage(float attackDamage, Enchantment attackEnchantment)
	{
		if (this.invulnerable == true || attackDamage <= 0)
		{
			return;
		}

		float calculatedDamageDone = this.CalculateDamage(attackDamage, attackEnchantment);
		if (calculatedDamageDone > 0)
		{
			this.currentHP -= calculatedDamageDone;
			StartCoroutine(this.TriggerInvulnerabilityFrames());
		}
	}

	private IEnumerator TriggerInvulnerabilityFrames()
	{
		MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

		this.invulnerable = true;
		for (int i = 0; i < this.invulnerabilityFrames; i++)
		{

			if (renderer.material.name == "Damaged (Instance)")
			{
				renderer.material = this.defaultMaterial;
			}
			else
			{
				renderer.material = Resources.Load<Material>("Materials/Damaged");
			}

			yield return null;
		}

		renderer.material = this.defaultMaterial;

		this.invulnerable = false;
	}
}
