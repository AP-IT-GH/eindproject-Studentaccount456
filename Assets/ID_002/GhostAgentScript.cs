using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class GhostAgentScript : Agent
{
    //Variable to hold the rigidbody of the agent
    Rigidbody rBody;
    private bool hasTouchedTarget;

    //Variable to hold the target position
    public Transform Player;

    private SceneManagement sceneManagement;

    public float forceMultiplier = 50;

    // Reference to the terrain
    private Vector3 startingPos;



    // Start is called before the first frame update
    void Start()
    {
        //Assigning the rigidbody
        rBody = GetComponent<Rigidbody>();
        sceneManagement = GameObject.FindObjectOfType(typeof(SceneManagement)) as SceneManagement;
        this.startingPos = this.transform.position;
    }

    public override void OnEpisodeBegin()
    {
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;

        // Randomly position the agent on the terrain at the start of each episode
        this.transform.position = startingPos;
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {

        // Move the agent using the action.
        MoveAgent(actionBuffers.DiscreteActions);

        // Reward for each step
        SetReward(-0.01f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!hasTouchedTarget)
            {
                Debug.Log("rewarded");
                SetReward(1.0f);
                hasTouchedTarget = true;
                sceneManagement.LoadDeathScene();

            };
            EndEpisode();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Touches wall");
            AddReward(-0.5f); // Negative reward for touching a wall
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Keeps touching the wall");
            AddReward(-0.1f); // Negative reward for touching a wall
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
                dirToGo = transform.forward * forceMultiplier;
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
        rBody.AddForce(dirToGo * 2, ForceMode.Force);
    }
}
