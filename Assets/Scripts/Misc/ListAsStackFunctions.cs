using UnityEngine;

public static class ListAsStackFunctions
{
    public static void Push<T>(this System.Collections.Generic.List<T> list, T item)
    {
        list.Add(item);
    }
    
    public static T Pop<T>(this System.Collections.Generic.List<T> list)
    {
        if (list.Count == 0)
        {
            throw new System.InvalidOperationException("The list is empty.");
        }

        int lastIndex = list.Count - 1;
        T item = list[lastIndex];
        list.RemoveAt(lastIndex);
        return item;
    }

    public static T Peek<T>(this System.Collections.Generic.List<T> list)
    {
        if (list.Count == 0)
        {
            throw new System.InvalidOperationException("The list is empty.");
        }
        return list[list.Count - 1];
    }
}
