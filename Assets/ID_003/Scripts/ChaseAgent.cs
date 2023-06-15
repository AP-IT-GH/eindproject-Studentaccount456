using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ChaseAgent : Agent
{
    //Variable to hold the rigidbody of the agent
    Rigidbody rBody;
    private bool hasTouchedTarget;


    public GameObject groundPlane; // Reference to the ground plane object
    private Bounds areaBounds;


    void Start()
    {
        //Assigning the rigidbody
        rBody = GetComponent<Rigidbody>();
        areaBounds = groundPlane.GetComponent<Collider>().bounds;
    }


    //Variable to hold the target position
    public Transform Target;
    public override void OnEpisodeBegin()
    {
        // If the Agent fell, zero its momentum
        if (this.transform.localPosition.y < groundPlane.transform.localPosition.y)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = GetRandomPositionOnGround();
        }

        // Move the target to a new spot on the ground plane
        Target.localPosition = GetRandomPositionOnGround();
    }

    private Vector3 GetRandomPositionOnGround()
    {

        // Calculate the random spawn position within the ground plane bounds
        float xSpawn = Random.Range(-areaBounds.extents.x, areaBounds.extents.x);
        float zSpawn = Random.Range(-areaBounds.extents.z, areaBounds.extents.z);
        float ySpawn = areaBounds.extents.y +  2.5f; // Add a small offset to spawn on top

        Vector3 spawnPos = new Vector3(xSpawn, ySpawn, zSpawn);

        return spawnPos;
    }


    public float forceMultiplier = 50;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {

        // Move the agent using the action.
        MoveAgent(actionBuffers.DiscreteActions);

        // Reward for each step
        SetReward(-0.01f);

        // Check if the agent has fallen off the plane
        if (this.transform.localPosition.y < groundPlane.transform.localPosition.y)
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            if (!hasTouchedTarget)
            {
                Debug.Log("rewarded");
                SetReward(1.0f);
                hasTouchedTarget = true;
            }
            EndEpisode();
        }
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var moveAction = act[0];
        var rotateAction = act[1];

        // Determine movement direction
        switch (moveAction)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                break;
        }

        // Determine rotation direction
        switch (rotateAction)
        {
            case 1:
                rotateDir = transform.up * 1f;
                break;
            case 2:
                rotateDir = transform.up * -1f;
                break;
            case 3:
                break;
        }

        transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);
        rBody.AddForce(dirToGo * 2, ForceMode.VelocityChange);
    }



    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[1] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[1] = 2;
        }
    }

}
