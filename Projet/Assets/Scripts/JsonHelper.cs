using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// Deprecated. This class was used to wrap other classes in order to serialize them easily. Turned out it wasn't necessary.
/// </summary>
public static class JsonHelper
{   
    /// <summary>
    /// Takes a string in json format and turns it into the specified class.
    /// </summary>
    /// <param name="json">String to deserialize, json format.</param>
    /// <typeparam name="T">Class we want to turn the json into</typeparam>
    /// <returns></returns>
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }
    /// <summary>
    /// Takes an array of class and turns it into a string json formatted.
    /// </summary>
    /// <param name="array">The array to convert</param>
    /// <typeparam name="T">The type of the classes in the array</typeparam>
    /// <returns>Json string</returns>
    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }
    /// <summary>
    /// Takes an array of class and turns it into a string json formatted.
    /// </summary>
    /// <param name="array">The array to convert</param>
    /// <typeparam name="T">The type of the classes in the array</typeparam>
    /// <returns>Json string, pretty printed</returns>
    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }
    /// <summary>
    /// Wrapper class used by the JsonHelper.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
