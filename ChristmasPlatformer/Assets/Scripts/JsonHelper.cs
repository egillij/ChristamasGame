using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper
{
    public static T[] FromJson<T>(string json, string levelName)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);

        switch(levelName)
        {
            case "Overall":
                return wrapper.Overall;
            case "Level1":
                return wrapper.Level1;
            case "Level2":
                return wrapper.Level2;
            case "Level3":
                return wrapper.Level3;
            default:
                return wrapper.Overall;
        }        
    }

    public static string ToJson<T>(
        T[] overallArray,
        T[] level1Array,
        T[] level2Array,
        T[] level3Array
    ) {
        Wrapper<T> wrapper = new Wrapper<T>();
            
        wrapper.Overall = overallArray;
        wrapper.Level1 = level1Array;
        wrapper.Level2 = level2Array;
        wrapper.Level3 = level3Array;
        
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(
        T[] overallArray,
        T[] level1Array,
        T[] level2Array,
        T[] level3Array,
        bool prettyPrint
    ) {
        Wrapper<T> wrapper = new Wrapper<T>();

        wrapper.Overall = overallArray;
        wrapper.Level1 = level1Array;
        wrapper.Level2 = level2Array;
        wrapper.Level3 = level3Array;

        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Overall;
        public T[] Level1;
        public T[] Level2;
        public T[] Level3;
    }
}
