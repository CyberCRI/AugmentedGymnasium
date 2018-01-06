using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AugmentedGymnasium
{
	public class PongGoal : MonoBehaviour
	{
		public delegate void PongGoalEvent (PongGoal pongGoal, PongBall pongBall);

		public static event PongGoalEvent onGoal;

		/// <summary>
		/// At which side is the pong goal
		/// </summary>
		public Side side { get; private set; }


		/// <summary>
		/// The team associated with this goal. When the ball hit the goal, all the opposing team win points.
		/// </summary>
		public PongTeam team { get; private set; }

		private float _ratio;

		/// <summary>
		/// The ratio of the screen that the goal occupies.
		/// </summary>
		public float ratio {
			get {
				return _ratio;
			}
			set {
				_ratio = value;
				ResizeSprite (value);
			}
		}

		[Tooltip ("If this value is at true, it will be deleted the next time a goal needs to be added.")]
		/// <summary>
	/// If this value is at true, it will be deleted the next time a goal needs to be added.
	/// </summary>
	public bool tempGoal = false;

		void SetGoalSide (Side side)
		{
			var bounds = Camera.main.GetComponent<MainCamera> ().bounds;
			var position = bounds.center;

			switch (side) {
			case Side.Top:
				position.y = bounds.min.y;
				this.transform.Rotate (0.0f, 0.0f, 90.0f);
				break;
			case Side.Right:
				position.x = bounds.max.x;  
				break;
			case Side.Bottom:
				position.y = bounds.max.y;
				this.transform.Rotate (0.0f, 0.0f, 90.0f);
				break;
			case Side.Left:
				position.x = bounds.min.x;
				break;
			}

			this.transform.position = position;
		}

		/// <summary>
		/// Init the values of the pong goal
		/// </summary>
		public void Init (PongTeam team, float ratio, Side side, bool tempGoal = false)
		{
			this.GetComponent<SpriteRenderer> ().color = team.color;

			SetGoalSide (side);

			this.team = team;
			this.side = side;
			this._ratio = ratio;
			this.tempGoal = tempGoal;

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

			this.transform.localScale = new Vector3 ((Camera.main.GetComponent<MainCamera> ().bounds.extents.y / width) * 2.0f * Mathf.Clamp (ratio, 0.0f, 1.0f), this.transform.localScale.y);
		}

		/// <summary>
		/// Reset the values of the pong goal
		/// </summary>
		public void Reset ()
		{
			Init (this.team, this._ratio, this.side);
		}

		void OnTriggerEnter2D (Collider2D col)
		{
			if (col.tag == "Ball") {
				if (onGoal != null)
					onGoal (this, col.GetComponent<PongBall> ());
			}
		}
	}
}