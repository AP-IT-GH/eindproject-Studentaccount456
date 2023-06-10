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
    public Agent otherAgent;


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


    //Adding more observations so the agent can take more informed actions => Improves learning efficiency
    //DANGER: adding too much observations because this can lead to slower training times and worse performance. (We need to keep it balanced)
    public override void CollectObservations(VectorSensor sensor)
    {
        // Agent position
        sensor.AddObservation(this.transform.localPosition);

        // Agent velocity
        sensor.AddObservation(agentRigidbody.velocity);

        // Distance to target
        sensor.AddObservation(Vector3.Distance(this.transform.localPosition, Target.localPosition));

        // We want to include the other agent his position for the enclosing mechanic
        // Other agent position
        sensor.AddObservation(otherAgent.transform.localPosition);
    }

    public float speedMultiplier = 0.1f;
    public float rotationMultiplier = 5;

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float moveX = actionBuffers.ContinuousActions[0];
        float moveZ = actionBuffers.ContinuousActions[1];

        Vector3 addForce = new Vector3(moveX, 0, moveZ);
        float moveSpeed = 5f;
        agentRigidbody.AddForce(addForce * moveSpeed);


        // Calculate distance to target and other agent
        float distanceToOtherAgent = Vector3.Distance(this.transform.localPosition, otherAgent.transform.localPosition);
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);


        // Give a reward if the agent is close to the target and close to another agent at the same time (enclosing)
        if (distanceToTarget < 1.42f && distanceToOtherAgent < 2.0f)
        {
            SetReward(1.0f);
        }

        // One agent is close to the target
        else if (distanceToTarget < 1.42f)
        {
            SetReward(0.5f);
        }

        // Penalize the agent for being far from the target
        else
        {
            SetReward(-0.01f);
        }

        // Agent fell off platform?
        if (this.transform.localPosition.y < 0)
        {
            EndEpisode();  // End the episode
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetObject)
        {
            if (this.CompareTag("agent1"))
            {
                // This agent caught the target, give a big reward
                SetReward(1f);
            }
            else if (this.CompareTag("agent2"))
            {
                // The other agent caught the target, give a big reward
                otherAgent.SetReward(1f);
            }

            // Check if both agents are close to the target
            float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);
            float otherDistanceToTarget = Vector3.Distance(otherAgent.transform.localPosition, Target.localPosition);
            if (distanceToTarget < 1.42f && otherDistanceToTarget < 1.42f)
            {
                // Both agents are close to the target, give a small reward to both
                SetReward(0.1f);
                otherAgent.SetReward(0.1f);
            }

            // End the episode for both agents
            EndEpisode();
            otherAgent.EndEpisode();
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

