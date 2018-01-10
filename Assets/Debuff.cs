using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour {

	public float attackBuff = 0f;
	public float defenseBuff = 0f;
	public float buffDuration = 0f;

	public string buffName = string.Empty;

	public void StartBuff()
	{
		CancelInvoke();
		Invoke("StopBuff", this.buffDuration);
	}

	private void StopBuff()
	{
		Destroy(this);
	}
}
