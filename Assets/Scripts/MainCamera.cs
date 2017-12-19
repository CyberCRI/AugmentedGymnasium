using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {
	public Bounds bounds { get { return CameraBounds (); } }

	/// <summary>
	/// Returns the bounds of the camera view
	/// </summary>
	/// <returns>The bounds.</returns>
	Bounds CameraBounds()
	{
		float screenAspect = (float)Screen.width / (float)Screen.height;
		float cameraHeight = GetComponent<Camera> ().orthographicSize * 2.0f;
		var bounds = new Bounds (
						 (Vector2)GetComponent<Camera> ().transform.position,
			             new Vector3 (cameraHeight * screenAspect, cameraHeight, 0.0f)
		);
		return bounds;
	}
}
