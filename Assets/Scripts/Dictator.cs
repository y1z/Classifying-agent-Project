using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/*
 * dictates which fruit type the agent should go after
 */
public sealed class Dictator : MonoBehaviour
{
    public Fruit currentFruitDemand = Fruit.Apple;
    private Fruit prevFruit = Fruit.Apple;

    private List<Fruit> possible_fruit_array = new List<Fruit>
        { Fruit.Apple, Fruit.Banana, Fruit.Guava, Fruit.BlueBerry };

    public List<Target> targets = new List<Target>();

    [SerializeField] private Transform trainingGround;

    // changer 
    private ColorChanger _colorChanger;

    private int possible_fruit_array_index = 0;

    private void Awake()
    {
        _colorChanger = GetComponent<ColorChanger>();
        _colorChanger._color = FruitType.fruitToColor(currentFruitDemand);
    }

    private void Update()
    {
        if (currentFruitDemand != prevFruit)
        {
            _colorChanger._color = FruitType.fruitToColor(currentFruitDemand);
            prevFruit = currentFruitDemand;
        }
    }

    public void changeFruit()
    {
        possible_fruit_array.Shuffle();
        int array_len = possible_fruit_array.Count;
        
        possible_fruit_array_index += UnityEngine.Random.Range(0, array_len - 1);
        possible_fruit_array_index = possible_fruit_array_index % array_len;
        
        currentFruitDemand = possible_fruit_array[possible_fruit_array_index];
    }

    public void changeTargetsFruit()
    {
        int index = possible_fruit_array.Count - 1;
        foreach (var target in targets)
        {
            target.targetData.heldFruit = possible_fruit_array[index];
            index = index - 1;
        }
    }

    public Target desiredTarget()
    {
        Target result = null;
        foreach (var target in targets)
        {
            if(target.targetData.heldFruit == this.currentFruitDemand)
            {
                result = target; break;
            }

        }
        return result;
    }

    public bool isSameFruit(Fruit fruit)
    {
        return currentFruitDemand == fruit;
    }

    public bool receiveFruit(Fruit fruit)
    {
        bool is_correct_fruit = isSameFruit(fruit);

        prevFruit = fruit;
        changeFruit();

        return is_correct_fruit;
    }
}