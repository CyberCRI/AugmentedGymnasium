using UnityEngine;
using System.Collections.Generic;
using CodaRTNetCSharp;

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
	[Tooltip("The color of the player.")]
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

	public Color startingColor { get; private set;}

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

	void Awake ()
	{
		_color = this.GetComponent<SpriteRenderer> ().color;
		startingColor = _color;
	}

	public void SetMarkerID(uint markerID)
	{
		_markerId = markerID;
	}

	void SetPlayerColor(Color color)
	{
		this.GetComponent<SpriteRenderer> ().color = color;
	}

	void SetPlayerRatio(float ratio)
	{
		this.transform.localScale *= ratio;
	}

	void SetPosition()
	{
		CodaFrame markerFrame = PlayerManager.instance.markerFrame;

		if (markerFrame.isMarkerVisible (markerID)) {
			Vector3 position = markerFrame.getMarkerPosition (markerID);
			Debug.Log (position);
			transform.position = position;
		}
	}

	void Update()
	{
		_velocity = (this.transform.position - _previousPosition) / Time.deltaTime;
		_previousPosition = this.transform.position;
		SetPosition ();
	}
}


