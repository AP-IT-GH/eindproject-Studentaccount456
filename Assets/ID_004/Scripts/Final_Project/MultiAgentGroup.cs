using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

// Environment Controller
public class MultiAgentGroup : MonoBehaviour
{
    // Add agents to the group using the RegisterAgent(Agent agent) method.
    // All agents in the same group must have the same behavior name and Behavior Parameters
    private SimpleMultiAgentGroup agentGroup;
    private GameObject targetObject;
    private bool isTargetCaught;

    Dictionary<Agent, Vector3> agentStartingPositions = new Dictionary<Agent, Vector3>();
    Dictionary<Agent, Quaternion> agentStartingRotations = new Dictionary<Agent, Quaternion>();
    Vector3 targetStartingPosition;
    Quaternion targetStartingRotation;

    private void Start()
    {
        // Register agents to the group
        foreach (var agent in GetComponentsInChildren<Agent>())
        {
            agentGroup.RegisterAgent(agent);
            agentStartingPositions.Add(agent, agent.transform.position);
            agentStartingRotations.Add(agent, agent.transform.rotation);
        }
        // Find the target object in the scene
        targetObject = GameObject.FindWithTag("Target");
        targetStartingPosition = targetObject.transform.position;
        targetStartingRotation = targetObject.transform.rotation;
    }

    private void Update()
    {
        if (isTargetCaught)
        {
            // End the episode and reset the scene
            agentGroup.EndGroupEpisode();
            ResetScene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetObject)
        {
            // Set a group reward for catching the target
            agentGroup.AddGroupReward(1f);

            // End the episode if both agents have caught the target      
            isTargetCaught = true;         
        }
    }

    private void ResetScene()
    {
        // Reset the target object and agents to their starting positions
        targetObject.transform.position = targetStartingPosition;
        targetObject.transform.rotation = targetStartingRotation;
        foreach (var agent in agentGroup.GetRegisteredAgents())
        {
            agent.transform.position = agentStartingPositions[agent];
            agent.transform.rotation = agentStartingRotations[agent];
            agent.GetComponent<Rigidbody>().velocity = Vector3.zero;
            agent.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        isTargetCaught = false;
    }
}

