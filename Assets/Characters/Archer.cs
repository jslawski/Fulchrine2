using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolarCoordinates;

public class Archer : RangedAttacker {

	private float hailOfArrowsWindUpTime = 0.5f;
	private float numArrows = 5f;
	private float coneWidth = 60f * Mathf.Deg2Rad;

	private float explosiveProjectileWindUpTime = 0.75f;
	private float explosiveProjectileCoolOffTime = 0.25f;

	private GameObject explosiveProjectile;

	private float explosiveProjectileCost = 30f;

	protected override void Awake()
	{
		base.Awake();
		this.moveSpeed = 5f;

		this.maxHP = 120f;
		this.maxSP = 100f;
		this.hpRegenRate = 0.6f;
		this.spRegenRate = 0.7f;
		this.characterName = "Archer";

		this.explosiveProjectile = Resources.Load("ExplosiveProjectile") as GameObject;
	}

	protected new void Update()
	{
		base.Update();

		if (this.device.Action2.WasPressed == true && this.attackingCoroutine == null)
		{
			this.attackingCoroutine = StartCoroutine(this.LaunchExplosiveProjectile());
		}

		/*if (this.device.Action2.WasPressed == true && this.attackingCoroutine == null)
		{
			this.attackingCoroutine = StartCoroutine(this.HailOfArrows());
		}*/
	}

	private IEnumerator LaunchExplosiveProjectile()
	{
		if (this.currentSP < this.explosiveProjectileCost)
		{
			yield break;
		}

		this.currentSP -= this.explosiveProjectileCost;

		yield return new WaitForSeconds(this.explosiveProjectileWindUpTime);

		GameObject newRangedProjectileObject;
		newRangedProjectileObject = GameObject.Instantiate(this.explosiveProjectile, this.transform.position, new Quaternion()) as GameObject;
		RangedProjectile newRangedProjectile = newRangedProjectileObject.GetComponent<RangedProjectile>();
		newRangedProjectile.projectileDirection = this.transform.forward;

		yield return new WaitForSeconds(this.explosiveProjectileCoolOffTime);
		this.attackingCoroutine = null;
	}

	private IEnumerator HailOfArrows()
	{
		yield return new WaitForSeconds(this.hailOfArrowsWindUpTime);

		this.LaunchArrows();

		yield return new WaitForSeconds(this.hailOfArrowsWindUpTime);
		this.attackingCoroutine = null;
	}

	private void LaunchArrows()
	{
		PolarCoordinate currentForwardDirection = new PolarCoordinate(this.transform.forward, Orientation.XZ);
		float degreeIncrement = this.coneWidth / numArrows;
		float degreeStartingPos = currentForwardDirection.angle - (coneWidth / 2);

		for (float i = 0, j = degreeStartingPos; i < this.numArrows; i++, j += degreeIncrement)
		{
			Vector3 launchDirection = new PolarCoordinate(currentForwardDirection.radius, j).PolarToCartesianTopDown();

			GameObject newRangedProjectileObject;
			newRangedProjectileObject = GameObject.Instantiate(this.rangedProjectile, this.transform.position, new Quaternion()) as GameObject;
			RangedProjectile newRangedProjectile = newRangedProjectileObject.GetComponent<RangedProjectile>();
			newRangedProjectile.projectileDirection = launchDirection;
		}
	}
}
