using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
       return arr[UnityEngine.Random.Range(0, arr.Length)];
    }
    /// <summary>
    /// Returns an random object from the list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T GetRandomizedObjectFromList<T>(List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
    /// <summary>
    /// returns a random item from a list and removes it from the passed in list reference. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T GetRandomizedObjectFromListAndRemove<T>(ref List<T> list)
    {
        T item = list[UnityEngine.Random.Range(0, list.Count)];
        list.Remove(item);
        return item;
    }
    public static Suspect GetConditionalRandomizedSuspectFromListAndRemove(ref List<Suspect> list)
    {
        int num = list.Count;
        Func<Suspect, bool> condition = s => !s.IsKiller;
        Suspect item = list
            .OrderBy(s => UnityEngine.Random.Range(0, num))
            .Where(s => condition(s))
            .FirstOrDefault();
        list.Remove(item);
        return item;
    }
    public static List<T> GetRandomizedGroupFromList<T>(List<T> list, int count)
    {
        if (list.Count >= count)
        {
            List<T> newList = list
            .OrderBy(s => UnityEngine.Random.Range(0, list.Count))
            .Take(count)
            .ToList();
            return newList;
        }
        else if (list.Count > 0)
        {
            List<T> newList = list
                .OrderBy(s => UnityEngine.Random.Range(0, list.Count))
                .Take(list.Count)
                .ToList();
            return newList;
        }
        else return null;
            
    }
    public static List<T> GetRandomizedGroupFromListAndRemove<T>(ref List<T> list, int count)
    {
        int num = list.Count;
        if (list.Count >= count)
        {
            List<T> newList = list
            .OrderBy(s => UnityEngine.Random.Range(0,num))
            .Take(count)
            .ToList();
            foreach (var item in newList)
            {
                list.Remove(item);
            }
            return newList;

        }
        else if (list.Count > 0)
        {
            List<T> newList = list
                .OrderBy(s => UnityEngine.Random.Range(0, num))
                .Take(list.Count)
                .ToList();
            foreach (var item in newList)
            {
                list.Remove(item);
            }
            return newList;
        }
        else return null;
        
    }
}
