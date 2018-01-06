using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AugmentedGymnasium
{
	public class UIScoreText : MonoBehaviour
	{
		void Update ()
		{
			if (GameManager.instance.hasGameStarted) {
				var team1 = GameManager.instance.pongTeams [0];
				var team2 = GameManager.instance.pongTeams [1];

				this.GetComponent<Text> ().text = "<color=" + team1.color.ToHex () + ">" + team1.name + "</color>" + " - " + team1.score
				+ " | " + "<color=" + team2.color.ToHex () + ">" + team2.name + "</color>" + " - " + team2.score;
			} else {
				this.GetComponent<Text> ().text = "";
			}
		}
	}
}