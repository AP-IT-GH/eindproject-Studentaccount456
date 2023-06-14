using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LightorbCollider : MonoBehaviour
{
    [SerializeField] private Animator mooseAnimator;
    [SerializeField] private Animator bearAnimator;
    [SerializeField] private Animator lightOrbAnimator;
    [SerializeField] private GameObject mooseGhostPrefab;
    [SerializeField] private GameObject bearGhostPrefab;

    private const float MinX = 3f;
    private const float MaxX = 40f;
    private const float Y = 90f;
    private const float MinZ = 22f;
    private const float MaxZ = 95f;
    private const float DelayedY = 10f;
    private const float DelayTime = 2f;

    private List<Rigidbody> rigidbodies = new List<Rigidbody>();
    private List<Animator> animators = new List<Animator>();
    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = GameObject.FindObjectOfType(typeof(ScoreManager)) as ScoreManager;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("MooseGhost"))
        {
            mooseAnimator.SetTrigger("MooseDies");
            lightOrbAnimator.SetTrigger("LightOrbExplode");
            GameObject ghost = InstantiateGhostObject(mooseGhostPrefab);
            Rigidbody ghostRigidbody = ghost.GetComponent<Rigidbody>();
            Animator ghostAnimator = ghost.GetComponent<Animator>();

            if (ghostRigidbody != null)
            {
                ghostRigidbody.velocity = Vector3.zero;
                ghostRigidbody.angularVelocity = Vector3.zero;
                rigidbodies.Add(ghostRigidbody);
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
            lightOrbAnimator.SetTrigger("LightOrbExplode");
            GameObject ghost = InstantiateGhostObject(bearGhostPrefab);
            Rigidbody ghostRigidbody = ghost.GetComponent<Rigidbody>();
            Animator ghostAnimator = ghost.GetComponent<Animator>();

            if (ghostRigidbody != null)
            {
                ghostRigidbody.velocity = Vector3.zero;
                ghostRigidbody.angularVelocity = Vector3.zero;
                rigidbodies.Add(ghostRigidbody);
            }
            if (ghostAnimator != null)
            {
                animators.Add(ghostAnimator);
                ghostAnimator.enabled = false;
            }
        }

        if (!collision.collider.CompareTag("Lightorb"))
        {
            StartCoroutine(DelayedResetPosition(collision.gameObject));
        }
    }

    private GameObject InstantiateGhostObject(GameObject ghostPrefab)
    {
        scoreManager.Reward();
        float randomX = Random.Range(MinX, MaxX);
        float randomZ = Random.Range(MinZ, MaxZ);

        Vector3 position = new Vector3(randomX, Y, randomZ);
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
                originalRigidbody.position = new Vector3(originalRigidbody.position.x, DelayedY, originalRigidbody.position.z);
                rigidbodies.Remove(originalRigidbody);
            }
            else
            {
                Debug.LogError("Rigidbody component not found on the ghost object!");
            }
        }

        foreach (Animator animator in animators)
        {
            if (animator != null)
                animator.enabled = false;
        }
    }

}