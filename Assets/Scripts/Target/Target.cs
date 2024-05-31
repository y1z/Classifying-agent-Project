using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Target : MonoBehaviour
{
    public TargetData targetData;
    public ColorChanger colorChanger;

    private void Awake()
    {
        colorChanger = GetComponent<ColorChanger>();
        targetData.position = transform.localPosition;
        targetData.heldFruit = Fruit.NON_FRUIT;
    }

    private void Update()
    {
        colorChanger._color = FruitType.fruitToColor(targetData.heldFruit);

    }
}