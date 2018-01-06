using UnityEngine;
using System.Collections.Generic;

namespace AugmentedGymnasium
{
	[System.Serializable]
	public class PongTeam
	{
		/// <summary>
		/// The name of the team
		/// </summary>
		/// 
		public string name { get; private set; }

		/// <summary>
		/// The color associated with the team.
		/// </summary>
		public Color color { get; private set; }

		/// <summary>
		/// The current score of the team.
		/// </summary>
		public int score = 0;

		/// <summary>
		/// The players in the team.
		/// </summary>
		public List<Player> players { get; private set; }

		/// <summary>
		/// Gets or sets a value indicating whether this team is ready.
		/// </summary>
		/// <value><c>true</c> if ready; otherwise, <c>false</c>.</value>
		public bool ready = false;

		public PongTeam (string name, Color color)
		{
			this.name = name;
			this.color = color;
			this.score = 0;
			this.players = new List<Player> ();
		}

		public PongTeam (string name, Color color, int score, List<Player> players)
		{
			this.name = name;
			this.color = color;
			this.score = score;
			this.players = players;
		}

		/// <summary>
		/// Add a player to the team.
		/// </summary>
		/// <param name="player">The player that will be added</param>
		public void AddPlayer (Player player)
		{
			this.players.Add (player);
		}

		/// <summary>
		/// Remove a player from the team.
		/// </summary>
		/// <param name="player">The player that will be removed</param>
		public void RemovePlayer (Player player)
		{
			this.players.Remove (player);
		}

		/// <summary>
		/// Starts the game for the team.
		/// </summary>
		public void StartGame ()
		{
			foreach (var player in players) {
				player.color = this.color;
			}
		}
	}
}