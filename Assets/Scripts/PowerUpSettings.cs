using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

[CreateAssetMenu (fileName = "Power Up Settings", menuName = "Data/Settings/Power Up Settings", order = 22)]
/// <summary>
/// All the settings for the powerups.
/// </summary>
public class PowerUpSettings : ScriptableObject
{
	[System.Serializable]
	public class AllowedPowerUp {
		public PowerUpType powerUpType;
		public bool allowed;

		public AllowedPowerUp(PowerUpType powerUpType, bool allowed)
		{
			this.powerUpType = powerUpType;
			this.allowed = allowed;
		}
	}

	public List<AllowedPowerUp> allowedPowerUps = new List<AllowedPowerUp> ();
	public float goalIncreaseRatio;
	public float goalDecreaseRatio;

	void Awake()
	{
		allowedPowerUps = AllowedPowerUpInitialization ();
	}

	private List<AllowedPowerUp> AllowedPowerUpInitialization()
	{
		var powerUps = Enum.GetValues (typeof(PowerUpType)).Cast<PowerUpType> ();
		var res = new List<AllowedPowerUp> ();

		foreach (var powerUp in powerUps) {
			res.Add (new AllowedPowerUp (powerUp, true));
		}

		return res;
	}
}
