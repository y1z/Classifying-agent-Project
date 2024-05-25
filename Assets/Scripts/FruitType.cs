using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Fruit : int
{
    Apple, // red 
    Banana, // yellow
    Guava, // green
    BlueBerry, // blue
    NON_FRUIT, // WHITE indicate error
}

public sealed class FruitType : MonoBehaviour
{
    public Fruit fruit;
    [SerializeField] private Color currentColor;
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
                currentColor = Color.red;
                break;
            case Fruit.Banana:
                meshRenderer.material.color = Color.yellow;
                currentColor = Color.yellow;
                break;
            case Fruit.Guava:
                meshRenderer.material.color = Color.green;
                currentColor = Color.green;
                break;
            case Fruit.BlueBerry:
                meshRenderer.material.color = Color.blue;
                currentColor = Color.blue;
                break;
            case Fruit.NON_FRUIT:
                meshRenderer.material.color = Color.white;
                currentColor = Color.white;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static int convertFruitToInt(Fruit fruit)
    {
        return (int)fruit;
    }

    public Color color
    {
        get => currentColor;
        private set => currentColor = value;
    }
}