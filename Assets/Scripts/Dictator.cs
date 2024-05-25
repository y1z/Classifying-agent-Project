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

    private List<Fruit> possible_fruit_array = new List<Fruit>
        { Fruit.Apple, Fruit.Banana, Fruit.Guava, Fruit.BlueBerry };

    public List<Target> targets = new List<Target>();

    [SerializeField] private Transform trainingGround;

    void Start()
    {
        foreach (Transform child in trainingGround)
        {
            if (child.gameObject.CompareTag("target"))
            {
                targets.Add(child.GetComponent<Target>());
            }
        }
    }


    private void changeFruit()
    {
        possible_fruit_array.Shuffle();
        int last_item_index = possible_fruit_array.Count - 1;
        currentFruitDemand = possible_fruit_array[last_item_index];
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

    public bool isSameFruit(Fruit fruit)
    {
        return currentFruitDemand == fruit;
    }

    public bool receiveFruit(Fruit fruit)
    {
        bool is_correct_fruit = isSameFruit(fruit);

        changeFruit();

        return is_correct_fruit;
    }
}