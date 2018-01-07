using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AugmentedGymnasium
{
	public class MenuManager : MonoBehaviour
	{
		/// <summary>
		/// Reloads the scene.
		/// </summary>
		public void ReloadScene ()
		{
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}

		/// <summary>
		/// Quits the game.
		/// </summary>
		public void QuitGame ()
		{
			Application.Quit ();
		}
	}
}
