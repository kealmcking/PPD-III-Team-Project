using UnityEngine;

public static class Randomizer 
{
    public static T RandomizeArray<T>(T[] arr) 
    {
       return arr[Random.Range(0, arr.Length)];
    }
}
