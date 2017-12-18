using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MeleeAttacker {

	public GameObject spinAttackObject;

	private float spinAttackWindUpTime = 0.75f;
	private float spinAttackDuration = 3f;

	private float normalMoveSpeed = 2f;
	private float spinAttackMoveSpeed = 0.5f;

	protected override void Awake()
	{
		base.Awake();
		this.moveSpeed = this.normalMoveSpeed;
		this.attackWindUpTime = 0.5f;
		this.attackSwingTime = 0.2f;
		this.attackField.weaponTransform.localScale = new Vector3(4f, 1f, 1f);
		this.spinAttackObject = Resources.Load("SpinAttack") as GameObject;
	}

	protected new void Update()
	{
		base.Update();

		if (this.device.Action2.WasPressed == true && this.attackingCoroutine == null)
		{
			this.attackingCoroutine = StartCoroutine(this.SpinAttack());
		}
	}

	private IEnumerator SpinAttack()
	{
		yield return new WaitForSeconds(this.spinAttackWindUpTime);

		GameObject currentSpinAttack = GameObject.Instantiate(this.spinAttackObject, this.transform.position, new Quaternion()) as GameObject;
		this.moveSpeed = spinAttackMoveSpeed;

		for (float i = 0; i < this.spinAttackDuration; i += Time.deltaTime)
		{
			if (this.device.LeftStick.Vector.magnitude > 0)
			{
				this.Move(this.device.LeftStick.Vector);
				currentSpinAttack.transform.position = this.transform.position;
			}

			yield return null;
		}

		Destroy(currentSpinAttack);
		this.moveSpeed = this.normalMoveSpeed;
		this.attackingCoroutine = null;
	}
}
