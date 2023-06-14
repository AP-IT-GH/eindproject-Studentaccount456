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
    //private bool isTargetCaught;
    private AgentScript agent1;
    private AgentScript agent2;

    private List<Vector3> agentStartingPositions = new List<Vector3>();
    private List<Quaternion> agentStartingRotations = new List<Quaternion>();
    Vector3 targetStartingPosition;
    Quaternion targetStartingRotation;

    private void Start()
    {
        // Find the target object in the scene
        targetObject = GameObject.FindWithTag("Target");

        // Find the two agents in the scene
        agent1 = GameObject.Find("Agent1").GetComponent<AgentScript>();
        agent2 = GameObject.Find("Agent2").GetComponent<AgentScript>();

        // Register the two agents to a new SimpleMultiAgentGroup
        agentGroup = new SimpleMultiAgentGroup();
        agentGroup.RegisterAgent(agent1);
        agentGroup.RegisterAgent(agent2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetObject)
        {
            // Set a group reward for catching the target
            agentGroup.AddGroupReward(1f);

            // End the episode if both agents have caught the target
            //isTargetCaught = true;
            agentGroup.EndGroupEpisode();  // End the episode
            ResetScene();
        }
    }


    private void ResetScene()
    {
        // Reset the target object and agents to their starting positions
        // (random postions not yet implemented)
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
        //isTargetCaught = false;

    }


}

