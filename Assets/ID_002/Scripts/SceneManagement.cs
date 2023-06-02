using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void LoadMainScene()
    {
        Debug.Log("loadmainscene");
        SceneManager.LoadScene(1);
    }
    public void LoadEndScene()
    {
        Debug.Log("loadendscene");
        SceneManager.LoadScene(2);
    }
    public void QuitApplicationNow()
    {
        Application.Quit();
    }
}
