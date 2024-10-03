using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public static class Randomizer 
{
    public static T GetRandomizedObjectFromArray<T>(T[] arr) 
    {
       return arr[Random.Range(0, arr.Length)];
    }
    public static T GetRandomizedObjectFromList<T>(List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
    public static T GetRandomizedObjectFromList<T>(ref List<T> list)
    {
        T item = list[Random.Range(0, list.Count)];
        list.Remove(item);
        return item;
    }
}
