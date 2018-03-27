using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttacker : PlayerCharacter {
	//Attack variables
	protected Coroutine attackingCoroutine;
	protected GameObject rangedProjectile;
	private float currentFireCooldown;
	private float fireRate = 0.5f;

	protected override void Awake()
	{
		base.Awake();
		this.currentFireCooldown = fireRate;
		this.rangedProjectile = Resources.Load("RangedProjectile") as GameObject;
	}

	// Update is called once per frame
	protected new void Update() {
		if (this.device.LeftStick.Vector.magnitude > 0 && this.attackingCoroutine == null && this.currentFireCooldown >= this.fireRate)
		{
			this.Move(this.device.LeftStick.Vector);
		}

		if (this.device.Action1.WasPressed == true && this.attackingCoroutine == null)
		{
			this.attackingCoroutine = StartCoroutine(this.Attack());
		}

		if (this.currentFireCooldown < this.fireRate)
		{
			this.currentFireCooldown += Time.deltaTime;
		}

		this.DPadCharacterSelection();
		this.AnalogCharacterSelection();
	}

	private IEnumerator Attack()
	{
		while (this.device.Action1.IsPressed == true)
		{
			if (this.device.LeftStick.Vector.magnitude > 0)
			{
				this.Rotate(this.device.LeftStick.Vector.normalized);
			}

			if (this.currentFireCooldown >= this.fireRate)
			{
				GameObject newRangedProjectileObject;
				newRangedProjectileObject = GameObject.Instantiate(this.rangedProjectile, this.transform.position, new Quaternion()) as GameObject;
				RangedProjectile newRangedProjectile = newRangedProjectileObject.GetComponent<RangedProjectile>();
				newRangedProjectile.projectileDirection = this.GetFireDirection();
				this.currentFireCooldown = 0;
			}

			yield return null;
		}
			
		this.attackingCoroutine = null;
	}

	private Vector3 GetFireDirection()
	{
		//Enemy Layer only
		int layerMask = 1 << 9;

		RaycastHit[] potentialTargets = Physics.SphereCastAll(this.transform.position + (this.gameObject.transform.forward * 2f), 1f, this.gameObject.transform.forward, 15f, layerMask);

		float closestDistance = float.MaxValue;
		int closestIndex = int.MaxValue;

		for (int i = 0; i < potentialTargets.Length; i++)
		{
			Debug.LogError("yey");
			if (potentialTargets[i].distance < closestDistance)
			{
				closestDistance = potentialTargets[i].distance;
				closestIndex = i;
			}
		}

		if (closestIndex != int.MaxValue)
		{
			Vector3 direction = potentialTargets[closestIndex].collider.gameObject.transform.position - this.transform.position;
			this.transform.forward = direction;
			return direction.normalized;
		}
		else
		{
			return this.transform.forward.normalized;
		}
	}
}
