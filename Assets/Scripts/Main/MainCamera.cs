﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AugmentedGymnasium
{
	public class MainCamera : MonoBehaviour
	{
		public Bounds bounds { get { return CameraBounds (); } }

		/// <summary>
		/// The ratio of the camera bound. At 1, the bound is the camera screen. At 0.5, it is 2 times smaller than the camera screen.
		/// </summary>
		[Tooltip ("The ratio of the camera bound. At 1, the bound is the camera screen. At 0.5, it is 2 times smaller than the camera screen.")]
		[Range (0.0f, 1.0f)]
		public float ratio = 1.0f;

		/// <summary>
		/// Returns the bounds of the camera view
		/// </summary>
		/// <returns>The bounds.</returns>
		Bounds CameraBounds ()
		{
			int screenWidth = 0;
			int screenHeight = 0;
			try {
				screenWidth = Display.displays [GetComponent<Camera> ().targetDisplay].systemWidth;
				screenHeight = Display.displays [GetComponent<Camera> ().targetDisplay].systemHeight;
			} catch (IndexOutOfRangeException) {
				screenWidth = Screen.width;
				screenHeight = Screen.height;
			}

			float screenAspect = (float)screenWidth / (float)screenHeight;
			float cameraHeight = GetComponent<Camera> ().orthographicSize * 2.0f;
			var bounds = new Bounds (
				            (Vector2)GetComponent<Camera> ().transform.position,
				            new Vector3 (cameraHeight * screenAspect, cameraHeight, 0.0f) * ratio
			            );
			return bounds;
		}
	}
}