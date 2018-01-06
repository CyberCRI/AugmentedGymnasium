using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace AugmentedGymnasium
{
	[CreateAssetMenu (fileName = "Power Up Settings", menuName = "Data/Settings/Power Up Settings", order = 22)]
	/// <summary>
/// All the settings for the powerups.
/// </summary>
public class PowerUpSettings : ScriptableObject
	{
		public List<PowerUpDescription> powerUpDescriptions = new List<PowerUpDescription> ();

		[Tooltip ("Time until the power-up object disappears.")]
		/// <summary>
	/// Time until the power-up object disappears.
	/// </summary>
	public float powerUpTime;
		[Tooltip ("Range of time until a new power-up appears.")]
		/// <summary>
	/// Range of time until a new power-up appears.
	/// </summary>
	public MinMax spawnTimeRange;
		[Tooltip ("The max number of power-up that could be on the board at the same time.")]
		/// <summary>
	/// The max number of power-ups that could be on the board at the same time.
	/// </summary>
	public int maxPowerUps;
		[Tooltip ("The ratio by which the goal will be multiplicated when the size increases")]
		/// <summary>
	/// The ratio by which the goal will be multiplicated when the size increases.
	/// </summary>
	public float goalIncreaseRatio;
		[Tooltip ("The ratio by which the goal will be dividated when the size decreases")]
		/// <summary>
	/// The ratio by which the goal will be dividated when the size decreases.
	/// </summary>
	public float goalDecreaseRatio;
		[Tooltip ("How long the size of the goal will stay increased. If the time is negative or zero, it will stay increased.")]
		/// <summary>
	/// How long the size of the goal will stay increased. If the time is negative or zero, it will stay increased.
	/// </summary>
	public float goalIncreaseTime;
		[Tooltip ("How long the size of the goal will stay decreased. If the time is negative or zero, it will stay decreased.")]
		/// <summary>
	/// How long the size of the goal will stay decreased. If the time is negative or zero, it will stay decreased.
	/// </summary>
	public float goalDecreaseTime;
		[Tooltip ("The ratio by which the goal will be multiplicated when the size increases.")]
		/// <summary>
	/// The ratio by which the player will be multiplicated when the size increases.
	/// </summary>
	public float playerIncreaseRatio;
		[Tooltip ("How long the player will stay increased. If the time is negative or zero, it will stay increased.")]
		/// <summary>
	/// How long the player will stay increased. If the time is negative or zero, it will stay increased.
	/// </summary>
	public float playerIncreaseTime;
		[Tooltip ("How long the multi-goal status will last. If the time is negative or zero, it will last undefinitely.")]
		/// <summary>
	/// How long the multi-goal status will last. If the time is negative or zero, it will last undefinitely.
	/// </summary>
	public float multiGoalTime;


		void Awake ()
		{
			powerUpDescriptions = PowerUpDescriptionInitialization ();
		}

		private List<PowerUpDescription> PowerUpDescriptionInitialization ()
		{
			var powerUps = Enum.GetValues (typeof(PowerUpType)).Cast<PowerUpType> ();
			var res = new List<PowerUpDescription> ();

			foreach (var powerUp in powerUps) {
				res.Add (new PowerUpDescription (powerUp, true, 1));
			}

			return res;
		}
	}
}
