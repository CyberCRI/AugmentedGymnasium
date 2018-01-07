using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodaRTNetCSharp;

namespace AugmentedGymnasium
{
	public class PlayerManager : MonoBehaviour
	{
		private static PlayerManager _instance;
		[SerializeField] private Player _playerPrefab;

		public List<Player> players { get; private set; }

		bool started = false;
		public bool updateInvokeRepeating = false;

		public static PlayerManager instance {
			get { return _instance; }
		}

		public const float codaRatioX = 505.8f;
		public const float codaRatioY = 501.8f;

		public CodaFrame markerFrame;

		void Awake ()
		{
			if (_instance == null)
				_instance = this;
			else {
				Destroy (this.gameObject);
			}
		}

		void Start ()
		{
			players = new List<Player> ();
			Debug.Log ("Initializing Coda system");
			if (CodaRTNetClient.InitSystem ()) {
				if (CodaRTNetClient.StartAcquiring ()) {
					Debug.Log ("StartAcq success");

					uint numMarkers = CodaRTNetClient.getMaxMarkers ();

					for (uint i = 0; i < numMarkers; i++) {
						var player = GameObject.Instantiate (_playerPrefab);
						player.SetMarkerID (i + 1);
					}

					started = true;
					if (updateInvokeRepeating) {
						InvokeRepeating ("CodaFrameUpdate", 1.0f, 0.001f);
					}
				}
			}
		}

		void Update ()
		{
			if (started && !updateInvokeRepeating) {
				var markerFrame = CodaRTNetClient.GetLatestFrame ();
				if (markerFrame != null)
					this.markerFrame = markerFrame;
			}
		}

		void CodaFrameUpdate ()
		{
			if (started && updateInvokeRepeating) {
				var markerFrame = CodaRTNetClient.GetLatestFrame ();
				if (markerFrame != null)
					this.markerFrame = markerFrame;
			}
		}

		void OnDisable ()
		{
			if (started) {
				CodaRTNetClient.StopAcquiring ();
				CodaRTNetClient.StopSystem ();
				Debug.Log ("Client Closed");
				started = false;
			}
		}
	}
}