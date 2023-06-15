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
        LoadSceneWithoutTransition(2);
    }
    public void LoadWinScene()
    {
        StartCoroutine(LoadSceneWithTransition(3));
    }

    public void QuitApplicationNow()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    private IEnumerator LoadSceneWithTransition(int sceneIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneIndex);
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor initially
        Cursor.visible = false; // Make the cursor invisible initially
        yield break;
    }

    void LoadSceneWithoutTransition(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex); // Load the desired scene using SceneManager
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor initially
        Cursor.visible = false; // Make the cursor invisible initially
    }
}