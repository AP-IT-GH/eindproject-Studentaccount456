using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents;
using UnityEngine;

public class GreenZoneAgent2 : Agent
{
    public Transform Target;
    public Transform TargetArea;
    private float startDistance;
    public override void OnEpisodeBegin()
    {
        // reset de positie en orientatie als de agent gevallen is
        transform.localPosition = new Vector3(0, 0.5f, 0); this.transform.localRotation = Quaternion.identity;
        Target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
        foundCube = false;
        TargetArea.GetComponent<ZoneTrigger>().TargetEntered = false;
        closestDistanceTargetToZone = Vector3.Distance(TargetArea.localPosition, Target.localPosition);
        startDistance = closestDistanceTargetToZone;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(TargetArea.gameObject.transform.localPosition);
        sensor.AddObservation(foundCube);
    }
    public float speedMultiplier = 0.5f;
    public float rotationMultiplier = 5;
    public bool foundCube;
    public float closestDistanceTargetToZone;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {    // Acties, size = 2    

        Vector3 controlSignal = Vector3.zero;
        controlSignal.z = actionBuffers.ContinuousActions[0];
        transform.Translate(controlSignal * speedMultiplier);
        transform.Rotate(0.0f, rotationMultiplier * actionBuffers.ContinuousActions[1], 0.0f);

        // Beloningen
        float distanceToTarget = Vector3.Distance(transform.localPosition, Target.localPosition);
        //cube vinden
        if (transform.localPosition.y < -1)
        {
            Debug.Log(GetCumulativeReward());
            EndEpisode();
        }
        if (!foundCube)//eerder niet gevonden
        {
            if (distanceToTarget < 1.42f)
            {
                SetReward(0.5f);
                foundCube = true;
                Target.localPosition = new Vector3(Target.localPosition.x, -1, Target.localPosition.z);
            }
            return;
        }

        if (TargetArea.GetComponent<ZoneTrigger>().TargetEntered && foundCube)//target in zone geduuwd
        {

            SetReward(0.5f);
            Debug.Log(GetCumulativeReward());
            EndEpisode();
        }
    }
}