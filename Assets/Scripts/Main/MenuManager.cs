using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AugmentedGymnasium
{
	public class MenuManager : MonoBehaviour
	{
		public void ReloadScene ()
		{
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}

		public void QuitGame ()
		{
			Application.Quit ();
		}
	}
}
