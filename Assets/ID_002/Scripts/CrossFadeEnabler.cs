using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrossFadeEnabler : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public void LoadMainSceneWithTransition()
    {
        StartCoroutine(LoadSceneWithTransition(1));
    }

    public void LoadEndSceneWithTransition()
    {
        StartCoroutine(LoadSceneWithTransition(2));
    }

    IEnumerator LoadSceneWithTransition(int sceneIndex)
    {
        Debug.Log("Animation started!");
        transition.SetTrigger("Start"); // Trigger the crossfade animation

        yield return new WaitForSeconds(transitionTime); // Wait for the crossfade animation to complete

        SceneManager.LoadScene(sceneIndex); // Load the desired scene using SceneManager
    }
}