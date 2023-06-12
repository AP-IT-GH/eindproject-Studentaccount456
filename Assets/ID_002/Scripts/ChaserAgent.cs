using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class ChaserAgent : Agent
{
    //Variable to hold the rigidbody of the agent
    Rigidbody rBody;

    private float timer;
    private const float rewardInterval = 10f;

    void Start()
    {
        //Assigning the rigidbody
        rBody = GetComponent<Rigidbody>();
    }


    //Variable to hold the target position
    public Transform Target;
    public override void OnEpisodeBegin()
    {
        timer = rewardInterval;
        // If the Agent fell, zero its momentum
        if (this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(Random.Range(-90f, 36f),
                                           -7.360606f,
                                           Random.Range(-40f, 88f));
        }

        // Move the target to a new spot
        Target.localPosition = new Vector3(Random.Range(-90f, 36f),
                                           -7.360606f,
                                           Random.Range(-40f, 88f));
    }


    public float forceMultiplier = 50;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {

        // Move the agent using the action.
        MoveAgent(actionBuffers.DiscreteActions);

        timer -= Time.deltaTime;
        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        // Reached target
        if (distanceToTarget < 7f)
        {
            SetReward(10.0f);
            EndEpisode();
        }

        // Fell off platform
        else if (this.transform.localPosition.y < -10f)
        {
            EndEpisode();
        }
        else if (timer <= 0)
        {
            SetReward(0.5f);
            timer = rewardInterval;
        }
    }
    public bool CheckCollisionWithObstacle()
    {
   
        Collider[] colliders = this.GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            
            if (collider.CompareTag("Obstacle"))
            {
                return true; 
            }
        }

        return false; 
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = act[0];

        switch (action)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2: 
                break;
            case 3:
                rotateDir = transform.up * 1f;
                break;
            case 4:
                rotateDir = transform.up * -1f;
                break;
            case 5:
                break;
        }
        transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);
        rBody.AddForce(dirToGo * 2,
            ForceMode.VelocityChange);
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 3;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = 4;
        }
    }

}
