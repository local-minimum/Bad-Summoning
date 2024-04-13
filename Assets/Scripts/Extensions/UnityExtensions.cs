using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtensions
{
    public static void DestroyAllChildren(this Transform parent, Action<GameObject> destroy)
    {
        while (parent.childCount > 0)
        {
            Transform child = parent.GetChild(0);
            child.SetParent(null);
            destroy.Invoke(child.gameObject);
        }
    }

    private static void SetAllChildrenVisibility(this Transform parent, bool visible)
    {
        for (int i = 0, n = parent.childCount; i < n; ++i)
        {
            parent.GetChild(i).gameObject.SetActive(visible);
        }
    }

    public static void HideAllChildren(this Transform parent) => parent.SetAllChildrenVisibility(false);


    public static void ShowAllChildren(this Transform parent) => parent.SetAllChildrenVisibility(true);
}

