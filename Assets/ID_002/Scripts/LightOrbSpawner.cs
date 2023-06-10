using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOrbSpawner : MonoBehaviour
{
    [SerializeField] private GameObject lightOrb;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField] private float spawnRadius = 15f;
    [SerializeField] private int maxLightOrbs = 3;

    private bool hasMaxLightOrbs = false;
    private int currentLightOrbs = 0;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnObject), spawnInterval, spawnInterval);
    }

    private void SpawnObject()
    {
        if (hasMaxLightOrbs)
        {
            return;
        }

        if (currentLightOrbs >= maxLightOrbs)
        {
            hasMaxLightOrbs = true;
            return;
        }

        Vector3 playerPosition = playerObject.transform.position;
        Vector3 spawnPosition = new Vector3(
            Random.Range(playerPosition.x - spawnRadius, playerPosition.x + spawnRadius),
            playerPosition.y + 50f,
            Random.Range(playerPosition.z - spawnRadius, playerPosition.z + spawnRadius)
        );

        Instantiate(lightOrb, spawnPosition, Quaternion.identity);

        currentLightOrbs++;
    }
}
