using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : DamagableCharacter {

	private float moveSpeed = 3f;

	public bool defenseDebuffed = false;

	private float maxHP = 1000f;
	private float _currentHP;
	public override float currentHP
	{
		get { return this._currentHP; }
		set 
		{ 
			this._currentHP = value;
			//Debug.LogError("Current HP: " + this._currentHP);
			if (this._currentHP <= 0)
			{
				Destroy(this.gameObject);
			}
		}
	}

	public void Awake()
	{
		this.armor = this.gameObject.GetComponent<Armor>();
		this.armor.damageBlocked = 30f;
		this.currentHP = this.maxHP;
		this.invulnerabilityFrames = 10;
		this.defaultMaterial = this.gameObject.GetComponent<Renderer>().material;
	}

	protected void Move(Vector2 moveVector)
	{
		this.Move(new Vector3(moveVector.x, 0, moveVector.y));
	}

	private void Move(Vector3 moveVector)
	{	
		this.gameObject.transform.forward = moveVector.normalized;
		this.gameObject.transform.Translate(moveVector.normalized * Time.deltaTime * this.moveSpeed, Space.World);
	}

	protected void Rotate(Vector2 forwardVector)
	{
		this.Rotate(new Vector3(forwardVector.x, 0, forwardVector.y));
	}

	private void Rotate(Vector3 forwardVector)
	{
		this.gameObject.transform.forward = forwardVector;
	}

	protected float GetDistanceFromPlayer(Vector3 playerPosition, Vector3 enemyPosition)
	{
		return Mathf.Sqrt(Mathf.Pow((playerPosition.x - enemyPosition.x), 2) + Mathf.Pow((playerPosition.z - enemyPosition.z), 2));
	}
}
