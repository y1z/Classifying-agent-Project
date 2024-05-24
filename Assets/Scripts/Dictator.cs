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
    public Fruit _currentFruitDemand = Fruit.Apple;
    private  List<Fruit>  possible_fruit_array = new List<Fruit> { Fruit.Apple, Fruit.Banana, Fruit.Guava, Fruit.BlueBerry };
    
    
    private void changeFruit()
    {
        possible_fruit_array.Shuffle();
        int last_item_index = possible_fruit_array.Count - 1;
        _currentFruitDemand = possible_fruit_array[last_item_index];
    }


    public bool isSameFruit(Fruit fruit)
    {
        return _currentFruitDemand == fruit;
    }

    public bool receiveFruit(Fruit fruit)
    {
        bool is_correct_fruit = isSameFruit(fruit);

        changeFruit();

        return is_correct_fruit;
    }
}
