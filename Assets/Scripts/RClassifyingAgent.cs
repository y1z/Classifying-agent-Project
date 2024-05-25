using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

[RequireComponent(typeof(Rigidbody))]
public sealed class RClassifyingAgent : Agent
{
    Rigidbody rBody;
    public List<TargetData> data = new List<TargetData>();
    public Dictator dictator;
    public float multiplicador = 10.0f;
    public float minimumDistanceFromTarget = 1.45f;
    public float rewardAmount = 2.0f;
    public float punishmentAmount = -1.0f;

    public override void Initialize()
    {
        dictator.currentFruitDemand = Fruit.Apple;
        foreach (var VARIABLE in dictator.targets)
        {
            data.Add(VARIABLE.targetData);
        }

        rBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        moveAgentToCenter();
        dictator.changeTargetsFruit();
    }

    //funcion para programar los sensores
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition); //3 observations 

        {
            sensor.AddObservation(data[0].position); // 3 observations 
            sensor.AddObservation(data[1].position); // 3 observations 
            sensor.AddObservation(data[2].position); // 3 observations 
            sensor.AddObservation(data[3].position); // 3 observations 
            // 12 in total 
        }

        {
            sensor.AddObservation((int)data[0].heldFruit);
            sensor.AddObservation((int)data[1].heldFruit);
            sensor.AddObservation((int)data[2].heldFruit);
            sensor.AddObservation((int)data[3].heldFruit);
            sensor.AddObservation((int)dictator.currentFruitDemand);
            // 5 in total 
        }

        sensor.AddObservation(rBody.velocity.x); //1 observations
        sensor.AddObservation(rBody.velocity.z); //1 observations
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actions.ContinuousActions[0];
        controlSignal.z = actions.ContinuousActions[1];
        rBody.AddForce(controlSignal * multiplicador);


        float distanceFromObjective = float.MaxValue;
        Target final_target = null;

        foreach (var target in dictator.targets)
        {
            float currentDistance = Vector3.Distance(transform.localPosition, target.targetData.position);
            if (currentDistance < distanceFromObjective)
            {
                distanceFromObjective = currentDistance;
                final_target = target;
            }
        }

        if (isCorrectTarget(final_target) && distanceFromObjective < minimumDistanceFromTarget)
        {
            SetReward(rewardAmount);
            EndEpisode();
        }
        else if (this.transform.localPosition.y < 0)
        {
            SetReward(punishmentAmount);
            EndEpisode();
        }

        SetReward(-0.01f);
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var conti = actionsOut.ContinuousActions;
        conti[0] = Input.GetAxis("Horizontal");
        conti[1] = Input.GetAxis("Vertical");
    }

    public void moveAgentToCenter()
    {
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(0, 0.5f, 0);
    }

    public bool isCorrectTarget(Target target)
    {
        return dictator.currentFruitDemand == target.targetData.heldFruit;
    }
}