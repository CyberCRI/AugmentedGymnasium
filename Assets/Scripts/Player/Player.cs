using UnityEngine;
using System.Collections.Generic;
using CodaRTNetCSharp;

namespace AugmentedGymnasium
{
	public class Player : MonoBehaviour, IPlayerController
	{
		/// <summary>
		/// The position of the player at the end of the previous Update message
		/// </summary>
		private Vector3 _previousPosition;
		/// <summary>
		/// The velocity of the player
		/// </summary>
		private Vector3 _velocity;
		/// <summary>
		/// The size ratio of the player.
		/// </summary>
		private float _ratio = 1;
		/// <summary>
		/// The marker identifier.
		/// </summary>
		private uint _markerId;
		[Tooltip ("The color of the player.")]
		/// <summary>
		/// The color of the player.
		/// </summary>
		[SerializeField] private Color _color;

		/// <summary>
		/// The velocity of the player
		/// </summary>
		/// <value>The velocity.</value>
		public Vector3 velocity {
			get {
				return _velocity;
			}
		}

		public Color startingColor { get; private set; }

		/// <summary>
		/// The color of the player
		/// </summary>
		/// <value>The color.</value>
		public Color color {
			get {
				return _color;
			}
			set {
				SetPlayerColor (value);
				_color = value;
			}
		}

		/// <summary>
		/// The size ratio of the player
		/// </summary>
		/// <value>The ratio.</value>
		public float ratio {
			get {
				return _ratio;
			}
			set {
				SetPlayerRatio (value);
				_ratio = value;
			}
		}

		public uint markerID {
			get { return _markerId; }
		}

		public bool isJumping {
			get { return GameManager.instance.hasGameStarted && (currentZ - startingZ) >= jumpDifference; }
		}

		public float startingZ;
		public float currentZ;
		public float jumpDifference = 100.0f;

		void Awake ()
		{
			_color = this.GetComponentInChildren<SpriteRenderer> ().color;
			startingColor = _color;
		}

		public void SetMarkerID (uint markerID)
		{
			_markerId = markerID;
		}

		void SetPlayerColor (Color color)
		{
			this.GetComponent<SpriteRenderer> ().color = color;
		}

		void SetPlayerRatio (float ratio)
		{
			this.transform.localScale *= ratio;
		}

		void SetPosition ()
		{
			CodaFrame markerFrame = PlayerManager.instance.markerFrame;

			if (markerFrame != null && markerFrame.isMarkerVisible (markerID)) {
				Vector3 position = markerFrame.getMarkerPosition (markerID);
				transform.position = new Vector2 (position.x / PlayerManager.codaRatioX, position.y / PlayerManager.codaRatioY);
				currentZ = position.z;
			}
		}
			
		void OnGameStarted ()
		{
			startingZ = currentZ;
			if (GameManager.instance.GetPlayerTeam (this) == null) {
				Destroy (gameObject);
			}
		}
			
		void OnSetUpStarted ()
		{
			color = Color.white;
		}

		void Update ()
		{
			_velocity = (this.transform.position - _previousPosition) / Time.deltaTime;
			_previousPosition = this.transform.position;
			var color = GetComponentInChildren<SpriteRenderer> ().color;
			SetPosition ();
			GetComponent<Collider2D> ().enabled = !isJumping;
			GetComponentInChildren<SpriteRenderer> ().color = new Color(color.r, color.g, color.b, isJumping ? 0.25f : 1.0f);
			GetComponent<Animator> ().SetBool ("MagneticField", GameManager.instance.magneticField);
		}

		void OnEnable()
		{
			GameManager.onGameStarted += OnGameStarted;
			GameManager.onSetUpStarted += OnSetUpStarted;
			PlayerManager.instance.players.Add (this);
		}

		void OnDisable()
		{
			GameManager.onGameStarted -= OnGameStarted;
			GameManager.onSetUpStarted -= OnSetUpStarted;
			PlayerManager.instance.players.Remove (this);
		}
	}
}