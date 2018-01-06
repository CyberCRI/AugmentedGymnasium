using UnityEngine;

namespace AugmentedGymnasium
{
	[System.Serializable]
	public class PowerUpDescription
	{
		[Tooltip ("The type of the power-up")]
		/// <summary>
	/// The type of the power-up.
	/// </summary>
	public PowerUpType powerUpType;
		[Tooltip ("Has the power-up been allowed. If false, the power-up will never appear in-game.")]
		/// <summary>
	/// Has the power-up been allowed. If false, the power-up will never appear in-game.
	/// </summary>
	public bool allowed;
		[Tooltip ("The frequency of the power-up. The higher the value, the higher the chances are of seeing this power-up.")]
		/// <summary>
	/// The frequency of the power-up. The higher the value, the higher the chances are of seeing this power-up.
	/// </summary>
	public int weight;

		public PowerUpDescription (PowerUpType powerUpType, bool allowed, int weight)
		{
			this.powerUpType = powerUpType;
			this.allowed = allowed;
			this.weight = weight;
		}
	}
}