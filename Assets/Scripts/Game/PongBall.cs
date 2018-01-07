using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AugmentedGymnasium
{
	public class PongBall : MonoBehaviour
	{
		Rigidbody2D _rigidbody;

		[SerializeField] private float _defaultSpeedIncrease = 1.1f;
		[SerializeField] private float _maximumSpeed = 10.0f;
		[SerializeField] private float _startingSpeed = 300.0f;
		[SerializeField] private float _collisionSpeed = 2.0f;

		public float defaultSpeedInscrease { get { return _defaultSpeedIncrease; } }

		public float maximumSpeed { get { return _maximumSpeed; } }

		public float startingSpeed { get { return _startingSpeed; } }

		public void Init ()
		{
			Vector2 direction = Random.Range (0.0f, 1.0f) > 0.5f ? Vector2.right : Vector2.left;
			direction += Random.Range (0.0f, 1.0f) > 0.5f ? Vector2.up : Vector2.down;

			_rigidbody = this.GetComponent<Rigidbody2D> ();

			_rigidbody.velocity = Vector2.zero;
			_rigidbody.position = Camera.main.GetComponent<MainCamera> ().bounds.center;

			this.transform.position = _rigidbody.position;

			_rigidbody.AddForce (direction * startingSpeed);
		}

		/// <summary>
		/// Bounce to the specified side.
		/// </summary>
		/// <param name="side">Side.</param>
		void Bounce (Side side)
		{
			Vector3 velocity = _rigidbody.velocity;

			switch (side) {
			case Side.Top:
			case Side.Bottom:
				velocity.y = -velocity.y;
				break;
			case Side.Left:
			case Side.Right:
				velocity.x = -velocity.x;
				break;	
			}

			_rigidbody.velocity = velocity * _defaultSpeedIncrease;

			AudioManager.instance.PlayBounceSound ();
		}

		/// <summary>
		/// Reset this instance.
		/// </summary>
		public void Reset ()
		{
			Init ();
		}

		void FixedUpdate ()
		{
			var bounds = Camera.main.GetComponent<MainCamera> ().bounds;

			if (!bounds.Contains (this.transform.position)) {
				if (this.transform.position.y < bounds.min.y)
					Bounce (Side.Bottom);
				if (this.transform.position.y > bounds.max.y)
					Bounce (Side.Top);
				if (this.transform.position.x < bounds.min.x)
					Bounce (Side.Left);
				if (this.transform.position.x > bounds.max.x)
					Bounce (Side.Right);
				this.transform.position = bounds.ClosestPoint (this.transform.position);
			}
				
			Vector3 velocity = _rigidbody.velocity;
			float speed = Vector3.Magnitude (velocity);

			// If the speed exceed the maximum speed, we set the velocity to its maximum speed 
			if (speed > maximumSpeed) {
				velocity = velocity.normalized * maximumSpeed;
			}

			_rigidbody.velocity = velocity;

			if (GameManager.instance.magneticField) {
				foreach (var player in PlayerManager.instance.players) {
					var dist = (transform.position - player.transform.position);
					var sqrDist = dist.magnitude * dist.magnitude;
					_rigidbody.velocity += (Vector2)(dist.normalized * (GameManager.instance.powerUpSettings.magneticAlpha / sqrDist));
				}
			}
		}

		/// <summary>
		/// Raises the collision enter2d event.
		/// </summary>
		/// <param name="col">Col.</param>
		public void OnCollisionEnter2D (Collision2D col)
		{
			if (col.gameObject.tag == "Player" && !GameManager.instance.magneticField) {
				AudioManager.instance.PlayImpactSound ();
				_rigidbody.velocity += (Vector2)col.gameObject.GetComponent<IPlayerController> ().velocity * _collisionSpeed;
			}
		}
	}
}
