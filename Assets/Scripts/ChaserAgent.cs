using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class ChaserAgent : Agent
{
    public float speed = 5f;
    public float rotationSpeed = 5f;
    public float visionRange = 10f;
    public float visionAngle = 45f;

    private Transform player;
    private bool playerInSight = false;

    public override void OnEpisodeBegin()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerInSight = false;
        transform.position = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
        transform.rotation = Quaternion.identity;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Observe player's position relative to agent
        Vector3 directionToPlayer = player.position - transform.position;
        print(sensor);
       sensor.AddObservation(directionToPlayer.normalized);

        // Observe agent's forward vector
       sensor.AddObservation(transform.forward);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Move agent based on action
        float horizontalInput = actions.ContinuousActions[0];
        float verticalInput = actions.ContinuousActions[1];
        transform.Translate(new Vector3(horizontalInput, 0f, verticalInput) * speed * Time.deltaTime);

        // Rotate agent based on action
        float rotationInput = actions.ContinuousActions[2];
        transform.Rotate(Vector3.up, rotationInput * rotationSpeed * Time.deltaTime);

        // Check if player is in sight
        if (CanSeePlayer())
        {
            playerInSight = true;
            SetReward(1f);

            // Add reward to group reward
            Academy.Instance.StatsRecorder.Add("GroupReward", 1f);

            EndEpisode();
        }

        // Chase player if in sight
        if (playerInSight)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            transform.rotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Allow player to control agent
        var continuousActionsOut = actionsOut.ContinuousActions;
       // continuousActionsOut[0] = Input.GetAxis("Horizontal");
      //  continuousActionsOut[1] = Input.GetAxis("Vertical");
      //  continuousActionsOut[2] = Input.GetAxis("Rotation");
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;

        // Check if player is within vision range
        if (directionToPlayer.magnitude > visionRange)
        {
            return false;
        }

        // Check if player is within vision angle
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer > visionAngle)
        {
            return false;
        }

        // Check if there are any obstacles between enemy and player
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, directionToPlayer, out hitInfo))
        {
            if (hitInfo.transform != player)
            {
                return false;
            }
        }

        // If all checks passed, player is in sight
        return true;
    }
}