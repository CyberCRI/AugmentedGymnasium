using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongGoal : MonoBehaviour
{
	public delegate void PongGoalEvent(PongGoal pongGoal, PongBall pongBall);
	public static event PongGoalEvent onGoal;

	/// <summary>
	/// At which side is the pong goal
	/// </summary>
	public Side side { get; private set; }


	/// <summary>
	/// The team associated with this goal. When the ball hit the goal, all the opposing team win points.
	/// </summary>
	public PongTeam team { get; private set; }

	/// <summary>
	/// The ratio of the screen that the goal occupies.
	/// </summary>
	public float ratio { get; private set; }

	/// <summary>
	/// Init the values of the pong goal
	/// </summary>
	public void Init (PongTeam team, float ratio, Side side)
	{
		var bounds = Camera.main.GetComponent<MainCamera> ().bounds;
		var position = this.transform.position;

		if (side == Side.Left)
			position.x = bounds.min.x;
		if (side == Side.Right)
			position.x = bounds.max.x;
		this.transform.position = position;

		this.GetComponent<SpriteRenderer> ().color = team.color;

		this.team = team;
		this.side = side;
		this.ratio = ratio;

		ResizeSprite (ratio);
	}

	/// <summary>
	/// Resizes the sprite.
	/// </summary>
	/// <param name="ratio">The ratio of the resize (from 0.0 to 1.0)</param>
	public void ResizeSprite (float ratio)
	{
		var spriteRenderer = GetComponent<SpriteRenderer> ();

		this.transform.localScale = Vector2.one;

		float width = spriteRenderer.sprite.bounds.size.x;

		this.transform.localScale = new Vector3 ((Camera.main.GetComponent<MainCamera>().bounds.extents.y / width) * 2.0f * Mathf.Clamp(ratio, 0.0f, 1.0f), this.transform.localScale.y);
	}

	/// <summary>
	/// Reset the values of the pong goal
	/// </summary>
	public void Reset ()
	{
		Init (this.team, this.ratio, this.side);
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "Ball") {
			if (onGoal != null)
				onGoal (this, col.GetComponent<PongBall>());
		}
	}
}
