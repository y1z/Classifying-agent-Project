using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Assertions.Must;
using Vector3 = UnityEngine.Vector3;

public sealed class CameraScript : MonoBehaviour
{
    public RClassifyingAgentCams agent;
    private Transform agentTransform;
    public Camera agentCamera;
    public Vector3 offset = Vector3.one;
    void Start()
    {
        Assert.IsNotNull(agent,"agent != null");
        agentTransform = agent.transform;
        Assert.IsNotNull(agentTransform,"agentTransform != null");
        Assert.IsNotNull(agentCamera," agentCamera != null");
        
    }
    private void LateUpdate()
    {
        Vector3 agent_position = agentTransform.position;
        transform.position = agent_position;
        agentCamera.transform.position = agent_position;
        offset.x = agent.agentVelocity.x;
        offset.y = 0.0f;
        offset.z = agent.agentVelocity.z;
        agentCamera.transform.LookAt(agent_position +(offset * 2.5f));
    }
}
