using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongBackground : MonoBehaviour {
	public LineRenderer linePrefab;

	private List<LineRenderer> _lines = new List<LineRenderer>();


	void InitGamePositions(LineRenderer leftLine, LineRenderer topLine, LineRenderer rightLine, LineRenderer bottomLine)
	{
		var bounds = Camera.main.GetComponent<MainCamera> ().bounds;
		var p1 = new Vector3 (bounds.min.x, bounds.min.y, bounds.center.z);
		var p2 = new Vector3 (bounds.min.x, bounds.center.y, bounds.center.z);
		var p3 = new Vector3 (bounds.min.x, bounds.max.y, bounds.center.z);
		var p4 = new Vector3 (bounds.center.x, bounds.max.y, bounds.center.z);
		var p5 = new Vector3 (bounds.max.x, bounds.max.y, bounds.center.z);
		var p6 = new Vector3 (bounds.max.x, bounds.center.y, bounds.center.z);
		var p7 = new Vector3 (bounds.max.x, bounds.min.y, bounds.center.z);
		var p8 = new Vector3 (bounds.center.x, bounds.min.y, bounds.center.z);

		leftLine.SetPositions (new Vector3[] { p1, p2, p3 });
		topLine.SetPositions (new Vector3[] { p3, p4, p5 });
		rightLine.SetPositions (new Vector3[] { p5, p6, p7 });
		bottomLine.SetPositions (new Vector3[] { p7, p8, p1 });
	}

	void InitGameColors(LineRenderer leftLine, LineRenderer topLine, LineRenderer rightLine, LineRenderer bottomLine)
	{
		var team1Color = GameManager.instance.pongTeams [0].color;
		var team2Color = GameManager.instance.pongTeams [1].color;

		leftLine.startColor = team1Color;
		leftLine.endColor = team1Color;

		var topGradient = new Gradient ();
		topGradient.SetKeys(
			new GradientColorKey[] {
				new GradientColorKey(team1Color, 0.0f),
				new GradientColorKey (Color.white, 0.25f),
				new GradientColorKey (Color.white, 0.75f),
				new GradientColorKey(team2Color, 1.0f)
			},
			new GradientAlphaKey[] {
				new GradientAlphaKey(1.0f, 0.0f),
				new GradientAlphaKey(1.0f, 1.0f)
			}
		);
		topLine.colorGradient = topGradient;

		rightLine.startColor = team2Color;
		rightLine.endColor = team2Color;

		var bottomGradient = new Gradient ();
		bottomGradient.SetKeys (
			new GradientColorKey[] {
				new GradientColorKey (team2Color, 0.0f),
				new GradientColorKey (Color.white, 0.25f),
				new GradientColorKey (Color.white, 0.75f),
				new GradientColorKey (team1Color, 1.0f)
			},
			new GradientAlphaKey[] {
				new GradientAlphaKey(1.0f, 0.0f),
				new GradientAlphaKey(1.0f, 1.0f)
			}
		);
		bottomLine.colorGradient = bottomGradient;
	}

	void EmptyList()
	{
		foreach (var line in _lines) {
			Destroy (line.gameObject);
		}

		_lines.Clear();
	}

	public void InitGameBackground()
	{
		EmptyList ();
		_lines = new List<LineRenderer> ();

		for (int i = 0; i < 4; i++) {
			_lines.Add (GameObject.Instantiate (linePrefab, this.transform));
		}

		InitGamePositions (_lines [0], _lines [1], _lines [2], _lines [3]);
		InitGameColors (_lines [0], _lines [1], _lines [2], _lines [3]);
	}

	public void InitSetUpBackground()
	{
	}
}
