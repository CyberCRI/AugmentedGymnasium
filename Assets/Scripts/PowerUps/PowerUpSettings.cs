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
		[Tooltip("The type of the power-up")]
		/// <summary>
		/// The type of the power-up.
		/// </summary>
		public PowerUpType powerUpType;
		[Tooltip("Has the power-up been allowed. If false, the power-up will never appear in-game.")]
		/// <summary>
		/// Has the power-up been allowed. If false, the power-up will never appear in-game.
		/// </summary>
		public bool allowed;
		[Tooltip("The frequency of the power-up. The higher the value, the higher the chances are of seeing this power-up.")]
		/// <summary>
		/// The frequency of the power-up. The higher the value, the higher the chances are of seeing this power-up.
		/// </summary>
		public float frequency;

		public AllowedPowerUp(PowerUpType powerUpType, bool allowed, float frequency)
		{
			this.powerUpType = powerUpType;
			this.allowed = allowed;
			this.frequency = frequency;
		}
	}

	public List<AllowedPowerUp> allowedPowerUps = new List<AllowedPowerUp> ();

	[Tooltip("Time until the power-up object disappears.")]
	/// <summary>
	/// Time until the power-up object disappears.
	/// </summary>
	public float powerUpTime;
	[Tooltip("The ratio by which the goal will be multiplicated when the size increases")]
	/// <summary>
	/// The ratio by which the goal will be multiplicated when the size increases.
	/// </summary>
	public float goalIncreaseRatio;
	[Tooltip("The ratio by which the goal will be dividated when the size decreases")]
	/// <summary>
	/// The ratio by which the goal will be dividated when the size decreases.
	/// </summary>
	public float goalDecreaseRatio;
	[Tooltip("How long the size of the goal will stay increased. If the time is negative or zero, it will stay increased.")]
	/// <summary>
	/// How long the size of the goal will stay increased. If the time is negative or zero, it will stay increased.
	/// </summary>
	public float goalIncreaseTime;
	[Tooltip("How long the size of the goal will stay decreased. If the time is negative or zero, it will stay decreased.")]
	/// <summary>
	/// How long the size of the goal will stay decreased. If the time is negative or zero, it will stay decreased.
	/// </summary>
	public float goalDecreaseTime;
	[Tooltip("The ratio by which the goal will be multiplicated when the size increases.")]
	/// <summary>
	/// The ratio by which the player will be multiplicated when the size increases.
	/// </summary>
	public float playerIncreaseRatio;
	[Tooltip("How long the player will stay increased. If the time is negative or zero, it will stay increased.")]
	/// <summary>
	/// How long the player will stay increased. If the time is negative or zero, it will stay increased.
	/// </summary>
	public float playerIncreaseTime;
	[Tooltip("How long the multi-goal status will last. If the time is negative or zero, it will last undefinitely.")]
	/// <summary>
	/// How long the multi-goal status will last. If the time is negative or zero, it will last undefinitely.
	/// </summary>
	public float multiGoalTime;


	void Awake()
	{
		allowedPowerUps = AllowedPowerUpInitialization ();
	}

	private List<AllowedPowerUp> AllowedPowerUpInitialization()
	{
		var powerUps = Enum.GetValues (typeof(PowerUpType)).Cast<PowerUpType> ();
		var res = new List<AllowedPowerUp> ();

		foreach (var powerUp in powerUps) {
			res.Add (new AllowedPowerUp (powerUp, true, 1.0f));
		}

		return res;
	}
}
