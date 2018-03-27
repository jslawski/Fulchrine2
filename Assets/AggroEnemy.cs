using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolarCoordinates;

public class AggroEnemy : Enemy {

	[SerializeField]
	private Weapon attackField;

	private float attackDistance = 0.8f;

	private float directionAdjustment = 0;

	private int attackWindUpFrames = 5;
	private int attackCooldownFrames = 50;

	// Use this for initialization
	void Start () {
		attackField.damageOutput = Random.Range(100, 200);
		attackField.enchantment = this.attackEnchantment;

		StartCoroutine(AdjustDirection());
	}

	// Update is called once per frame
	void Update () {
		if (this.GetDistanceFromPlayer(this.playerTarget.transform.position, this.transform.position) > this.attackDistance)
		{
			Vector3 directionVector = this.playerTarget.transform.position - this.transform.position;
			PolarCoordinate directionPolarCoordinate = new PolarCoordinate(directionVector, Orientation.XZ);
			directionPolarCoordinate.angle += (this.directionAdjustment * Mathf.Deg2Rad);
			directionVector = directionPolarCoordinate.PolarToCartesianTopDown();

			Vector2 directionVector2D = new Vector2(directionVector.x, directionVector.z);
			this.Move(directionVector2D);
		}
		else if (this.attacking == false)
		{			
			StartCoroutine(this.Attack());
		}
	}

	private IEnumerator AdjustDirection()
	{
		while (true)
		{
			this.directionAdjustment = Random.Range(-45f, 45f);
			yield return new WaitForSeconds(2);
		}
	}

	private IEnumerator Attack()
	{
		this.attacking = true;

		while (this.GetDistanceFromPlayer(this.playerTarget.transform.position, this.transform.position) <= this.attackDistance)
		{
			for (int i = 0; i < this.attackWindUpFrames; i++)
			{
				yield return null;
			}

			this.attackField.gameObject.SetActive(true);

			for (int i = 0; i < 5; i++)
			{
				yield return null;
			}

			this.attackField.gameObject.SetActive(false);

			for (int i = 0; i < this.attackCooldownFrames; i++)
			{
				yield return null;
			}
		}
			
		this.attacking = false;
	}
}
