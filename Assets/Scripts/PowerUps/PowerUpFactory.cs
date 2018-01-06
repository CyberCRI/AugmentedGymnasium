using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AugmentedGymnasium
{
	public class PowerUpFactory : MonoBehaviour
	{
		public PowerUpSettings powerUpSettings {
			get {
				return GameManager.instance.powerUpSettings;
			}
		}

		[Tooltip ("Prefab for the power-up object.")]
	/// <summary>
	/// Prefab for the power-up object.
	/// </summary>
		[SerializeField]private PowerUp _powerUpPrefab;

		private List<PowerUp> powerUpList = new List<PowerUp> ();

		PowerUpType GetRandomPowerUpType ()
		{
			var allowedPowerUps = powerUpSettings.powerUpDescriptions.FindAll (x => x.allowed);
			int sum = allowedPowerUps.Sum (x => x.weight);
			int random = Random.Range (0, sum);
			int currentValue = 0;

			foreach (var powerUp in allowedPowerUps) {
				if (random >= currentValue && random < currentValue + powerUp.weight)
					return powerUp.powerUpType;
				currentValue += powerUp.weight;
			}

			throw new UnityException ("The power-up description table is incorrect");
		}

		IEnumerator CreatePowerUp ()
		{
			var bounds = Camera.main.GetComponent<MainCamera> ().bounds;
			while (true) {
				yield return new WaitForSeconds (Random.Range (powerUpSettings.spawnTimeRange.min, powerUpSettings.spawnTimeRange.max));

				if (powerUpList.Count < powerUpSettings.maxPowerUps) {
					var powerUp = GameObject.Instantiate (
						             _powerUpPrefab,
						             new Vector2 (Random.Range (bounds.min.x, bounds.max.x),
							             Random.Range (bounds.min.y, bounds.max.y)),
						             Quaternion.identity, this.transform
					             );

					powerUp.powerUpType = GetRandomPowerUpType ();
					powerUpList.Add (powerUp);
				}
			}
		}

		void OnPlayerGetPowerUp (Player Player, PowerUp powerUp)
		{
			powerUpList.Remove (powerUp);
		}

		void OnGameStarted ()
		{
			StartCoroutine (CreatePowerUp ());
		}

		void OnGameEnd ()
		{
			StopAllCoroutines ();
		}

		void OnEnable ()
		{
			PowerUp.onPlayerGetPowerUp += OnPlayerGetPowerUp;
			GameManager.onGameStarted += OnGameStarted;
			GameManager.onGameEnd += OnGameEnd;
		}

		void OnDisable ()
		{
			PowerUp.onPlayerGetPowerUp -= OnPlayerGetPowerUp;
			GameManager.onGameStarted -= OnGameStarted;
			GameManager.onGameEnd -= OnGameEnd;
		}
	}
}
