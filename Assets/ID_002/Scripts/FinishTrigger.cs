using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    [SerializeField] private GameObject playerObject; 
    [SerializeField] private GameObject cameraObject; 
    [SerializeField] private Animator animator;        

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DisablePlayer();
            EnableHeliCamera();
            TriggerAnimation();
        }
    }

    private void DisablePlayer()
    {
        playerObject.SetActive(false);
    }

    private void EnableHeliCamera()
    {
        cameraObject.SetActive(true);
    }

    private void TriggerAnimation()
    {
        animator.SetTrigger("StartFlying");
    }
}
