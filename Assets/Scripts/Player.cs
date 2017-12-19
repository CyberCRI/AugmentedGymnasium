using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour, IPlayerController
{
	public Vector3 _previousPosition;

	public Vector3 _velocity;

	public Vector3 velocity {
		get {
			return _velocity;
		}
	}

	void Update()
	{
		_velocity = (this.transform.position - _previousPosition) / Time.deltaTime;
		_previousPosition = this.transform.position;
	}
}


