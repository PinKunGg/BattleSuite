using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScene;
    public Slider slider;

    public void LoadLevel(int scene)
    {
        StartCoroutine(LoadAsyn(scene));
    }

    IEnumerator LoadAsyn(int scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

        loadingScene.SetActive(true);

        while(operation.isDone == false)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progress;

            yield return null;
        }
    }
}
