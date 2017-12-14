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
	[SerializeField] private float _startingRatio;

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

	public void Start()
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

		var ball = GameObject.Instantiate (ballPrefab);

		leftGoal.Init (team1, _startingRatio, Side.Left);
		rightGoal.Init (team2, _startingRatio, Side.Right);

		goals.Add (leftGoal);
		goals.Add (rightGoal);

		pongTeams.Add (team1);
		pongTeams.Add (team2);

		balls.Add (ball);
	}


	void OnGoal (PongGoal receivingGoal)
	{
		foreach (var team in pongTeams) {
			if (team != receivingGoal.team)
				team.score++;
		}

		Reset ();
	}

	public void Reset()
	{
		foreach (var ball in balls)
			ball.Reset ();
	}
}
