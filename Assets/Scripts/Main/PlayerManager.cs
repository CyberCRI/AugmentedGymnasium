using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodaRTNetCSharp;

public class PlayerManager : MonoBehaviour {
	private static PlayerManager _instance;
	[SerializeField] private Player _playerPrefab;

	public List<Player> players { get; private set; }

	bool started = false;
	public bool updateInvokeRepeating = false;

	public static PlayerManager instance {
		get { return _instance; }
	}
		
	public const float codaRatioX = 331.1f;
	public const float codaRatioY = 325.4f;

	public CodaFrame markerFrame;

	void Awake()
	{
		if (_instance == null)
			_instance = this;
		else {
			Destroy (this.gameObject);
		}
	}

	void Start()
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
					players.Add (player);
				}

				started = true;
				if (updateInvokeRepeating) {
					InvokeRepeating ("CodaFrameUpdate", 1.0f, 0.001f);
				}
			}
		}
	}

	void Update()
	{
		if (started && !updateInvokeRepeating) {
			var markerFrame = CodaRTNetClient.GetLatestFrame ();
			if (markerFrame != null)
				this.markerFrame = markerFrame;
		}
	}
	void CodaFrameUpdate()
	{
		if (started && updateInvokeRepeating) {
			var markerFrame = CodaRTNetClient.GetLatestFrame ();
			if (markerFrame != null)
				this.markerFrame = markerFrame;
		}
	}
		
	void OnGameStarted ()
	{
		foreach (var player in players) {
			if (GameManager.instance.GetPlayerTeam(player) == null)
			{
				player.color = new Color (1.0f, 1.0f, 1.0f, 0.25f);
				player.GetComponent<Collider2D> ().enabled = false;
			}
		}	
	}
		
	void OnSetUpStarted ()
	{
		foreach (var player in players) {
			player.color = Color.white;
			player.GetComponent<Collider2D> ().enabled = true;
		}
	}

	void OnEnable()
	{
		GameManager.onGameStarted += OnGameStarted;
		GameManager.onSetUpStarted += OnSetUpStarted;
	}

	void OnDisable()
	{
		GameManager.onGameStarted -= OnGameStarted;
		GameManager.onSetUpStarted -= OnSetUpStarted;
		if (started) {
			CodaRTNetClient.StopAcquiring ();
			CodaRTNetClient.StopSystem ();
			Debug.Log ("Client Closed");
			started = false;
		}
	}
}
