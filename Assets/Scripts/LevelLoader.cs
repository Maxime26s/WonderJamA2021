using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//pachinko
public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public Canvas animCanvas;

    public AudioSource buttonAudioSource = null;

    public void LoadMenu()
    {
        if (GameManager.Instance != null && GameManager.Instance.gameObject != null)
            Destroy(GameManager.Instance.gameObject);
        if (PlayerManager.Instance != null && PlayerManager.Instance.gameObject != null)
            Destroy(PlayerManager.Instance.gameObject);

        GameManager.Instance = null;
        PlayerManager.Instance = null;

        StartCoroutine(LoadScene("Menu"));
    }

    public void QuitGame()
    {
        StartCoroutine(Quit());
    }

    public void LoadInstr()
    {
        StartCoroutine(LoadScene("Instructions"));
    }

    public void LoadSelect()
    {
        StartCoroutine(LoadScene("PlayerSelect"));
    }

    public void PlayButtonSound()
    {
        if (buttonAudioSource != null && buttonAudioSource.clip != null)
            buttonAudioSource.PlayOneShot(buttonAudioSource.clip);
    }

    public void Disable()
    {
        animCanvas.gameObject.SetActive(false);
    }

    public void LoadNextLevel(string sceneName = "")
    {
        StartCoroutine(LoadNextLevelCo(sceneName));
    }

    IEnumerator LoadNextLevelCo(string sceneName = "")
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        animCanvas.gameObject.SetActive(true);
        if (transition != null)
            transition.SetTrigger("Fade_out_tr");

        yield return new WaitForSeconds(transitionTime);

        AsyncOperation asyncOperation;
        if (sceneName == "")
            asyncOperation = SceneManager.LoadSceneAsync(currentScene + 1);
        else
            asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        asyncOperation.completed += (_) =>
        {
            GameManager.Instance.InitMap();
        };
    }

    IEnumerator LoadScene(string scene_name)
    {
        animCanvas.gameObject.SetActive(true);
        if (transition != null)
            transition.SetTrigger("Fade_out_tr");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(scene_name);
    }

    IEnumerator Quit()
    {
        animCanvas.gameObject.SetActive(true);
        if (transition != null)
            transition.SetTrigger("Fade_out_tr");

        yield return new WaitForSeconds(transitionTime);

        Application.Quit();
    }

}
