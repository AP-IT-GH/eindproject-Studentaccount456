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
    //[SerializeField] private TextMeshProUGUI scoreText;

    private const float MinY = 22f;
    private const float MaxY = 95f;
    private const float MinX = 3f;
    private const float MaxX = 40f;
    private const float Z = 90f;
    private const float DelayedY = 10f;
    private const float DelayTime = 2f;
    // private const int ScoreIncrement = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lightorb"))
        {
            if (other.CompareTag("MooseGhost"))
            {
                mooseAnimator.SetTrigger("MooseDies");
                InstantiateGhostObject(mooseGhostPrefab);
            }
            else if (other.CompareTag("BearGhost"))
            {
                bearAnimator.SetTrigger("BearDies");
                InstantiateGhostObject(bearGhostPrefab);
            }

            IncreaseScore();
            StartCoroutine(DelayedResetPosition(other.gameObject));
        }
    }

    private void InstantiateGhostObject(GameObject ghostPrefab)
    {
        float randomX = Random.Range(MinX, MaxX);
        float randomY = Random.Range(MinY, MaxY);

        Vector3 position = new Vector3(randomX, randomY, Z);
        Instantiate(ghostPrefab, position, Quaternion.identity);
    }

    private void IncreaseScore()
    {

        // currentScore += ScoreIncrement;
        //scoreText.text = currentScore.ToString();
    }

    private System.Collections.IEnumerator DelayedResetPosition(GameObject original)
    {
        yield return new WaitForSeconds(DelayTime);
        Rigidbody rigidbody = original.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
        else
        {
            Debug.LogError("Voeg RigidBody toe aan component!");
        }

        original.transform.position = new Vector3(original.transform.position.x, DelayedY, original.transform.position.z);
        original.transform.rotation = Quaternion.identity;
    }
}
