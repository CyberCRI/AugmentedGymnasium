using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	public List<PongGoal> goals { get; private set; }

	public List<PongTeam> pongTeams { get; private set; }

	public List<PongBall> balls { get; private set; }

	[Tooltip("The ratio of the goal at the start of the game.")]
	/// <summary>
	/// The ratio of the goal at the start of the game.
	/// </summary>
	[SerializeField] [Range(0.0f, 1.0f)] private float _startingRatio;

	[Tooltip("Prefab for the goal.")]
	/// <summary>
	/// Prefab for the goal.
	/// </summary>
	public PongGoal goalPrefab;

	[Tooltip("Prefab for the ball.")]
	/// <summary>
	/// Prefab for the ball.
	/// </summary>
	public PongBall ballPrefab;

	/// <summary>
	/// The settings for the power up
	/// </summary>
	public PowerUpSettings powerUpSettings;

	void OnEnable()
	{
		PongGoal.onGoal += OnGoal;
	}

	void OnDisable ()
	{
		PongGoal.onGoal -= OnGoal;
	}

	void Awake()
	{
		if (instance == null)
			instance = this;
		else {
			Destroy (this.gameObject);
		}
	}

	void Start()
	{
		Init ();
	}

	void Init()
	{
		goals = new List<PongGoal> ();
		pongTeams = new List<PongTeam> ();
		balls = new List<PongBall> ();

		var team1 = new PongTeam ("Team 1", Color.cyan);
		var team2 = new PongTeam ("Team 2", Color.magenta);

		var leftGoal = GameObject.Instantiate (goalPrefab);
		var rightGoal = GameObject.Instantiate (goalPrefab);

		leftGoal.Init (team1, _startingRatio, Side.Top);
		rightGoal.Init (team2, _startingRatio, Side.Bottom);

		goals.Add (leftGoal);
		goals.Add (rightGoal);

		pongTeams.Add (team1);
		pongTeams.Add (team2);

		AddBall ();
	}


	void OnGoal (PongGoal receivingGoal, PongBall scoringBall)
	{
		foreach (var team in pongTeams) {
			if (team != receivingGoal.team)
				team.score++;
		}

		balls.Remove (scoringBall);
		Destroy (scoringBall.GetComponent<Rigidbody2D> ());
		Destroy (scoringBall.GetComponent<SpriteRenderer> ());
		Destroy (scoringBall.gameObject);

		Reset ();
	}

	/// <summary>
	/// Add a single ball to the game.
	/// </summary>
	public void AddBall()
	{
		var ball = GameObject.Instantiate (ballPrefab);
		ball.Init ();
		balls.Add (ball);
	}

	/// <summary>
	/// Add a set number of balls to the game.
	/// </summary>
	/// <param name="number">The number of balls that will be added.</param>
	public void AddBall(int number)
	{
		for (int i = 0; i < number; i++) {
			AddBall ();
		}
	}

	IEnumerator TimedGoalSizeIncrease(float multiplicator, PongTeam team)
	{
		foreach (var goal in goals) {
			if (goal.team != team) {
				goal.ratio = goal.ratio * multiplicator;
			}
		}

		if (powerUpSettings.goalIncreaseTime > 0.0f) {
			yield return new WaitForSeconds (powerUpSettings.goalIncreaseTime);

			foreach (var goal in goals) {
				if (goal.team != team) {
					goal.ratio = goal.ratio / multiplicator;
				}
			}
		}
	}

	IEnumerator TimedGoalSizeDecrease(float multiplicator, PongTeam team)
	{
		foreach (var goal in goals) {
			if (goal.team == team) {
				goal.ratio = goal.ratio / multiplicator;
			}
		}

		if (powerUpSettings.goalDecreaseTime > 0.0f) {
			yield return new WaitForSeconds (powerUpSettings.goalIncreaseTime);

			foreach (var goal in goals) {
				if (goal.team == team) {
					goal.ratio = goal.ratio * multiplicator;
				}
			}
		}
	}

	IEnumerator TimedPlayerSizeIncrease (float multiplicator, Player player)
	{
		player.ratio = player.ratio * multiplicator;

		if (powerUpSettings.playerIncreaseTime > 0.0f) {
			yield return new WaitForSeconds (powerUpSettings.playerIncreaseTime);

			player.ratio = player.ratio / multiplicator;
		}
	}

	/// <summary>
	/// Increase the size of all teams opposed to the team that triggered this power up.
	/// </summary>
	/// <param name="multiplicator">Multiplicator of the size.</param>
	/// <param name="team">The team that triggered this power up.</param>
	public void IncreaseGoalSize(float multiplicator, PongTeam team)
	{
		StartCoroutine (TimedGoalSizeIncrease (multiplicator, team));
	}

	/// <summary>
	/// Decrease the size of the goal of the team that triggered this power-up.
	/// </summary>
	/// <param name="multiplicator">Multiplicator.</param>
	/// <param name="team">The team that triggered this power up.</param>
	public void DecreaseGoalSize(float multiplicator, PongTeam team)
	{
		StartCoroutine (TimedGoalSizeDecrease (multiplicator, team));
	}

	/// <summary>
	/// Increase the size of the player that triggered this power-up.
	/// </summary>
	/// <param name="multiplicator">Multiplicator.</param>
	/// <param name="player">The player that triggered this power-up.</param>
	public void IncreasePlayerSize(float multiplicator, Player player)
	{
		StartCoroutine (TimedPlayerSizeIncrease (multiplicator, player));
	}

	/// <summary>
	/// Multiply the number of goals for any team that aren't the team that triggered this power-up
	/// </summary>
	/// <param name="team">The team that triggered this power-up.</param>
	public void MultiGoal(PongTeam team)
	{
		// At first we destroy each temporary goal
		foreach (var goal in goals) {
			if (goal.tempGoal) {
				Destroy (goal);
			}
		}

		// We create a temporary goal at the top if there's no permanent goal already there.
		if (!goals.Exists (x => x.side == Side.Top)) {
			var topGoal = GameObject.Instantiate (goalPrefab);
			topGoal.Init (OpposingTeam (team).PickRandom (), _startingRatio, Side.Top, true);
			if (powerUpSettings.multiGoalTime > 0)
				Destroy (topGoal, powerUpSettings.multiGoalTime);
		}

		// We create a temporary goal at the bottom if there's no permanent goal already there.
		if (!goals.Exists (x => x.side == Side.Bottom)) {
			var bottomGoal = GameObject.Instantiate (goalPrefab);
			bottomGoal.Init (OpposingTeam (team).PickRandom (), _startingRatio, Side.Bottom, true);
			if (powerUpSettings.multiGoalTime > 0)
				Destroy (bottomGoal, powerUpSettings.multiGoalTime);
		}
	}

	/// <summary>
	/// Get the team of the player from the list
	/// </summary>
	/// <returns>The team the player is in</returns>
	/// <param name="player">The player</param>
	public PongTeam GetPlayerTeam(Player player)
	{
		return pongTeams.Find (x => x.players.Contains (player));
	}

	/// <summary>
	/// Returns all teams that are not the given team
	/// </summary>
	/// <returns>The team.</returns>
	/// <param name="team">All of the other teams.</param>
	public List<PongTeam> OpposingTeam(PongTeam team)
	{
		return pongTeams.FindAll (x => x != team);
	}


	public void PowerUp(Player player, PowerUpType powerUpType)
	{
		switch (powerUpType) {
			case PowerUpType.ThreeBall:
				AddBall (3);
				break;
			case PowerUpType.FiveBall:
				AddBall (5);
				break;
			case PowerUpType.GoalSizeIncrease:
				IncreaseGoalSize (powerUpSettings.goalIncreaseRatio, GetPlayerTeam (player));
				break;
			case PowerUpType.GoalSizeDecrease:
				DecreaseGoalSize (powerUpSettings.goalDecreaseRatio, GetPlayerTeam (player));
				break;
			case PowerUpType.PlayerSizeIncrease:
				IncreasePlayerSize (powerUpSettings.playerIncreaseRatio, player);
				break;
			case PowerUpType.MultiGoal:
				MultiGoal (GetPlayerTeam (player));
				break;
		}
	}

	public void Reset()
	{
		if (balls.Count == 0)
			AddBall ();
	}
}
