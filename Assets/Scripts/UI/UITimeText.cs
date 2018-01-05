using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimeText : MonoBehaviour {
	void Update ()
	{
		if (GameManager.instance.hasCountdownStarted) {
			int time = GameManager.instance.time;
			this.GetComponent<Text> ().text = (time / 60).ToString ("00") + ":" + (time % 60).ToString ("00");
		} else
			this.GetComponent<Text> ().text = "";
	}
}
