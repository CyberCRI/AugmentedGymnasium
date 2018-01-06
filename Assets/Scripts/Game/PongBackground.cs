using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongBackground : MonoBehaviour {
	public LineRenderer linePrefab;
	public PongTeamSetUpArea teamAreaPrefab;

	private List<LineRenderer> _lines = new List<LineRenderer>();
	private List<LineRenderer> _calibrationLines = new List<LineRenderer> ();
	private List<PongTeamSetUpArea> _teamAreas = new List<PongTeamSetUpArea>();

	void InitGamePositions(LineRenderer leftLine,
		LineRenderer topLine,
		LineRenderer rightLine,
		LineRenderer bottomLine,
		LineRenderer middleLine)
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
		middleLine.SetPositions (new Vector3[] { p8, bounds.center, p4 });
	}

	void InitGameColors(LineRenderer leftLine,
		LineRenderer topLine,
		LineRenderer rightLine,
		LineRenderer bottomLine,
		LineRenderer middleLine)
	{
		var team1Color = GameManager.instance.pongTeams [0].color;
		var team2Color = GameManager.instance.pongTeams [1].color;

		leftLine.startColor = leftLine.endColor = team1Color;

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

		rightLine.startColor = rightLine.endColor = team2Color;

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

		middleLine.startColor = middleLine.endColor = Color.white;
	}

	void InitSetUpPositions(List<LineRenderer> lines, List<PongTeamSetUpArea> teamAreas)
	{
		if (lines.Count < 8)
			return;
		
		var bounds = Camera.main.GetComponent<MainCamera> ().bounds;

		var p1 = new Vector3 (bounds.min.x, bounds.min.y, bounds.center.z);
		var p2 = new Vector3 (bounds.center.x, bounds.min.y, bounds.center.z);
		var p3 = new Vector3 (bounds.center.x, bounds.max.y, bounds.center.z);
		var p4 = new Vector3 (bounds.min.x, bounds.max.y, bounds.center.z);

		var p5 = new Vector3 (bounds.max.x, bounds.max.y, bounds.center.z);
		var p6 = new Vector3 (bounds.center.x, bounds.max.y, bounds.center.z);
		var p7 = new Vector3 (bounds.center.x, bounds.min.y, bounds.center.z);
		var p8 = new Vector3 (bounds.max.x, bounds.min.y, bounds.center.z);

		lines [0].SetPositions (new Vector3[] { p1, p2 });
		lines [1].SetPositions (new Vector3[] { p2, p3 });
		lines [2].SetPositions (new Vector3[] { p3, p4 });
		lines [3].SetPositions (new Vector3[] { p4, p1 });

		lines [4].SetPositions (new Vector3[] { p5, p6 });
		lines [5].SetPositions (new Vector3[] { p6, p7 });
		lines [6].SetPositions (new Vector3[] { p7, p8 });
		lines [7].SetPositions (new Vector3[] { p8, p5 });

		StretchSprite (teamAreas [0].gameObject, p1, p3);
		StretchSprite (teamAreas [1].gameObject, p5, p7);
	}

	void InitSetUpColors(List<LineRenderer> lines, List<PongTeamSetUpArea> teamAreas)
	{
		var team1Color = GameManager.instance.pongTeams [0].color;
		var team2Color = GameManager.instance.pongTeams [1].color;

		lines [0].startColor = lines [0].endColor = team1Color;
		lines [1].startColor = lines [1].endColor = team1Color;
		lines [2].startColor = lines [2].endColor = team1Color;
		lines [3].startColor = lines [3].endColor = team1Color;

		lines [4].startColor = lines [4].endColor = team2Color;
		lines [5].startColor = lines [5].endColor = team2Color;
		lines [6].startColor = lines [6].endColor = team2Color;
		lines [7].startColor = lines [7].endColor = team2Color;

		teamAreas [0].GetComponent<SpriteRenderer> ().color = new Color (team1Color.r, team1Color.g, team1Color.b, 0.0f);
		teamAreas [1].GetComponent<SpriteRenderer> ().color = new Color (team2Color.r, team2Color.g, team2Color.b, 0.0f);
	}

	void InitCalibrationPositions(List<LineRenderer> lines)
	{
		var bounds = Camera.main.GetComponent<MainCamera> ().bounds;

		var p1 = new Vector3 (bounds.center.x - bounds.extents.x / 33.33f, bounds.center.y, bounds.center.z);
		var p2 = new Vector3 (bounds.center.x + bounds.extents.x / 33.33f, bounds.center.y, bounds.center.z);

		var p3 = new Vector3 (bounds.center.x, bounds.center.y - bounds.extents.y / 33.33f, bounds.center.z);
		var p4 = new Vector3 (bounds.center.x, bounds.center.y + bounds.extents.y / 33.33f, bounds.center.z);

		var p5 = new Vector2 (2.0f, 1.9f);
		var p6 = new Vector2 (2.0f, 2.1f);

		var p7 = new Vector2 (1.9f, 2.0f);
		var p8 = new Vector2 (2.1f, 2.0f);

		lines [0].SetPositions (new Vector3[] { p2, Vector2.zero });
		lines [1].SetPositions (new Vector3[] { p4, Vector2.zero });
		lines [2].SetPositions (new Vector3[] { p5, p6 });
		lines [3].SetPositions (new Vector3[] { p7, p8 });
	}

	void InitCalibrationColors(List<LineRenderer> lines)
	{
		lines [0].startColor = lines [0].endColor = Color.white;
		lines [1].startColor = lines [1].endColor = Color.white;
	}

	void StretchSprite(GameObject sprite, Vector3 startPos, Vector3 endPos)
	{
		var centerPos = new Vector2 (startPos.x + endPos.x, startPos.y + endPos.y) / 2.0f;

		float scaleX = Mathf.Abs (startPos.x - endPos.x);
		float scaleY = Mathf.Abs (startPos.y - endPos.y);

		sprite.transform.position = centerPos;
		sprite.transform.localScale = new Vector2 (scaleX, scaleY);
	}

	void EmptyList()
	{
		foreach (var line in _lines) {
			Destroy (line.gameObject);
		}

		foreach (var teamArea in _teamAreas) {
			Destroy (teamArea.gameObject);
		}

		_lines.Clear();
		_teamAreas.Clear ();
	}

	public void InitGameBackground()
	{
		EmptyList ();
		_lines = new List<LineRenderer> ();

		for (int i = 0; i < 6; i++) {
			var line = GameObject.Instantiate (linePrefab, this.transform);
			line.positionCount = 3;
			_lines.Add (line);
		}

		InitGamePositions (_lines [0], _lines [1], _lines [2], _lines [3], _lines[4]);
		InitGameColors (_lines [0], _lines [1], _lines [2], _lines [3], _lines[4]);
	}

	public void InitSetUpBackground()
	{
		EmptyList ();
		_lines = new List<LineRenderer> ();

		for (int i = 0; i < 8; i++) {
			var line = GameObject.Instantiate (linePrefab, this.transform);
			line.positionCount = 2;
			_lines.Add (line);
		}

		for (int i = 0; i < 2; i++) {
			var teamArea = GameObject.Instantiate (teamAreaPrefab, this.transform);
			_teamAreas.Add (teamArea);
		}

		_teamAreas [0].pongTeam = GameManager.instance.pongTeams [0];
		_teamAreas [1].pongTeam = GameManager.instance.pongTeams [1];

		InitSetUpPositions (_lines, _teamAreas);
		InitSetUpColors (_lines, _teamAreas);
	}

	public void InitCalibrationBackground()
	{
		foreach (var lines in _calibrationLines) {
			Destroy (lines);
		}

		_lines.Clear ();

		for (int i = 0; i < 4; i++) {
			var line = GameObject.Instantiate (linePrefab, this.transform);
			line.positionCount = 2;
			_calibrationLines.Add (line);
		}

		InitCalibrationPositions (_calibrationLines);
		InitCalibrationColors (_calibrationLines);
	}
}
