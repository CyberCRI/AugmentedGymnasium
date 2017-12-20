using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PowerUp : MonoBehaviour {
	[Tooltip("The type of the power-up.")]
	/// <summary>
	/// The type of the power-up.
	/// </summary>
	public PowerUpType powerUpType;

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player") {
			GameManager.instance.PowerUp (col.gameObject.GetComponent<Player> (), this.powerUpType);
			Destroy (this.gameObject);
		}
	}

	void Start()
	{
		this.powerUpType = Enum.GetValues (typeof(PowerUpType)).Cast<PowerUpType> ().PickRandom ();
	}
}
