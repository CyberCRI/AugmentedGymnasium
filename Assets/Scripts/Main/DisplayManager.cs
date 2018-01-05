using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : MonoBehaviour {

	void Start () {
		var UIcamera = ((GameObject)GameObject.FindGameObjectWithTag ("UICamera")).GetComponent<Camera> ();

		Debug.Log ("Displays connected: " + Display.displays.Length);
		if (Display.displays.Length > 1) {
			Display.displays [1].Activate ();
		} else {
			UIcamera.targetDisplay = 0;
			UIcamera.clearFlags = CameraClearFlags.Nothing;
		}
	}
}
