using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Enchantment {Fire, Ice, None};

public static class EnchantmentManager {

	public static float ApplyEnchantmentModifier(float baseAttackDamage, Enchantment attackEnchantment, Enchantment defenseEnchantment)
	{
		float finalAttackDamage = baseAttackDamage;
		if (attackEnchantment == null)
		{
			return finalAttackDamage;
		}

		if (defenseEnchantment == null)
		{
			finalAttackDamage += Mathf.Ceil(0.1f * baseAttackDamage);
		}
		else if (attackEnchantment == defenseEnchantment)
		{
			finalAttackDamage -= Mathf.Ceil(0.25f * baseAttackDamage);
		}
		else
		{
			finalAttackDamage += Mathf.Ceil(0.25f * baseAttackDamage);
		}

		return finalAttackDamage;
	}

	private static Color GetColor(Enchantment enchantment)
	{
		switch (enchantment)
		{
			case Enchantment.Fire:
				return Color.red;
			case Enchantment.Ice:
				return Color.blue;
			default:
				return Color.white;
		}
	}

	public static void SetIconColors(Image swordIcon, Enchantment attackEnchantment, Image shieldIcon, Enchantment defenseEnchantment)
	{	
		swordIcon.color = EnchantmentManager.GetColor(attackEnchantment);
		shieldIcon.color = EnchantmentManager.GetColor(defenseEnchantment);
	}
}
