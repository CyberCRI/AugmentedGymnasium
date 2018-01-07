using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	private static AudioManager _instance;

	public static AudioManager instance {
		get { return _instance; }
	}

	public AudioSource bounceSound;
	public AudioSource goalSound;
	public AudioSource bonusSound;
	public AudioSource endSound;

	void Awake ()
	{
		if (_instance == null)
			_instance = this;
		else {
			Destroy (this.gameObject);
		}
	}

	public void PlayBounceSound()
	{
		bounceSound.Play ();
	}

	public void PlayGoalSound()
	{
		goalSound.Play ();
	}

	public void PlayBonusSound()
	{
		bonusSound.Play ();
	}

	public void PlayEndSound()
	{
		endSound.Play ();
	}
}
