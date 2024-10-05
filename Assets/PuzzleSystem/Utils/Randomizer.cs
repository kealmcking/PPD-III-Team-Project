using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Helper class used specifically for randomizing the chosen murderers objects at the start of the game.
/// </summary>
public static class Randomizer 
{
    /// <summary>
    /// Returns a random object from an array. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="arr"></param>
    /// <returns></returns>
    public static T GetRandomizedObjectFromArray<T>(T[] arr) 
    {
       return arr[Random.Range(0, arr.Length)];
    }
    /// <summary>
    /// Returns an random object from the list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T GetRandomizedObjectFromList<T>(List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
    /// <summary>
    /// returns a random item from a list and removes it from the passed in list reference. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T GetRandomizedObjectFromList<T>(ref List<T> list)
    {
        T item = list[Random.Range(0, list.Count)];
        list.Remove(item);
        return item;
    }
}
