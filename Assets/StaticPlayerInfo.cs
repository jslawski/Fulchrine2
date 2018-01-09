using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StaticPlayerInfo : MonoBehaviour {

	public static StaticPlayerInfo instance;

	private Dictionary<string, Dictionary<string, float>> characterInfo;

	//Dictionary Keys
	const string CurrentHPKey = "CurrentHP";
	const string CurrentSPKey = "CurrentSP";
	const string MaxHPKey = "MaxHP";
	const string MaxSPKey = "MaxSP";
	const string HPRegenRateKey = "HPRegenRate";
	const string SPRegenRateKey = "SPRegenRate";
	const string DeadKey = "IsDead";

	public delegate void HPRegen();
	public static event HPRegen OnHPRegen;
	public delegate void SPRegen();
	public static event SPRegen OnSPRegen;

	private void Awake()
	{
		if (StaticPlayerInfo.instance == null)
		{
			StaticPlayerInfo.instance = this;
		}

		this.characterInfo = new Dictionary<string, Dictionary<string, float>>();
	}

	public bool CharacterInfoExists(string character)
	{
		return (this.characterInfo.ContainsKey(character));
	}

	public void SetInitialInfo(PlayerCharacter character)
	{
		Dictionary<string, float> newDictionary = new Dictionary<string, float>();

		newDictionary.Add(StaticPlayerInfo.MaxHPKey, character.maxHP);
		newDictionary.Add(StaticPlayerInfo.MaxSPKey, character.maxSP);
		newDictionary.Add(StaticPlayerInfo.CurrentHPKey, character.maxHP);
		newDictionary.Add(StaticPlayerInfo.CurrentSPKey, character.maxSP);
		newDictionary.Add(StaticPlayerInfo.HPRegenRateKey, character.hpRegenRate);
		newDictionary.Add(StaticPlayerInfo.SPRegenRateKey, character.spRegenRate);
		newDictionary.Add(StaticPlayerInfo.DeadKey, 0);

		this.characterInfo.Add(character.characterName, newDictionary);

		StartCoroutine(this.RegenHP(character));
		StartCoroutine(this.RegenSP(character));
	}

	public void SetCharacterHP(string character, float currentHP)
	{
		this.characterInfo[character][StaticPlayerInfo.CurrentHPKey] = currentHP;
	}

	public float GetCharacterHP(string character)
	{
		return this.characterInfo[character][StaticPlayerInfo.CurrentHPKey];
	}

	public void SetCharacterSP(string character, float currentSP)
	{
		this.characterInfo[character][StaticPlayerInfo.CurrentSPKey] = currentSP;
	}

	public float GetCharacterSP(string character)
	{
		return this.characterInfo[character][StaticPlayerInfo.CurrentSPKey];
	}

	public void SetDeadState(string character, bool isDead)
	{
		this.characterInfo[character][StaticPlayerInfo.DeadKey] = Convert.ToSingle(isDead);
	}

	public bool GetDeadState(string character)
	{
		return Convert.ToBoolean(this.characterInfo[character][StaticPlayerInfo.DeadKey]);
	}

	public void HealParty(float amount)
	{
		foreach (KeyValuePair<string, Dictionary<string, float>> character in this.characterInfo)
		{
			if (character.Value[StaticPlayerInfo.CurrentHPKey] < character.Value[StaticPlayerInfo.MaxHPKey] && !Convert.ToBoolean(character.Value[StaticPlayerInfo.DeadKey]))
			{
				character.Value[StaticPlayerInfo.CurrentHPKey] += 1;
				StaticPlayerInfo.OnHPRegen();
			}
		}
	}

	private IEnumerator RegenHP(PlayerCharacter character)
	{
		Dictionary<string, float> characterStats = this.characterInfo[character.characterName];

		while (!this.GetDeadState(character.characterName))
		{
			if (characterStats[StaticPlayerInfo.CurrentHPKey] < characterStats[StaticPlayerInfo.MaxHPKey])
			{
				characterStats[StaticPlayerInfo.CurrentHPKey] += 1;
				if (StaticPlayerInfo.OnHPRegen != null)
				{
					StaticPlayerInfo.OnHPRegen();
				}
			}

			yield return new WaitForSeconds(characterStats[StaticPlayerInfo.HPRegenRateKey]);
		}
	}

	private IEnumerator RegenSP(PlayerCharacter character)
	{
		Dictionary<string, float> characterStats = this.characterInfo[character.characterName];

		while (!this.GetDeadState(character.characterName))
		{
			if (characterStats[StaticPlayerInfo.CurrentSPKey] < characterStats[StaticPlayerInfo.MaxSPKey])
			{
				characterStats[StaticPlayerInfo.CurrentSPKey] += 1;
				if (StaticPlayerInfo.OnSPRegen != null)
				{
					StaticPlayerInfo.OnSPRegen();
				}
			}

			yield return new WaitForSeconds(characterStats[StaticPlayerInfo.SPRegenRateKey]);
		}
	}
}
