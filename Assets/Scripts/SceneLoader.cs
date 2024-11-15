using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public static void LoadStartScene()
	{
		SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
	}

	public static void LoadGameScene()
	{
		SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
	}
}
