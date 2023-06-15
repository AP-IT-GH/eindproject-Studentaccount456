using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class ChaseAgentCollab : Agent
{
    public ChaseAgentEnvController envController;
    private Rigidbody rBody;

    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Check if the agent has fallen off the plane
        if (this.transform.localPosition.y < envController.ground.transform.localPosition.y)
        {
            if (!envController.HasAgentFallen(this))
            {
                SetReward(-1.0f);
                // Increment the fallen agents count
                envController.OnAgentFell(this);
            }
        }
        else
        {
            // Move the agent using the action.
            MoveAgent(actionBuffers.DiscreteActions);
        }
  
    }

    public void AssignEnvController(ChaseAgentEnvController controller)
    {
        envController = controller;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            SetReward(1.0f);
            envController.OnAgentTargetCollision(this);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-0.5f); // Negative reward for touching a wall
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-0.1f); // Negative reward for touching a wall
        }
    }

    private void MoveAgent(ActionSegment<int> act)
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
