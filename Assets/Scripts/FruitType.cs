using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Fruit
{
    Apple, // red 
    Banana, // yellow
    Guava, // green
    BlueBerry, // blue
    NON_FRUIT, // WHITE indicate error
}

public class FruitType : MonoBehaviour
{
    public Fruit fruit;
    [SerializeField] private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        AssignColor();
    }

    void AssignColor()
    {
        switch (fruit)
        {
            case Fruit.Apple:
                meshRenderer.material.color = Color.red;
                break;
            case Fruit.Banana:
                meshRenderer.material.color = Color.yellow;
                break;
            case Fruit.Guava:
                meshRenderer.material.color = Color.green;
                break;
            case Fruit.BlueBerry:
                meshRenderer.material.color = Color.blue;
                break;
            case Fruit.NON_FRUIT:
                meshRenderer.material.color = Color.white;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}