using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffBeam : Weapon {

	private string debuffName = "DefenseDebuff";
	public Collider collider;
	private Buff currentBuff;

	private float debuffRate = 0.5f;
	private float debuffDuration = 5f;

	public void Awake()
	{
		this.collider = this.gameObject.GetComponent<Collider>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			Buff[] allBuffs = other.gameObject.GetComponents<Buff>();

			//Find existing debuff, if there is one
			int debuffIndex = -1;
			for (int i = 0; i < allBuffs.Length; i++)
			{
				if (allBuffs[i].buffName == this.debuffName)
				{
					this.currentBuff = allBuffs[i];
				}
			}

			if (this.currentBuff == null)
			{
				this.currentBuff = other.gameObject.AddComponent<Buff>() as Buff;
				this.currentBuff.buffDuration = this.debuffDuration;
			}

			Enemy collidedEnemy = other.gameObject.GetComponent<Enemy>();
			collidedEnemy.defenseDebuffed = true;

			StartCoroutine(this.ApplyDebuff(collidedEnemy));
		}
	}

	protected override void OnTriggerStay(Collider other)
	{
		return;
	}

	private void OnTriggerExit(Collider other)
	{
		other.gameObject.GetComponent<Enemy>().defenseDebuffed = false;
	}

	private IEnumerator ApplyDebuff(Enemy collidedEnemy)
	{
		while (collidedEnemy.defenseDebuffed == true)
		{
			currentBuff.defenseBuff -= 1;
			currentBuff.StartBuff();
			yield return new WaitForSeconds(this.debuffRate);
		}
	}
}
