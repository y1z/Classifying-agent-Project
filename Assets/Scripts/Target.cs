using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Target : MonoBehaviour
{
    public TargetData targetData;

    private void Awake()
    {
        targetData.position = transform.localPosition;
        targetData.heldFruit = Fruit.NON_FRUIT;
    }
}