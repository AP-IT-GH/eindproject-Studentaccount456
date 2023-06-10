using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AgentScript : Agent
{
    private GameObject targetObject;
    public Transform Target;

    private Rigidbody agentRigidbody;
    private void Awake()
    {
        agentRigidbody = GetComponent<Rigidbody>();
    }

    public override void Initialize()
    {
        // Find the target object in the scene
        targetObject = GameObject.FindWithTag("Target");
    }

    public override void OnEpisodeBegin()
    {
        // Reset position and oriëntation when agent falls off platform
        if (this.transform.localPosition.y < 0)
        {

            this.transform.localPosition = new Vector3(0, 0.5f, 0); this.transform.localRotation = Quaternion.identity;
        }

        // Move the target to a new random location
        Target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }

    public override void CollectObservations(VectorSensor sensor)
    {    // Agent position    
        sensor.AddObservation(this.transform.localPosition);
    }

    public float speedMultiplier = 0.1f;
    public float rotationMultiplier = 5;

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        int moveX = actionBuffers.DiscreteActions[0];
        int moveZ = actionBuffers.DiscreteActions[1];

        Vector3 addForce = new Vector3(0, 0, 0);

        switch (moveX)
        {
            case 0: addForce.x = 0f; break;
            case 1: addForce.x = -1f; break;
            case 2: addForce.x = +1f; break;
        }

        switch (moveZ)
        {
            case 0: addForce.z = 0f; break;
            case 1: addForce.z = -1f; break;
            case 2: addForce.z = +1f; break;
        }

        float moveSpeed = 5f;
        agentRigidbody.velocity = addForce * moveSpeed + new Vector3(0, agentRigidbody.velocity.y, 0);

        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        // Target reached
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        // Agent fell of platform?
        else if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetObject)
        {
            // Set a reward for catching the target
            SetReward(1f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers ActionsOut)
    {
        ActionSegment<int> discreteActions = ActionsOut.DiscreteActions;

        switch (Mathf.RoundToInt(Input.GetAxisRaw("Horizontal")))
        {
            case -1: discreteActions[0] = 1; break;
            case 0: discreteActions[0] = 0; break;
            case +1: discreteActions[0] = 2; break;
        }
        switch (Mathf.RoundToInt(Input.GetAxisRaw("Vertical")))
        {
            case -1: discreteActions[1] = 1; break;
            case 0: discreteActions[1] = 0; break;
            case +1: discreteActions[1] = 2; break;
        }
    }
}

