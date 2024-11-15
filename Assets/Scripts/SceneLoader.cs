using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public static void LoadStartScene()
	{
		SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
	}
}
