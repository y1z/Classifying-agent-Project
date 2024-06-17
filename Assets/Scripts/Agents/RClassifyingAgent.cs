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
    public int _nothingCount = 0;
    public Int64 episodeCount = 0;

    [SerializeField]
    private bool _isNearCorrectTarget = false;

    [SerializeField]
    private int[] _fruitvalues = { 0, 0, 0, 0 };
    [SerializeField]
    private Vector3[] _positions = { Vector3.zero, Vector3.zero , Vector3.zero , Vector3.zero };

    private Vector3 targetGoal = Vector3.zero;


    public const int MAX_NOTHING_COUNT = 4200;


    public override void Initialize()
    {
        rBody = GetComponent<Rigidbody>();
        dictator.currentFruitDemand = Fruit.Apple;
        int i = 0;
        foreach (var VARIABLE in dictator.targets)
        {
            data.Add(VARIABLE.targetData);
            _fruitvalues[i] = (int)VARIABLE.targetData.heldFruit;
            _positions[i] = VARIABLE.targetData.position;
            i++;
        }

        dictator.changeTargetsFruit();
        dictator.receiveFruit(Fruit.Apple);
    }

    private void UpdateFruitValues()
    {
        int i = 0;
        foreach (var VARIABLE in dictator.targets)
        {
            _fruitvalues[i] = (int)VARIABLE.targetData.heldFruit;
            i++;
        }
        
    }
    
    public void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + episodeCount + " Episodes");
    }


    public void Start()
    {
    }

    public void endAgentEpisode()
    {
        episodeCount += 1;
        EndEpisode();
    }

    public override void OnEpisodeBegin()
    {
        moveAgentToCenter();
        dictator.changeTargetsFruit();
        UpdateFruitValues();
        targetGoal = dictator.desiredTarget().targetData.position;
    }

    //funcion para programar los sensores
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition); //3 observations 

        {
            sensor.AddObservation(data[0].position); // 3 observations 
            sensor.AddObservation(_fruitvalues[0]); //
                                                   
            sensor.AddObservation(data[1].position); // 3 observations 
            sensor.AddObservation(_fruitvalues[1]);

            sensor.AddObservation(data[2].position); // 3 observations 
            sensor.AddObservation(_fruitvalues[2]);

            sensor.AddObservation(data[3].position); // 3 observations 
            sensor.AddObservation(_fruitvalues[3]);
            // 12 + 5 in total 
        }

        {

            sensor.AddObservation(targetGoal); // 3 observations 
            sensor.AddObservation((int)dictator.currentFruitDemand);
            // 1 in total 
        }
        sensor.AddObservation(_isNearCorrectTarget); // 1 observation

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


        float final_reward = -0.01f;

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

        bool is_in_range = distanceFromObjective < minimumDistanceFromTarget;
        _isNearCorrectTarget = isCorrectTarget(final_target);
        if (_isNearCorrectTarget && is_in_range)
        {
            final_reward = rewardAmount;
            SetReward(rewardAmount);
            Debug.Log("Did good");
            dictator.changeFruit();
            //dictator.receiveFruit()
            _nothingCount = 0;
            endAgentEpisode();
        }
        else if (!_isNearCorrectTarget && is_in_range)
        {
            final_reward = punishmentAmount * 0.76f;
            SetReward(punishmentAmount * 0.76f);
            Debug.Log("Did small bad");
            dictator.changeFruit();
            _nothingCount = 0;
            endAgentEpisode();
        }
        else if (this.transform.localPosition.y < -0.5f)
        {
            final_reward = punishmentAmount;
            SetReward(punishmentAmount);
            Debug.Log("Did bad");
            dictator.changeFruit();
            _nothingCount = 0;
            endAgentEpisode();
        }

        SetReward(final_reward);
        _nothingCount++;
        if (_nothingCount >= MAX_NOTHING_COUNT)
        {
            SetReward(punishmentAmount * 0.55f);
        }
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