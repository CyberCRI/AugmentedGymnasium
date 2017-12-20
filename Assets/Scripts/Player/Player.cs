using UnityEngine;
using System.Collections.Generic;

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
	[Tooltip("The size ratio of the player.")]
	/// <summary>
	/// The size ratio of the player.
	/// </summary>
	[SerializeField] private float _ratio;

	/// <summary>
	/// The velocity of the player
	/// </summary>
	/// <value>The velocity.</value>
	public Vector3 velocity {
		get {
			return _velocity;
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
		}
	}

	public void SetPlayerColor(Color color)
	{
		this.GetComponent<SpriteRenderer> ().color = color;
	}

	void SetPlayerRatio(float ratio)
	{
		
		this._ratio = ratio;
	}

	void Update()
	{
		_velocity = (this.transform.position - _previousPosition) / Time.deltaTime;
		_previousPosition = this.transform.position;
	}
}


