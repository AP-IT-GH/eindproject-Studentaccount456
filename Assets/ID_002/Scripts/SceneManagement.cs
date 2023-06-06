using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    private bool startAnimationActivated = false;

    public void LoadMainScene()
    {
        startAnimationActivated = false;
        StartCoroutine(LoadSceneWithTransition(1));
    }

    public void LoadEndScene()
    {
        startAnimationActivated = false;
        StartCoroutine(LoadSceneWithTransition(2));
    }

    public void QuitApplicationNow()
    {
        Application.Quit();
    }

    private IEnumerator LoadSceneWithTransition(int sceneIndex)
    {
        
        transition.SetTrigger("Start");
        Debug.Log("Animation started!");
        yield return new WaitForSeconds(transitionTime);
        if (!startAnimationActivated)
        {
            SceneManager.LoadScene(sceneIndex);
            Debug.Log("Scene loaded");
            ActivateStartAnimation();
            yield break;
        }
        Debug.Log("Timer on!");
        yield return new WaitForSeconds(transitionTime);
        transition.SetTrigger("End");
        Debug.Log("End triggered");
        transition.SetTrigger("BackToNormal");
        Debug.Log("backToNormal");
    }

    public void ActivateStartAnimation()
    {
        startAnimationActivated = true;
        Debug.Log("StartAnimationActivated");
    }
}