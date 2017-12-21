using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	public List<PongGoal> goals { get; private set; }

	public List<PongTeam> pongTeams { get; private set; }

	public List<PongBall> balls { get; private set; }

	public List<Player> players { get; private set; }

	[Tooltip("The ratio of the goal at the start of the game.")]
	/// <summary>
	/// The ratio of the goal at the start of the game.
	/// </summary>
	[SerializeField] [Range(0.0f, 1.0f)] private float _startingRatio;

	[Tooltip("Prefab for the goal.")]
	/// <summary>
	/// Prefab for the goal.
	/// </summary>
	[SerializeField]private PongGoal _goalPrefab;

	[Tooltip("Prefab for the ball.")]
	/// <summary>
	/// Prefab for the ball.
	/// </summary>
	[SerializeField]private PongBall _ballPrefab;
	[Tooltip("Prefab for the player.")]
	/// <summary>
	/// Prefab for the player.
	/// </summary>
	[SerializeField]private Player _playerPrefab;
	[Tooltip("The settings for the power-up.")]
	/// <summary>
	/// The settings for the power ups.
	/// </summary>
	[SerializeField] private PowerUpSettings _powerUpSettings;

	/// <summary>
	/// Gets the power-up settings
	/// </summary>
	public PowerUpSettings powerUpSettings {
		get { return _powerUpSettings; }
	}

	void OnEnable()
	{
		PongGoal.onGoal += OnGoal;
		PowerUp.onPlayerGetPowerUp += OnPlayerGetPowerUp;
	}

	void OnDisable ()
	{
		PongGoal.onGoal -= OnGoal;
		PowerUp.onPlayerGetPowerUp -= OnPlayerGetPowerUp;
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
		players = new List<Player> ();

		var bounds = Camera.main.GetComponent<MainCamera> ().bounds;

		var team1 = new PongTeam ("Team 1", Color.cyan);
		var team2 = new PongTeam ("Team 2", Color.magenta);

		var leftGoal = GameObject.Instantiate (_goalPrefab);
		var rightGoal = GameObject.Instantiate (_goalPrefab);

		var player1 = GameObject.Instantiate (_playerPrefab);
		var player2 = GameObject.Instantiate (_playerPrefab);

		leftGoal.Init (team1, _startingRatio, Side.Left);
		rightGoal.Init (team2, _startingRatio, Side.Right);

		team1.AddPlayer (player1);
		team2.AddPlayer (player2);

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
		var ball = GameObject.Instantiate (_ballPrefab);
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

		if (_powerUpSettings.goalIncreaseTime > 0.0f) {
			yield return new WaitForSeconds (_powerUpSettings.goalIncreaseTime);

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

		if (_powerUpSettings.goalDecreaseTime > 0.0f) {
			yield return new WaitForSeconds (_powerUpSettings.goalIncreaseTime);

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

		if (_powerUpSettings.playerIncreaseTime > 0.0f) {
			yield return new WaitForSeconds (_powerUpSettings.playerIncreaseTime);

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
			var topGoal = GameObject.Instantiate (_goalPrefab);
			topGoal.Init (OpposingTeam (team).PickRandom (), _startingRatio, Side.Top, true);
			if (_powerUpSettings.multiGoalTime > 0)
				Destroy (topGoal.gameObject, _powerUpSettings.multiGoalTime);
		}

		// We create a temporary goal at the bottom if there's no permanent goal already there.
		if (!goals.Exists (x => x.side == Side.Bottom)) {
			var bottomGoal = GameObject.Instantiate (_goalPrefab);
			bottomGoal.Init (OpposingTeam (team).PickRandom (), _startingRatio, Side.Bottom, true);
			if (_powerUpSettings.multiGoalTime > 0)
				Destroy (bottomGoal.gameObject, _powerUpSettings.multiGoalTime);
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

	public void TriggerPowerUp (Player player, PowerUpType powerUpType)
	{
		switch (powerUpType) {
		case PowerUpType.ThreeBall:
			AddBall (3);
			break;
		case PowerUpType.FiveBall:
			AddBall (5);
			break;
		case PowerUpType.GoalSizeIncrease:
			IncreaseGoalSize (_powerUpSettings.goalIncreaseRatio, GetPlayerTeam (player));
			break;
		case PowerUpType.GoalSizeDecrease:
			DecreaseGoalSize (_powerUpSettings.goalDecreaseRatio, GetPlayerTeam (player));
			break;
		case PowerUpType.PlayerSizeIncrease:
			IncreasePlayerSize (_powerUpSettings.playerIncreaseRatio, player);
			break;
		case PowerUpType.MultiGoal:
			MultiGoal (GetPlayerTeam (player));
			break;
		}
	}

	void OnPlayerGetPowerUp(Player player, PowerUp powerUp)
	{
		TriggerPowerUp (player, powerUp.powerUpType);
	}

	public void Reset()
	{
		if (balls.Count == 0)
			AddBall ();
	}
}
