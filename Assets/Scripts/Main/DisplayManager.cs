using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : MonoBehaviour {
	private Camera _UICamera;
	private Camera _menuCamera;
	private Camera _mainCamera;

	static bool _activated;

	void Start () {
		_UICamera = ((GameObject)GameObject.FindGameObjectWithTag ("UICamera")).GetComponent<Camera> ();
		_menuCamera = ((GameObject)GameObject.FindGameObjectWithTag ("MenuCamera")).GetComponent<Camera> ();
		_mainCamera = Camera.main;

		Debug.Log ("Displays connected: " + Display.displays.Length);
		if (Display.displays.Length > 2) {
			if (!_activated) {
				Display.displays [1].Activate ();
				Display.displays [2].Activate ();
			}
			_mainCamera.targetDisplay = 1;
			_UICamera.targetDisplay = 2;
		} else if (Display.displays.Length == 2) {
			if (!_activated)
				Display.displays [1].Activate ();
			_mainCamera.targetDisplay = 1;
			_UICamera.targetDisplay = 1;
			_UICamera.clearFlags = CameraClearFlags.Nothing;
		} else {
			_menuCamera.enabled = false;
			GameManager.instance.StartGame ();
		}

		_activated = true;
	}

	public void SwapDisplays()
	{
		int targetDisplay = _mainCamera.targetDisplay;
		_mainCamera.targetDisplay = _UICamera.targetDisplay;
		_UICamera.targetDisplay = targetDisplay;
	}
}
