using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDeath : MonoBehaviour
{
    private SceneManagement sceneManagement;

    private void Start()
    {
        sceneManagement = GameObject.FindObjectOfType(typeof(SceneManagement)) as SceneManagement;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MooseGhost") || collision.gameObject.CompareTag("BearGhost"))
        {
            sceneManagement.LoadDeathScene();
        }
    }
}
