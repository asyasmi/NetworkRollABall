using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

	public const int maxHealth = 100;

	[SyncVar]
	public int currentHealth = maxHealth;

	[Command]
	public void CmdTakeDamage(int amount){
		currentHealth -= amount;
		if (currentHealth <= 0)
		{
			currentHealth = 0;
			Debug.Log("Dead!");
		}
	}
}
