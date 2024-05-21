using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;



public class RAgent : Agent
{
    Rigidbody rBody;
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public Transform Objetivo;
    public override void OnEpisodeBegin()
    {
        //si te caes este va a ser tu punto de inicio
        if (this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
        }

        //mover el objetivo dentro del plano de manera aleatoria
        Objetivo.localPosition = new Vector3(UnityEngine.Random.value * 8 - 4, 0.5f, UnityEngine.Random.value * 8 - 4);
    }

    //funcion para programar los sensores
    public override void CollectObservations(VectorSensor sensor)
    {
        //el agente sepa la posicion del objetivo
        sensor.AddObservation(Objetivo.localPosition); //3 observaciones
        sensor.AddObservation(this.transform.localPosition); //3 observaciones

        //la velocidad del agente
        sensor.AddObservation(rBody.velocity.x);//1 observacion
        sensor.AddObservation(rBody.velocity.z);//1 observacion
    }
    //funcion de acciones y politicas
    public float multiplicador = 10;
    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);
        //programar 2 actuadores
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actions.ContinuousActions[0];
        controlSignal.z = actions.ContinuousActions[1];
        rBody.AddForce(controlSignal * multiplicador);

        //programar las politicas
        float distanciaobjetivo = Vector3.Distance(this.transform.localPosition, Objetivo.localPosition);

        //politica para cuando el agente agarre al objetivo
        if (distanciaobjetivo < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }
        //politica en caso de que el agente sea tan pendejo y se caiga

        else if (this.transform.localPosition.y < 0)
        {
            SetReward(-2.0f);
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
}
