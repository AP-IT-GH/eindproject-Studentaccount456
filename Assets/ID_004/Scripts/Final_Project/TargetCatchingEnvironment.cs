using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

// Environment Controller
public class TargetCatchingEnvironment : MonoBehaviour
{
    // Add agents to the group using the RegisterAgent(Agent agent) method.
    // All agents in the same group must have the same behavior name and Behavior Parameters
    private SimpleMultiAgentGroup agentGroup;
    private GameObject targetObject;
    private bool isTargetCaught;

    private List<Vector3> agentStartingPositions = new List<Vector3>();
    private List<Quaternion> agentStartingRotations = new List<Quaternion>();
    Vector3 targetStartingPosition;
    Quaternion targetStartingRotation;

    private void Start()
    {
        // Register agents to the group
        foreach (var agent in GetComponentsInChildren<Agent>())
        {
            agentGroup.RegisterAgent(agent);
            agentStartingPositions.Add(agent.transform.position);
            agentStartingRotations.Add(agent.transform.rotation);
        }
        // Find the target object in the scene
        targetObject = GameObject.FindWithTag("Target");
        targetStartingPosition = targetObject.transform.position;
        targetStartingRotation = targetObject.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetObject)
        {
            // Set a group reward for catching the target
            agentGroup.AddGroupReward(1f);

            // End the episode if both agents have caught the target
            isTargetCaught = true;
            agentGroup.EndGroupEpisode();  // End the episode
            ResetScene();
        }
    }


    private void ResetScene()
    {
        // Reset the target object and agents to their starting positions
        targetObject.transform.position = targetStartingPosition;
        targetObject.transform.rotation = targetStartingRotation;
        int i = 0;
        foreach (var agent in agentGroup.GetRegisteredAgents())
        {
            agent.transform.position = agentStartingPositions[i];
            agent.transform.rotation = agentStartingRotations[i];
            agent.GetComponent<Rigidbody>().velocity = Vector3.zero;
            agent.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            i++;
        }
        isTargetCaught = false;
    }
}

