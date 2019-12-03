using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static string nextScene;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation asyncOperator = SceneManager.LoadSceneAsync(nextScene);
        asyncOperator.allowSceneActivation = true;
		//float timer = 0.0f;

		//while (!asyncOperator.isDone)
		//{
		//	yield return null;
		//	timer += Time.deltaTime;

		//	progressText.text = ((int)Mathf.Round(progressBar.fillAmount * 100)).ToString() + "%";
		//	if (asyncOperator.progress < 0.9f)
		//	{
		//		progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, asyncOperator.progress, timer);
		//		if (progressBar.fillAmount >= asyncOperator.progress)
		//			timer = 0f;
		//	}
		//	else
		//	{
		//		progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
		//		if (progressBar.fillAmount == 1.0f)
		//		{
		//			asyncOperator.allowSceneActivation = true;
		//			yield break;
		//		}
		//	}
		//}
	}
}
