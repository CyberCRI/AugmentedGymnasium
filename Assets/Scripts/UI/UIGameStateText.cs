using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameStateText : MonoBehaviour {
	void Update()
	{
		var team1 = GameManager.instance.pongTeams [0];
		var team2 = GameManager.instance.pongTeams [1];

		if (GameManager.instance.gameState == GameState.SetUp)
			this.GetComponent<Text> ().text = "Choose your team !";
		else if (GameManager.instance.gameState == GameState.SuddenDeath)
			this.GetComponent<Text> ().text = "SUDDEN DEATH !";
		else if (GameManager.instance.gameState == GameState.GameEnd)
			this.GetComponent<Text> ().text = (team1.score > team2.score ? team1.name : team2.name) + " WINS!";
		else {
			this.GetComponent<Text> ().text = "";
		}
	}
}
