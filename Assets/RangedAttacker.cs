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
		rangedProjectile = Resources.Load("RangedProjectile") as GameObject;
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
				newRangedProjectile.projectileDirection = this.gameObject.transform.forward;
				this.currentFireCooldown = 0;
			}

			yield return null;
		}
			
		this.attackingCoroutine = null;
	}
}
