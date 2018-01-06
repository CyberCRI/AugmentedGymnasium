using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace AugmentedGymnasium
{
	public class PowerUp : MonoBehaviour
	{
		public delegate void PowerUpEvent (Player Player, PowerUp powerUp);

		public static event PowerUpEvent onPlayerGetPowerUp;

		[Tooltip ("The type of the power-up.")]
		/// <summary>
	/// The type of the power-up.
	/// </summary>
	public PowerUpType powerUpType;

		private bool _destroyed;

		void OnTriggerEnter2D (Collider2D col)
		{
			if (!_destroyed && col.gameObject.tag == "Player") {
				if (onPlayerGetPowerUp != null)
					onPlayerGetPowerUp (col.gameObject.GetComponent<Player> (), this);
				Destroy (this.gameObject);
				_destroyed = true;
			}
		}
	}
}