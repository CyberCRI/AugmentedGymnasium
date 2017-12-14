using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongBall : MonoBehaviour {
	Rigidbody2D _rigidbody;

	[SerializeField] private float _defaultSpeedIncrease = 1.1f;
	[SerializeField] private float _maximumSpeed = 10.0f;
	[SerializeField] private float _startingSpeed = 300.0f;

	public float defaultSpeedInscrease { get { return _defaultSpeedIncrease; }}
	public float maximumSpeed { get { return _maximumSpeed; } }
	public float startingSpeed { get { return _startingSpeed; } }

	void Start()
	{
		Init ();
	}

	void Init ()
	{
		Vector2 direction = Random.Range (0.0f, 1.0f) > 0.5f ? Vector2.right : Vector2.left;
		direction += Random.Range (0.0f, 1.0f) > 0.5f ? Vector2.up : Vector2.down;

		_rigidbody = this.GetComponent<Rigidbody2D> ();

		_rigidbody.velocity = Vector2.zero;
		_rigidbody.position = Camera.main.GetComponent<MainCamera> ().bounds.center;


		_rigidbody.AddForce (direction * startingSpeed);
	}

	void Bounce(Side side)
	{
		Vector3 velocity = _rigidbody.velocity;

		switch (side)
		{
		case Side.Top:
		case Side.Bottom:
				velocity.y = -velocity.y;
				break;
		case Side.Left:
		case Side.Right:
			velocity.x = -velocity.x;
			break;	
		}

		_rigidbody.velocity = velocity;
		IncreaseSpeed ();
	}

	public void IncreaseSpeed(float speedIncrease)
	{
		Vector3 velocity = _rigidbody.velocity * speedIncrease;
		float speed = Vector3.Magnitude (velocity);

		Debug.Log (speed);
		// If the speed exceed the maximum speed, we set the velocity to its maximum speed 
		if (speed > maximumSpeed) {
			velocity = velocity.normalized * maximumSpeed;
		}

		_rigidbody.velocity = velocity;
	}
		
	public void IncreaseSpeed()
	{
		IncreaseSpeed (_defaultSpeedIncrease);
	}

	public void Reset()
	{
		Init ();
	}

	void FixedUpdate()
	{
		var bounds = Camera.main.GetComponent<MainCamera> ().bounds;

		if (this.transform.position.y < bounds.min.y)
			Bounce (Side.Bottom);
		if (this.transform.position.y > bounds.max.y)
			Bounce (Side.Top);
		if (this.transform.position.x < bounds.min.x)
			Bounce (Side.Left);
		if (this.transform.position.x > bounds.max.x)
			Bounce (Side.Right);
	}

	public void OnColliderEnter2D(Collision col)
	{
		IncreaseSpeed ();
	}
}
