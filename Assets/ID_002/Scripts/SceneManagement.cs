using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 1f;

    public void LoadMainScene()
    {
        StartCoroutine(LoadSceneWithTransition(1));
    }

    public void LoadDeathScene()
    {
        StartCoroutine(LoadSceneWithTransition(2));
    }
    public void LoadWinScene()
    {
        StartCoroutine(LoadSceneWithTransition(3));
    }

    public void QuitApplicationNow()
    {
        Application.Quit();
    }

    private IEnumerator LoadSceneWithTransition(int sceneIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneIndex);
        yield break;
        
    }
}