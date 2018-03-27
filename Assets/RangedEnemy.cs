using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolarCoordinates;

public class RangedEnemy : Enemy {

	private GameObject rangedProjectile;
	private float fireRate = 0.75f;
	private float attackRange = 5f;
	private float retreatBuffer = 3f;
	private float pursueBuffer = 3f;
	private float directionAdjustment;

	private void Start () 
	{
		this.moveSpeed = 4f;
		this.rangedProjectile = Resources.Load("EnemyRangedProjectile") as GameObject;

		this.retreatBuffer = Random.Range(3f, 5f);

		StartCoroutine(AdjustDirection());
		StartCoroutine(PursuePlayer());
	}

	private void MoveInDirection(Vector3 startPoint, Vector3 targetPoint)
	{
		Vector3 directionVector = targetPoint - startPoint;
		PolarCoordinate directionPolarCoordinate = new PolarCoordinate(directionVector, Orientation.XZ);
		directionPolarCoordinate.angle += (this.directionAdjustment * Mathf.Deg2Rad);
		directionVector = directionPolarCoordinate.PolarToCartesianTopDown();

		Vector2 directionVector2D = new Vector2(directionVector.x, directionVector.z);
		this.Move(directionVector2D);
	}

	private IEnumerator Retreat() {
		//Retreat if player gets too close
		while (true)
		{
			this.MoveInDirection(this.playerTarget.transform.position, this.transform.position);

			if (this.GetDistanceFromPlayer(this.playerTarget.transform.position, this.transform.position) > this.attackRange)
			{
				StartCoroutine(this.PursuePlayer());
				yield break;
			}

			yield return null;
		}
	}

	private IEnumerator PursuePlayer()
	{
		while (true)
		{
			this.MoveInDirection(this.transform.position, this.playerTarget.transform.position);

			if (this.GetDistanceFromPlayer(this.playerTarget.transform.position, this.transform.position) <= this.attackRange)
			{
				StartCoroutine(this.Attack());
				yield break;
			}

			yield return null;
		}
	}

	private IEnumerator Attack()
	{
		while (true)
		{
			for (float i = 0; i < this.fireRate; i += Time.deltaTime)
			{
				if (this.GetDistanceFromPlayer(this.playerTarget.transform.position, this.transform.position) >= 0.5f)
				{
					this.gameObject.transform.forward = this.playerTarget.transform.position - this.transform.position;
				}

				yield return null;
			}

			float playerDistance = this.GetDistanceFromPlayer(this.playerTarget.transform.position, this.transform.position);
			if (playerDistance < (this.attackRange - this.retreatBuffer))
			{
				StartCoroutine(this.Retreat());
				yield break;
			}
			else if (playerDistance > (this.attackRange + this.pursueBuffer))
			{
				StartCoroutine(this.PursuePlayer());
				yield break;
			}

			GameObject newRangedProjectileObject;
			newRangedProjectileObject = GameObject.Instantiate(this.rangedProjectile, this.transform.position, new Quaternion()) as GameObject;
			RangedProjectile newRangedProjectile = newRangedProjectileObject.GetComponent<RangedProjectile>();
			newRangedProjectile.projectileDirection = this.gameObject.transform.forward;

			yield return null;
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

}
