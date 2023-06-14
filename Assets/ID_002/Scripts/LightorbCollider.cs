using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LightorbCollider : MonoBehaviour
{
    [SerializeField] private Animator mooseAnimator;
    [SerializeField] private Animator bearAnimator;
    [SerializeField] private GameObject mooseGhostPrefab;
    [SerializeField] private GameObject bearGhostPrefab;

    private const float MinY = 22f;
    private const float MaxY = 95f;
    private const float MinX = 3f;
    private const float MaxX = 40f;
    private const float Z = 90f;
    private const float DelayedY = 10f;
    private const float DelayTime = 2f;
    // private const int ScoreIncrement = 10;

    private List<Rigidbody> rigidbodies = new List<Rigidbody>();
    private List<Animator> animators = new List<Animator>();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("MooseGhost"))
        {
            mooseAnimator.SetTrigger("MooseDies");
            GameObject ghost = InstantiateGhostObject(mooseGhostPrefab);
            Rigidbody ghostRigidbody = ghost.GetComponent<Rigidbody>();
            Animator ghostAnimator = ghost.GetComponent<Animator>();

            if (ghostRigidbody != null)
            {
                rigidbodies.Add(ghostRigidbody);
                ghostRigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            }
            if (ghostAnimator != null)
            {
                animators.Add(ghostAnimator);
                ghostAnimator.enabled = false;
            }
        }
        else if (collision.collider.CompareTag("BearGhost"))
        {
            bearAnimator.SetTrigger("BearDies");
            GameObject ghost = InstantiateGhostObject(bearGhostPrefab);
            Rigidbody ghostRigidbody = ghost.GetComponent<Rigidbody>();
            Animator ghostAnimator = ghost.GetComponent<Animator>();

            if (ghostRigidbody != null)
            {
                rigidbodies.Add(ghostRigidbody);
                ghostRigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            }
            if (ghostAnimator != null)
            {
                animators.Add(ghostAnimator);
                ghostAnimator.enabled = false;
            }
        }

        //IncreaseScore();
        StartCoroutine(DelayedResetPosition(collision.gameObject));
    }

    private GameObject InstantiateGhostObject(GameObject ghostPrefab)
    {
        float randomX = Random.Range(MinX, MaxX);
        float randomY = Random.Range(MinY, MaxY);

        Vector3 position = new Vector3(randomX, randomY, Z);
        GameObject ghost = Instantiate(ghostPrefab, position, Quaternion.identity);
        return ghost;
    }

    private IEnumerator DelayedResetPosition(GameObject original)
    {
        yield return new WaitForSeconds(DelayTime);

        if (original.CompareTag("MooseGhost") || original.CompareTag("BearGhost"))
        {
            Rigidbody originalRigidbody = original.GetComponent<Rigidbody>();
            if (originalRigidbody != null)
            {
                originalRigidbody.velocity = Vector3.zero;
                originalRigidbody.angularVelocity = Vector3.zero;
                originalRigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                rigidbodies.Add(originalRigidbody);
            }
            else
            {
                Debug.LogError("Rigidbody component not found on the ghost object!");
            }

            original.transform.position = new Vector3(original.transform.position.x, DelayedY, original.transform.position.z);
            original.transform.rotation = Quaternion.identity;

            Debug.Log("Position reset for object: " + original.name);
        }

        foreach (Rigidbody rb in rigidbodies)
        {
            if (rb != null)
                rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }

        foreach (Animator animator in animators)
        {
            if (animator != null)
                animator.enabled = false;
        }
    }
}
